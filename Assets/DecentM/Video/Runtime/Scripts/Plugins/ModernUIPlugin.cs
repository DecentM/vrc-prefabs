using UnityEngine;

using UnityEngine.UI;
using VRC.SDK3.Components;
using UdonSharp;

namespace DecentM.Video.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public sealed class ModernUIPlugin : VideoPlugin
    {
        [Space]
        public Animator animator;

        [Space]
        public VRCUrlInputField urlInput;
        public Button enterButton;

        [Space, SerializeField]
        private Slider progress;

        protected override void OnProgress(float timestamp, float duration)
        {

            if (!float.IsInfinity(timestamp) && !float.IsInfinity(duration))
                this.progress.SetValueWithoutNotify(Mathf.Max(timestamp / duration, 0.001f));
        }

        internal void HandlePlay()
        {
            this.system.Play();
        }

        internal void HandleSeek()
        {
            this.system.Seek(this.progress.normalizedValue * this.system.GetDuration());
        }

        internal void HandlePause()
        {
            this.system.Pause();
        }

        internal void HandleStop()
        {
            this.system.Stop();
        }
    }
}
