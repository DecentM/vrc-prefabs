using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components.Video;

namespace DecentM.Video.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), AddComponentMenu("DecentM/Video/Plugins/SoundEffects")]
    internal sealed class SoundEffectsPlugin : VideoPlugin
    {
        [SerializeField] private AudioSource audioSource;

        [Space]
        [SerializeField] private AudioClip init;
        [SerializeField] private AudioClip loadBegin;
        [SerializeField] private AudioClip loadReady;
        [SerializeField] private AudioClip videoUnloaded;
        [SerializeField] private AudioClip error;
        [SerializeField] private AudioClip play;
        [SerializeField] private AudioClip autoRetry;

        private void PlaySound(AudioClip clip)
        {
            if (this.system.IsPlaying())
                return;

            this.audioSource.PlayOneShot(clip);
        }

        protected override void OnVideoPlayerInit()
        {
            this.PlaySound(this.init);
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

        protected override void OnLoadError(VideoError error)
        {
            this.PlaySound(this.error);
        }

        protected override void OnPlay(float timestamp)
        {
            this.PlaySound(this.play);
        }

        protected override void OnCustomVideoEvent(string name)
        {
            if (name == "OnAutoRetryLoadTimeout")
                this.PlaySound(this.autoRetry);
        }
    }
}
