using UnityEngine;
using UdonSharp;

namespace DecentM.Video.Handlers
{
    public enum VideoHandlerType
    {
        AVPro,
        Unity,
    }

    public abstract class PlayerHandler : UdonSharpBehaviour
    {
        public abstract VideoHandlerType type { get; }
        public VideoEvents events;

        protected PlayerHandler playerHandler;

        public void RegisterPlayerHandler(PlayerHandler playerHandler)
        {
            this.playerHandler = playerHandler;
        }

        public abstract bool IsPlaying { get; }

        public abstract float GetDuration();

        public abstract float GetTime();

        public abstract void SetTime(float time);

        public abstract void LoadURL(string url);

        public abstract void Play();

        public abstract void Play(float time);

        public abstract void Unload();

        public abstract void Pause();

        public abstract void Pause(float time);

        public abstract Texture GetScreenTexture();

        protected virtual void OnVideoUnload()
        {
            this.events.OnUnload();
        }

        protected virtual void OnProgress()
        {
            this.events.OnProgress(this.GetTime(), this.GetDuration());
        }

        protected virtual new void OnVideoEnd()
        {
            this.events.OnPlaybackEnd();
        }

        protected virtual new void OnVideoPause()
        {
            this.events.OnPlaybackStop(this.GetTime());
        }

        protected virtual new void OnVideoPlay()
        {
            this.events.OnPlaybackStart(this.GetTime());
        }

        protected virtual new void OnVideoReady()
        {
            this.events.OnLoadReady(this.GetDuration());
        }

        protected virtual void OnVideoError()
        {
            this.events.OnLoadError(new VideoError(VideoErrorType.Unknown));
        }
    }
}

