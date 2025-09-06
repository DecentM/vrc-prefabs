using UnityEngine;

using UdonSharp;

namespace DecentM.Video.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public sealed class TextureUpdaterPlugin : VideoPlugin
    {
        public Texture idleTexture;

        protected override void OnPlaybackStart(float duration)
        {
            Texture videoTexture = this.system.GetVideoTexture();

            if (videoTexture == null)
                return;

            this.SetTexture(videoTexture);
        }

        private void ShowIdleTexture()
        {
            this.SetTexture(idleTexture);
        }

        private void SetAVPro(bool isAVPro)
        {
            foreach (ScreenHandler screen in this.system.screens)
            {
                screen.SetIsAVPro(isAVPro);
            }
        }

        protected override void OnVideoPlayerInit()
        {
            this.ShowIdleTexture();
        }

        protected override void OnUnload()
        {
            this.ShowIdleTexture();
        }

        protected override void OnPlaybackEnd()
        {
            this.ShowIdleTexture();
        }

        protected override void OnPlayerSwitch(VideoHandlerType type)
        {
            this.SetAVPro(type == VideoHandlerType.AVPro);
        }

        public void SetTexture(Texture texture)
        {
            if (texture == null)
                return;

            foreach (ScreenHandler screen in this.system.screens)
            {
                screen.SetTexture(texture);
            }

            this.events.OnScreenTextureChange();
        }
    }
}
