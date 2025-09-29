using UdonSharp;
using UnityEngine;

namespace DecentM.Video.Plugins
{
    /// <summary>
    /// When a video loads, this plugin updates the screen resolution to match the video's size
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), AddComponentMenu("DecentM/Video/Plugins/ResolutionUpdater")]
    internal sealed class ResolutionUpdaterPlugin : VideoPlugin
    {
        private void ChangeScreenResolution()
        {
            Texture videoTexture = this.system.GetScreenTexture();

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
