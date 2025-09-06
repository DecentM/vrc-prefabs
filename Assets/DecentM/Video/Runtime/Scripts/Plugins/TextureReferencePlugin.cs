using UnityEngine;

using DecentM.Video.Handlers;

namespace DecentM.Video.Plugins
{
    public class TextureReferencePlugin : VideoPlugin
    {
        public Material[] materials;
        public Renderer[] renderers;

        public string textureProperty = "_EmissionMap";
        public string avProProperty = "_IsAVProInput";

        protected override void OnScreenTextureChange()
        {
            if (this.materials == null || this.materials.Length == 0)
                return;

            if (this.renderers == null || this.renderers.Length == 0)
                return;

            Texture videoPlayerTex = this.system.GetVideoTexture();

            foreach (Material material in this.materials)
            {
                material.SetTexture(this.textureProperty, videoPlayerTex);
            }

            foreach (Renderer renderer in this.renderers)
            {
                renderer.material.SetTexture(this.textureProperty, videoPlayerTex);
            }
        }

        protected override void OnPlayerSwitch(VideoHandlerType type)
        {
            foreach (Material material in this.materials)
            {
                material.SetInt(this.avProProperty, type == VideoHandlerType.AVPro ? 1 : 0);
            }

            foreach (Renderer renderer in this.renderers)
            {
                renderer.material.SetInt(this.avProProperty, type == VideoHandlerType.AVPro ? 1 : 0);
            }
        }
    }
}
