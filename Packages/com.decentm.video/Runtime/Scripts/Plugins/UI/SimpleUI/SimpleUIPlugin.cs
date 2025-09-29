using UnityEngine;

using UnityEngine.UI;
using VRC.SDK3.Components;
using UdonSharp;
using VRC.SDKBase;

namespace DecentM.Video.Plugins
{
    /// <summary>
    /// Implements a minimal UI to control the video player using its events
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), AddComponentMenu("DecentM/Video/Plugins/UI/SimpleUIPlugin")]
    internal sealed class SimpleUIPlugin : VideoPlugin
    {
        [Space]
        [SerializeField] private Animator animator;

        [Space]
        [SerializeField] private VRCUrlInputField urlInput;

        [Space]
        [SerializeField] private Slider progress;
        [SerializeField] private Slider brightness;
        [SerializeField] private Slider volume;

        protected override void _Start()
        {
            this.volume.SetValueWithoutNotify(this.system.GetVolume());
            this.brightness.SetValueWithoutNotify(this.system.GetBrightness());
        }

        protected override void OnProgress(float timestamp, float duration)
        {

            if (!float.IsInfinity(timestamp) && !float.IsInfinity(duration))
                this.progress.SetValueWithoutNotify(Mathf.Max(timestamp / duration, 0.001f));
        }

        public void HandleUrlEditEnd()
        {
            VRCUrl url = this.urlInput.GetUrl();
            this.system.RequestVideo(url);
        }

        public void HandlePlay()
        {
            this.system.Play();
        }

        public void HandleSeek()
        {
            this.system.Seek(this.progress.normalizedValue * this.system.GetDuration());
        }

        public void HandlePause()
        {
            this.system.Pause();
        }

        public void HandleStop()
        {
            this.system.Stop();
        }

        protected override void OnLoadRequested(VRCUrl url)
        {
            this.animator.SetBool("Loading", true);
        }

        protected override void OnLoadReady(float duration)
        {
            this.animator.SetBool("Loading", false);
        }

        protected override void OnStop()
        {
            this.animator.SetBool("Loading", false);
            this.animator.SetBool("ShowControls", true);
            this.urlInput.SetUrl(VRCUrl.Empty);
        }

        protected override void OnPlay(float timestamp)
        {
            this.animator.SetBool("ShowControls", false);
        }

        public void HandleToggleUI()
        {
            this.animator.SetBool("ShowControls", !this.animator.GetBool("ShowControls"));
        }

        public void HandleVolumeChange()
        {
            this.system.SetVolume(this.volume.normalizedValue);
        }

        public void HandleToggleMute()
        {
            this.system.SetVolume(this.system.GetVolume() == 0 ? 0.1f : 0);
        }

        public void HandleBrightnessChange()
        {
            this.system.SetBrightness(this.brightness.normalizedValue);
        }

        protected override void OnVolumeChange(float volume)
        {
            if (this.volume.normalizedValue == volume) return;

            this.volume.SetValueWithoutNotify(volume);
        }

        protected override void OnBrightnessChange(float alpha)
        {
            if (this.brightness.normalizedValue == alpha) return;

            this.brightness.SetValueWithoutNotify(alpha);
        }
    }
}
