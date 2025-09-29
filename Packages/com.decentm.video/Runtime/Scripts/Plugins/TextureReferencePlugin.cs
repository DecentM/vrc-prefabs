using UnityEngine;
using UdonSharp;

namespace DecentM.Video.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), AddComponentMenu("DecentM/Video/Plugins/TextureReference")]
    internal sealed class TextureReferencePlugin : VideoPlugin
    {
        [SerializeField] private Material[] materials;
        [SerializeField] private Renderer[] renderers;

        public string textureProperty = "_EmissionMap";
        public string avProProperty = "_IsAVProInput";

        protected override void OnScreenTextureChange()
        {
            if (this.materials == null || this.materials.Length == 0)
                return;

            if (this.renderers == null || this.renderers.Length == 0)
                return;

            Texture videoPlayerTex = this.system.GetScreenTexture();

            foreach (Material material in this.materials)
            {
                material.SetTexture(this.textureProperty, videoPlayerTex);
            }

            foreach (Renderer renderer in this.renderers)
            {
                renderer.material.SetTexture(this.textureProperty, videoPlayerTex);
            }
        }

        protected override void OnPlayerSwitch(string type)
        {
            foreach (Material material in this.materials)
            {
                material.SetInt(this.avProProperty, type == nameof(VideoHandlerType.AVPro) ? 1 : 0);
            }

            foreach (Renderer renderer in this.renderers)
            {
                renderer.material.SetInt(this.avProProperty, type == nameof(VideoHandlerType.AVPro) ? 1 : 0);
            }
        }
    }
}
