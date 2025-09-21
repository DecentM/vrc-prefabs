using VRC.SDKBase;
using UnityEngine;

namespace DecentM.Video.Plugins
{
    /// <summary>
    /// Causes the video player to automatically start playing a video once it loads.
    /// If there are multiple players in the world, it'll wait for everyone to load before starting playback.
    /// </summary>
    internal sealed class AutoPlayPlugin : VideoPlugin
    {
        [SerializeField] private bool autoplayOnLoad = true;

        private bool isOwner
        {
            get { return Networking.GetOwner(this.gameObject) == Networking.LocalPlayer; }
        }

        private int receivedLoadedFrom = 0;

        protected override void OnLoadReady(float duration)
        {
            if (VRCPlayerApi.GetPlayerCount() == 1)
            {
                this.system.Play();
                return;
            }

            if (this.isOwner)
                return;

            this.SendCustomNetworkEvent(
                VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner,
                nameof(this.OnClientLoaded)
            );
        }

        // Everyone except the owner does this, because then owner already knows when it finishes loading
        public void OnClientLoaded()
        {
            if (!this.isOwner || !this.autoplayOnLoad)
                return;

            this.receivedLoadedFrom++;

            this.events.OnRemotePlayerLoaded(this.receivedLoadedFrom);

            // Everyone is loaded when this counter is above the player count
            // (skipping one for the local player)
            if (this.receivedLoadedFrom < VRCPlayerApi.GetPlayerCount() - 1)
                return;

            this.system.Play();
        }

        protected override void OnLoadRequested(VRCUrl url)
        {
            if (!this.isOwner || !this.autoplayOnLoad)
                return;

            this.receivedLoadedFrom = 0;
        }
    }
}