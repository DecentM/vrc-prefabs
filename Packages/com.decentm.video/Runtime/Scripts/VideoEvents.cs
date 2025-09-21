using UnityEngine;

using DecentM.Pubsub;
using UdonSharp;
using VRC.SDKBase;
using VRC.SDK3.Components.Video;

namespace DecentM.Video
{
    
    internal sealed class VideoEvent
    {
        internal void OnDebugLog() { }

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
        internal void OnLoadRatelimitWaiting() { }

        internal void OnAutoRetry() { }
        internal void OnAutoRetryLoadTimeout() { }
        internal void OnAutoRetryAbort() { }
        internal void OnAutoRetryAllPlayersFailed() { }

        internal void OnOwnershipChanged() { }
        internal void OnOwnershipSecurityChanged() { }
        internal void OnOwnershipRequested() { }

        internal void OnRemotePlayerLoaded() { }

        internal void OnMetadataChange() { }
        internal void OnSubtitleLanguageOptionsChange() { }
        internal void OnSubtitleLanguageRequested() { }
        internal void OnSubtitleRender() { }
        internal void OnSubtitleClear() { }
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public sealed class VideoEvents : PubsubHost
    {
        #region Core

        public void OnDebugLog(string message)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnDebugLog), message);
        }

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

        public void OnLoadRatelimitWaiting()
        {
            this.BroadcastEvent(nameof(VideoEvent.OnLoadRatelimitWaiting));
        }

        public void OnPlayerChange(string type)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnPlayerChange), type);
        }

        #endregion

        #region Plugins

        public void OnAutoRetry(int attempt)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnAutoRetry), attempt);
        }

        public void OnAutoRetryLoadTimeout(int timeout)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnAutoRetryLoadTimeout), timeout);
        }

        public void OnAutoRetryAbort()
        {
            this.BroadcastEvent(nameof(VideoEvent.OnAutoRetryAbort));
        }

        public void OnAutoRetryAllPlayersFailed()
        {
            this.BroadcastEvent(nameof(VideoEvent.OnAutoRetryAllPlayersFailed));
        }

        public void OnOwnershipChanged(int previousOwnerId, VRCPlayerApi nextOwner)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnOwnershipChanged), previousOwnerId, nextOwner);
        }

        public void OnOwnershipSecurityChanged(bool locked)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnOwnershipSecurityChanged), locked);
        }

        public void OnOwnershipRequested()
        {
            this.BroadcastEvent(nameof(VideoEvent.OnOwnershipRequested));
        }

        public void OnScreenTextureChange()
        {
            this.BroadcastEvent(nameof(VideoEvent.OnScreenTextureChange));
        }

        public void OnRemotePlayerLoaded(int loadedPlayers)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnRemotePlayerLoaded), loadedPlayers);
        }

        public void OnMetadataChange(
            string title,
            string uploader,
            string siteName,
            int viewCount,
            int likeCount,
            string resolution,
            int fps,
            string description,
            string duration,
            TextAsset[] subtitles
        )
        {
            this.BroadcastEvent(
                nameof(VideoEvent.OnMetadataChange),
                title,
                uploader,
                siteName,
                viewCount,
                likeCount,
                resolution,
                fps,
                description,
                duration,
                subtitles
            );
        }

        public void OnSubtitleRender(string text)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnSubtitleRender), text);
        }

        public void OnSubtitleClear()
        {
            this.BroadcastEvent(nameof(VideoEvent.OnSubtitleClear));
        }

        public void OnSubtitleLanguageOptionsChange(string[][] newOptions)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnSubtitleLanguageOptionsChange), newOptions);
        }

        public void OnSubtitleLanguageRequested(string language)
        {
            this.BroadcastEvent(nameof(VideoEvent.OnSubtitleLanguageRequested), language);
        }

        #endregion
    }
}
