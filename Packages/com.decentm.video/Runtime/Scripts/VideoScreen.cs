using System;
using UdonSharp;
using UnityEngine;

namespace DecentM.Video
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    internal sealed class VideoScreen : UdonSharpBehaviour
    {
        [NonSerialized]
        private Renderer screen;

        private void Start()
        {
            this.screen = this.GetComponent<Renderer>();
        }

        internal float GetBrightness()
        {
            if (this.screen == null)
                return -1f;

            return this.screen.material.GetFloat("_EmissionStrength");
        }

        internal void SetAspectRatio(float aspectRatio)
        {
            this.screen.material.SetFloat("_TargetAspectRatio", aspectRatio);
        }

        internal void SetBrightness(float alpha)
        {
            this.screen.material.SetFloat("_EmissionStrength", alpha);
        }

        internal void SetIsAVPro(bool isAVPro)
        {
            this.screen.material.SetInt("_IsAVPro", isAVPro ? 1 : 0);
        }

        internal void SetTexture(Texture texture)
        {
            this.screen.material.SetTexture("_MainTex", texture);
        }

        internal void SetSize(Vector2 size)
        {
            this.screen.transform.localScale = new Vector3(
                size.x,
                size.y,
                this.screen.transform.localScale.z
            );
        }
    }
}
