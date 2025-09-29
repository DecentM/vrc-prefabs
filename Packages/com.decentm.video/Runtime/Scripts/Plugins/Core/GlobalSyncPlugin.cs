using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace DecentM.Video.Plugins
{
    /// <summary>
    /// Causes the video player state to be synced to other players in the world. <br/>
    /// Includes ownership locking, and drift correction.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual), AddComponentMenu("DecentM/Video/Plugins/GlobalSync")]
    internal sealed class GlobalSyncPlugin : VideoPlugin
    {
        [Tooltip("If the amount of drift is larger than this, the plugin will jump to the synced position."), SerializeField] private float diffToleranceSeconds = 2f;

        private float latency = 0.1f;

        [UdonSynced, FieldChangeCallback(nameof(progress))]
        private float _progress;

        internal float progress
        {
            set
            {
                if (this.system.IsLocallyOwned())
                    return;

                float desiredProgress = value + this.latency;

                _progress = value;
                float localProgress = this.system.GetTime();
                float diff = desiredProgress - localProgress;

                // While paused we accept any seek input from the owner
                if (!system.IsPlaying())
                    this.system.Seek(value);

                // If diff is positive, the local player is ahead of the remote one
                if (diff > diffToleranceSeconds || diff < diffToleranceSeconds * -1)
                    this.system.Seek(desiredProgress);
            }
            get => _progress;
        }

        [UdonSynced, FieldChangeCallback(nameof(isPlaying))]
        private bool _isPlaying;

        internal bool isPlaying
        {
            set
            {
                if (this.system.IsLocallyOwned())
                    return;

                _isPlaying = value;
                if (value)
                    this.system.Play(this.system.GetTime() + this.latency);
                else
                    this.system.Play();
            }
            get => _isPlaying;
        }

        [UdonSynced, FieldChangeCallback(nameof(url))]
        private VRCUrl _url;

        internal VRCUrl url
        {
            set
            {
                if (this.system.IsLocallyOwned())
                    return;

                if (value == null || string.IsNullOrEmpty(value.ToString()))
                    return;

                _url = value;

                this.system.LoadVideo(value);
            }
            get => _url;
        }

        protected override void OnProgress(float timestamp, float duration)
        {
            if (!this.system.IsLocallyOwned())
                return;

            this._progress = timestamp;
            this.RequestSerialization();
        }

        protected override void OnLoadRequested(VRCUrl url)
        {
            if (url == null)
                return;

            if (!this.system.IsLocallyOwned())
            {
                this.system.DenyVideoRequest(url);
                return;
            }

            this._url = url;
            this.RequestSerialization();
        }

        public void SyncUnload()
        {
            if (this.system.IsLocallyOwned())
                return;

            this.system.Stop();
        }

        protected override void OnStop()
        {
            this.latency = 0;

            if (!this.system.IsLocallyOwned())
                return;

            this.SendCustomNetworkEvent(
                VRC.Udon.Common.Interfaces.NetworkEventTarget.All,
                nameof(this.SyncUnload)
            );

            this._url = null;
            this._isPlaying = false;
            this.RequestSerialization();
        }

        protected override void OnPlay(float timestamp)
        {
            if (!this.system.IsLocallyOwned())
                return;

            this._isPlaying = true;
            this.RequestSerialization();
        }

        protected override void OnPause(float timestamp)
        {
            if (!this.system.IsLocallyOwned())
            {
                this.SyncPausedTime();
                return;
            }

            this._progress = timestamp;
            this._isPlaying = false;
            this.RequestSerialization();
        }

        private void SyncPausedTime()
        {
            if (this.system.IsLocallyOwned())
                return;

            // Continuously refine the latency by calculating the diff between the synced progress and the local progress
            // Next time that "play" is pressed, the stream will be offset by this amount
            float currentTime = this.system.GetTime();
            float diff = this.progress - currentTime;
            this.latency += diff;

            if (this.system.IsPlaying())
                this.system.Pause(this.progress);
        }

        protected override void OnLoadReady(float duration)
        {
            if (this.system.IsLocallyOwned())
                return;

            if (this.isPlaying)
                this.system.Pause();
        }

        protected override void OnVideoPlayerInit()
        {
            VRCPlayerApi owner = Networking.GetOwner(this.gameObject);
            if (owner == null || !owner.IsValid())
                return;

            this.system.SetOwner(owner);
            this.events.OnCustomVideoEvent("OnOwnershipChanged", new object[] { owner });
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            if (player == null || !player.IsValid())
                return;

            this.events.OnCustomVideoEvent("OnOwnershipChanged", new object[] { player });
            this.system.SetOwner(player);
        }

        private bool ownershipLocked = false;

        public override bool OnOwnershipRequest(
            VRCPlayerApi requestingPlayer,
            VRCPlayerApi requestedOwner
        )
        {
            if (this.ownershipLocked)
                return false;

            if (requestingPlayer == null || !requestingPlayer.IsValid())
                return false;
            if (requestedOwner == null || !requestedOwner.IsValid())
                return false;

            return requestingPlayer.playerId == requestedOwner.playerId;
        }

        protected override void OnCustomVideoEvent(string name)
        {
            if (name == "OnOwnershipRequested")
                Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        }

        protected override void OnCustomVideoEvent(string name, object[] data)
        {
            if (name == "OnOwnershipSecurityChanged")
            {
                bool locked = (bool)data[0];
                this.ownershipLocked = locked;
            }
        }
    }
}

