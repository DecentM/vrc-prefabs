using System;
using UdonSharp;
using UnityEngine;

namespace DecentM.Video
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VideoScreen : UdonSharpBehaviour
    {
        [NonSerialized]
        private Renderer screen;

        private void Start()
        {
            this.screen = this.GetComponent<Renderer>();
        }

        public float GetBrightness()
        {
            if (this.screen == null)
                return 1f;

            return this.screen.material.GetFloat("_EmissionStrength");
        }

        public void SetAspectRatio(float aspectRatio)
        {
            this.screen.material.SetFloat("_TargetAspectRatio", aspectRatio);
        }

        public void SetBrightness(float alpha)
        {
            this.screen.material.SetFloat("_EmissionStrength", alpha);
        }

        public void SetIsAVPro(bool isAVPro)
        {
            this.screen.material.SetInt("_IsAVPro", isAVPro ? 1 : 0);
        }

        public void SetTexture(Texture texture)
        {
            this.screen.material.SetTexture("_MainTex", texture);
        }

        public void SetSize(Vector2 size)
        {
            this.screen.transform.localScale = new Vector3(
                size.x,
                size.y,
                this.screen.transform.localScale.z
            );
        }
    }
}
