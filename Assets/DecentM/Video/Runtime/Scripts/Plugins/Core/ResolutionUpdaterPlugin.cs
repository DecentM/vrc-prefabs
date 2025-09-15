using UnityEngine;

namespace DecentM.Video.Plugins
{
    public sealed class ResolutionUpdaterPlugin : VideoPlugin
    {
        public Vector2Int defaultResolution = new Vector2Int(1920, 1080);

        private void ChangeScreenResolution()
        {
            Texture videoTexture = this.system.GetVideoTexture();

            if (videoTexture == null)
                return;

            float w = videoTexture.width;
            float h = videoTexture.height;

            this.system.ChangeScreenResolution(w, h);
        }

        protected override void OnPlay(float duration)
        {
            this.ChangeScreenResolution();
        }

        protected override void OnScreenTextureChange()
        {
            this.ChangeScreenResolution();
        }
    }
}