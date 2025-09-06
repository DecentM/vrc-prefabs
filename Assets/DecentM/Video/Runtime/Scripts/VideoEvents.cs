using UnityEngine;

using DecentM.Pubsub;
using UdonSharp;
using VRC.SDKBase;
using VRC.SDK3.Components.Video;

namespace DecentM.Video
{
    public enum VideoEvent
    {
        OnDebugLog,

        OnVideoPlayerInit,
        OnBrightnessChange,
        OnVolumeChange,
        OnMutedChange,
        OnFpsChange,
        OnScreenResolutionChange,
        OnScreenTextureChange,
        OnPlayerSwitch,

        OnPlaybackStart,
        OnPlaybackStop,
        OnPlaybackEnd,
        OnProgress,

        OnLoadBegin,
        OnLoadReady,
        OnLoadError,
        OnUnload,
        OnLoadRequested,
        OnLoadApproved,
        OnLoadDenied,
        OnLoadRatelimitWaiting,

        OnAutoRetry,
        OnAutoRetryLoadTimeout,
        OnAutoRetryAbort,
        OnAutoRetryAllPlayersFailed,

        OnOwnershipChanged,
        OnOwnershipSecurityChanged,
        OnOwnershipRequested,

        OnRemotePlayerLoaded,

        OnMetadataChange,
        OnSubtitleLanguageOptionsChange,
        OnSubtitleLanguageRequested,
        OnSubtitleRender,
        OnSubtitleClear,
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public sealed class VideoEvents : PubsubHost
    {
        #region Core

        public void OnDebugLog(string message)
        {
            this.BroadcastEvent(VideoEvent.OnDebugLog, message);
        }

        public void OnVideoPlayerInit()
        {
            this.BroadcastEvent(VideoEvent.OnVideoPlayerInit);
        }

        public void OnBrightnessChange(float alpha)
        {
            this.BroadcastEvent(VideoEvent.OnBrightnessChange, alpha);
        }

        public void OnVolumeChange(float volume, bool muted)
        {
            this.BroadcastEvent(VideoEvent.OnVolumeChange, volume, muted);
        }

        public void OnMutedChange(bool muted, float volume)
        {
            this.BroadcastEvent(VideoEvent.OnMutedChange, muted, volume);
        }

        public void OnFpsChange(int fps)
        {
            this.BroadcastEvent(VideoEvent.OnFpsChange, fps);
        }

        public void OnScreenResolutionChange(ScreenHandler screen, float width, float height)
        {
            this.BroadcastEvent(VideoEvent.OnScreenResolutionChange, screen, width, height);
        }

        public void OnPlaybackStart(float timestamp)
        {
            this.BroadcastEvent(VideoEvent.OnPlaybackStart, timestamp);
        }

        public void OnPlaybackStop(float timestamp)
        {
            this.BroadcastEvent(VideoEvent.OnPlaybackStop, timestamp);
        }

        public void OnProgress(float timestamp, float duration)
        {
            this.BroadcastEvent(VideoEvent.OnProgress, timestamp, duration);
        }

        public void OnPlaybackEnd()
        {
            this.BroadcastEvent(VideoEvent.OnPlaybackEnd);
        }

        public void OnLoadBegin(VRCUrl url)
        {
            this.BroadcastEvent(VideoEvent.OnLoadBegin, url);
        }

        public void OnLoadRequested(VRCUrl url)
        {
            this.BroadcastEvent(VideoEvent.OnLoadRequested, url);
        }

        public void OnLoadApproved(VRCUrl url)
        {
            this.BroadcastEvent(VideoEvent.OnLoadApproved, url);
        }

        public void OnLoadDenied(VRCUrl url, string reason)
        {
            this.BroadcastEvent(VideoEvent.OnLoadDenied, url, reason);
        }

        public void OnLoadReady(float duration)
        {
            this.BroadcastEvent(VideoEvent.OnLoadReady, duration);
        }

        public void OnUnload()
        {
            this.BroadcastEvent(VideoEvent.OnUnload);
        }

        public void OnLoadError(VideoError error)
        {
            this.BroadcastEvent(VideoEvent.OnLoadError, error);
        }

        public void OnLoadRatelimitWaiting()
        {
            this.BroadcastEvent(VideoEvent.OnLoadRatelimitWaiting);
        }

        public void OnPlayerSwitch(VideoHandlerType type)
        {
            this.BroadcastEvent(VideoEvent.OnPlayerSwitch, type);
        }

        #endregion

        #region Plugins

        public void OnAutoRetry(int attempt)
        {
            this.BroadcastEvent(VideoEvent.OnAutoRetry, attempt);
        }

        public void OnAutoRetryLoadTimeout(int timeout)
        {
            this.BroadcastEvent(VideoEvent.OnAutoRetryLoadTimeout, timeout);
        }

        public void OnAutoRetryAbort()
        {
            this.BroadcastEvent(VideoEvent.OnAutoRetryAbort);
        }

        public void OnAutoRetryAllPlayersFailed()
        {
            this.BroadcastEvent(VideoEvent.OnAutoRetryAllPlayersFailed);
        }

        public void OnScreenTextureChange()
        {
            this.BroadcastEvent(VideoEvent.OnScreenTextureChange);
        }

        public void OnRemotePlayerLoaded(int loadedPlayers)
        {
            this.BroadcastEvent(VideoEvent.OnRemotePlayerLoaded, loadedPlayers);
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
                VideoEvent.OnMetadataChange,
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
            this.BroadcastEvent(VideoEvent.OnSubtitleRender, text);
        }

        public void OnSubtitleClear()
        {
            this.BroadcastEvent(VideoEvent.OnSubtitleClear);
        }

        public void OnSubtitleLanguageOptionsChange(string[][] newOptions)
        {
            this.BroadcastEvent(VideoEvent.OnSubtitleLanguageOptionsChange, newOptions);
        }

        public void OnSubtitleLanguageRequested(string language)
        {
            this.BroadcastEvent(VideoEvent.OnSubtitleLanguageRequested, language);
        }

        #endregion
    }
}
