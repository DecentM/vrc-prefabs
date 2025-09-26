using UdonSharp;
using UnityEngine;

namespace DecentM.Video.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    internal sealed class SoundEffectsPlugin : VideoPlugin
    {
        [SerializeField] private AudioSource audioSource;

        [Space]
        [SerializeField] private AudioClip loadBegin;
        [SerializeField] private AudioClip loadReady;
        [SerializeField] private AudioClip videoUnloaded;

        private void PlaySound(AudioClip clip)
        {
            if (this.system.IsPlaying())
                return;

            this.audioSource.PlayOneShot(clip);
        }

        protected override void OnLoadBegin()
        {
            this.PlaySound(this.loadBegin);
        }

        protected override void OnLoadReady(float duration)
        {
            this.PlaySound(this.loadReady);
        }

        protected override void OnStop()
        {
            this.PlaySound(this.videoUnloaded);
        }
    }
}
