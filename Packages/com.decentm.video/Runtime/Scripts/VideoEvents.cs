using DecentM.Pubsub;
using UdonSharp;
using VRC.SDKBase;
using VRC.SDK3.Components.Video;

namespace DecentM.Video
{
    
    internal sealed class VideoEvent
    {
        internal void OnVideoPlayerInit() { }
        internal void OnBrightnessChange() { }
        internal void OnVolumeChange() { }
        internal void OnFpsChange() { }
        internal void OnScreenResolutionChange() { }
        internal void OnScreenTextureChange() { }
        internal void OnPlayerChange() { }

        internal void OnPlay() { }
        internal void OnPause() { }
        internal void OnStop() { }
        internal void OnProgress() { }

        internal void OnLoadBegin() { }
        internal void OnLoadReady() { }
        internal void OnLoadError() { }
        internal void OnLoadRequested() { }
        internal void OnCustomVideoEvent() { }
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public sealed class VideoEvents : PubsubHost
    {
        #region Core

        public void OnVideoPlayerInit()
        {
            this.BroadcastEvent(nameof(VideoEvent.OnVideoPlayerInit));
        }

        public void OnBrightnessChange(float alpha)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnBrightnessChange), alpha);
        }

        public void OnVolumeChange(float volume)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnVolumeChange), volume);
        }

        public void OnFpsChange(int fps)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnFpsChange), fps);
        }

        public void OnScreenResolutionChange(float width, float height)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnScreenResolutionChange), width, height);
        }

        public void OnPlay(float timestamp)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnPlay), timestamp);
        }

        public void OnPause(float timestamp)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnPause), timestamp);
        }

        public void OnProgress(float timestamp, float duration)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnProgress), timestamp, duration);
        }

        public void OnStop()
        {
            this.BroadcastEvent(nameof(VideoEvent.OnStop));
        }

        public void OnLoadBegin(VRCUrl url)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnLoadBegin), url);
        }

        public void OnLoadRequested(VRCUrl url)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnLoadRequested), url);
        }

        public void OnLoadReady(float duration)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnLoadReady), duration);
        }

        public void OnLoadError(VideoError error)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnLoadError), error);
        }

        public void OnPlayerChange(string type)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnPlayerChange), type);
        }

        public void OnScreenTextureChange()
        {
            this.BroadcastEvent(nameof(VideoEvent.OnScreenTextureChange));
        }

        public void OnCustomVideoEvent(string name, object[] data)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnCustomVideoEvent), name, data);
        }

        public void OnCustomVideoEvent(string name)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnCustomVideoEvent), name);
        }

        #endregion
    }
}
