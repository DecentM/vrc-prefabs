using UnityEngine;
using UdonSharp;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;
using VRC.SDK3.Components.Video;
using System;

namespace DecentM.Video
{
    internal class VideoHandlerType
    {
        public void Unity() { }
        public void AVPro() { }
    }

    [RequireComponent(typeof(Renderer))]
    internal abstract class PlayerHandler : UdonSharpBehaviour
    {
        public abstract string type { get; }

        [NonSerialized] private BaseVRCVideoPlayer player;
        [NonSerialized] private VideoEvents events;
        [NonSerialized] private VideoSystem system;

        [NonSerialized] private Renderer screen;

        void Start()
        {
            this.player = this.GetComponent<BaseVRCVideoPlayer>();
            this.system = this.GetComponentInParent<VideoSystem>();
            this.events = this.GetComponentInParent<VideoEvents>();
            
            this.screen = this.GetComponent<Renderer>();
            this.system.RegisterPlayerHandler(this);
        }

        private const float progressReportIntervalSeconds = 1;

        private float clock = 0;

        private void FixedUpdate()
        {
            if (
                this.player == null
                || !this.player.IsPlaying
                || float.IsInfinity(this.player.GetDuration())
            )
                return;

            this.clock += Time.fixedUnscaledDeltaTime;

            if (this.clock > PlayerHandler.progressReportIntervalSeconds)
            {
                this.HandleProgress();
                this.clock = 0;
            }
        }

        internal Texture GetScreenTexture()
        {
            Texture tex = this.screen.material.GetTexture("_MainTex");

            if (tex == null)
            {
                MaterialPropertyBlock fetchBlock = new MaterialPropertyBlock();
                this.screen.GetPropertyBlock(fetchBlock);
                tex = fetchBlock.GetTexture("_MainTex");
            }

            return tex;
        }

        private void HandleProgress()
        {
            this.events.OnProgress(this.player.GetTime(), this.player.GetDuration());
        }

        public override void OnVideoEnd()
        {
            this.system.Stop();
        }

        public override void OnVideoPause()
        {
            this.events.OnPause(this.player.GetTime());
        }

        public override void OnVideoPlay()
        {
            this.events.OnPlay(this.player.GetTime());
        }

        public override void OnVideoReady()
        {
            this.events.OnLoadReady(this.player.GetDuration());
        }

        public override void OnVideoStart()
        {
            this.events.OnPlay(this.player.GetTime());
        }

        public override void OnVideoError(VideoError videoError)
        {
            this.events.OnLoadError(videoError);
        }

        public void Play(float timestamp)
        {
            this.player.SetTime(timestamp);
            this.player.Play();
        }

        public void Play()
        {
            this.player.Play();
        }

        public void SetTime(float timestamp)
        {
            this.player.SetTime(timestamp);
        }

        public void Pause(float timestamp)
        {
            this.player.Pause();
            this.player.SetTime(timestamp);
        }

        public void Pause()
        {
            this.player.Pause();
        }

        public void LoadURL(VRCUrl url)
        {
            this.player.LoadURL(url);
            this.player.Pause();
            this.player.SetTime(0);
        }

        public void Unload()
        {
            this.player.Stop();
        }

        public bool IsPlaying()
        {
            if (this.player == null)
                return false;

            return this.player.IsPlaying;
        }

        public float GetDuration()
        {
            return this.player.GetDuration();
        }

        public float GetTime()
        {
            return this.player.GetTime();
        }
    }
}