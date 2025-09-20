using UnityEngine;
using UdonSharp;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;
using VRC.SDK3.Components.Video;
using System;

namespace DecentM.Video
{
    public class VideoHandlerType
    {
        public void Unity() { }
        public void AVPro() { }
    }

    internal abstract class PlayerHandler : UdonSharpBehaviour
    {
        public abstract string type { get; }

        [NonSerialized] private BaseVRCVideoPlayer player;
        [NonSerialized] private VideoEvents events;
        public Renderer screen;
        [NonSerialized] private VideoSystem system;

        private MaterialPropertyBlock _fetchBlock;

        void Start()
        {

            this.player = this.GetComponent<BaseVRCVideoPlayer>();
            this.system = this.GetComponentInParent<VideoSystem>();
            this.events = this.GetComponentInParent<VideoEvents>();
            this._fetchBlock = new MaterialPropertyBlock();

            this.system.RegisterPlayerHandler(this);
        }

        public float progressReportIntervalSeconds = 1;

        private float clock = 0;

        private void FixedUpdate()
        {
            if (
                this.player == null
                || !this.player.IsPlaying
                || float.IsInfinity(this.player.GetDuration())
            )
                return;

            this.clock += Time.fixedDeltaTime;

            if (this.clock > this.progressReportIntervalSeconds)
            {
                this.HandleProgress();
                this.clock = 0;
            }
        }

        internal Texture GetScreenTexture()
        {
            Texture result = this.screen.material.GetTexture("_MainTex");

            if (result == null)
            {
                this.screen.GetPropertyBlock(_fetchBlock);
                result = _fetchBlock.GetTexture("_MainTex");
            }

            return result;
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