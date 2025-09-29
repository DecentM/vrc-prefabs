using System;
using UdonSharp;
using UnityEngine;

namespace DecentM.Video
{
    [
        UdonBehaviourSyncMode(BehaviourSyncMode.None),
        AddComponentMenu("DecentM/Video/VideoScreen"),
        RequireComponent(typeof(Renderer))
    ]
    internal sealed class VideoScreen : UdonSharpBehaviour
    {
        [NonSerialized]
        private Renderer screen;

        private void Start()
        {
            this.screen = this.GetComponent<Renderer>();
        }

        internal bool SetAspectRatio(float aspectRatio)
        {
            if (this.screen == null || this.screen.material == null)
                return false;

            this.screen.material.SetFloat("_TargetAspectRatio", aspectRatio);
            return true;
        }

        internal float GetAspectRatio()
        {
            if (this.screen == null || this.screen.material == null)
                return -1f;

            return this.screen.material.GetFloat("_TargetAspectRatio");
        }

        internal float GetBrightness()
        {
            if (this.screen == null || this.screen.material == null)
                return -1f;

            return this.screen.material.GetFloat("_EmissionStrength");
        }

        internal bool SetBrightness(float alpha)
        {
            if (this.screen == null || this.screen.material == null)
                return false;

            this.screen.material.SetFloat("_EmissionStrength", alpha);
            return true;
        }

        internal bool SetIsAVPro(bool isAVPro)
        {
            if (this.screen == null || this.screen.material == null)
                return false;

            this.screen.material.SetInt("_IsAVPro", isAVPro ? 1 : 0);
            return true;
        }

        internal bool GetIsAVPro()
        {
            if (this.screen == null || this.screen.material == null)
                return false;

            return this.screen.material.GetInt("_IsAVPro") == 1;
        }

        internal bool SetTexture(Texture texture)
        {
            if (this.screen == null || this.screen.material == null)
                return false;

            this.screen.material.SetTexture("_MainTex", texture);
            return true;
        }

        internal Texture GetTexture()
        {
            if (this.screen == null || this.screen.material == null)
                return null;

            Texture tex = this.screen.material.GetTexture("_MainTex");

            if (tex == null)
            {
                MaterialPropertyBlock fetchBlock = new MaterialPropertyBlock();
                this.screen.GetPropertyBlock(fetchBlock);
                tex = fetchBlock.GetTexture("_MainTex");
            }

            return tex;
        }

        internal bool SetSize(Vector2 size)
        {
            if (this.screen == null || this.screen.material == null)
                return false;

            this.screen.transform.localScale = new Vector3(
                size.x,
                size.y,
                this.screen.transform.localScale.z
            );
            return true;
        }

        internal Vector2 GetSize()
        {
            if (this.screen == null || this.screen.material == null)
                return Vector2.zero;

            return new Vector2(this.screen.transform.localScale.x, this.screen.transform.localScale.y);
        }
    }
}
