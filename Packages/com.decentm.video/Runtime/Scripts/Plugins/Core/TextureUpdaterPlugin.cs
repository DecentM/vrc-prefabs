using UnityEngine;

using UdonSharp;

namespace DecentM.Video.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), AddComponentMenu("DecentM/Video/Plugins/TextureUpdater")]
    internal sealed class TextureUpdaterPlugin : VideoPlugin
    {
        [SerializeField] private Texture idleTexture;

        protected override void OnPlay(float duration)
        {
            Texture videoTexture = this.system.GetScreenTexture();

            if (videoTexture == null)
                return;

            this.system.SetScreenTexture(videoTexture);
        }

        private void ShowIdleTexture()
        {
            this.system.SetScreenTexture(this.idleTexture);
        }

        protected override void OnVideoPlayerInit()
        {
            this.ShowIdleTexture();
        }

        protected override void OnStop()
        {
            this.ShowIdleTexture();
        }
    }
}
