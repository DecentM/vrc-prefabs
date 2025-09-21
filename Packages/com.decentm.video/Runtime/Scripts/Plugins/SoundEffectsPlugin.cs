using UdonSharp;
using UnityEngine;

namespace DecentM.Video.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    internal sealed class SoundEffectsPlugin : VideoPlugin
    {
        [SerializeField] private AudioSource audioSource;

        [Space]
        [SerializeField] private AudioClip autoRetry;
        [SerializeField] private AudioClip autoRetryAbort;
        [SerializeField] private AudioClip loadReady;
        [SerializeField] private AudioClip remotePlayerLoaded;
        [SerializeField] private AudioClip videoUnloaded;

        private void PlaySound(AudioClip clip)
        {
            if (this.system.IsPlaying())
                return;

            this.audioSource.PlayOneShot(clip);
        }

        protected override void OnAutoRetry(int attempt)
        {
            this.PlaySound(this.autoRetry);
        }

        protected override void OnAutoRetryAbort()
        {
            this.PlaySound(this.autoRetryAbort);
        }

        protected override void OnLoadReady(float duration)
        {
            this.PlaySound(this.loadReady);
        }

        protected override void OnRemotePlayerLoaded(int loadedPlayers)
        {
            this.PlaySound(this.remotePlayerLoaded);
        }

        protected override void OnStop()
        {
            this.PlaySound(this.videoUnloaded);
        }
    }
}
