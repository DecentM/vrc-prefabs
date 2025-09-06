using UnityEngine;

using DecentM.Pubsub;
using VRC.SDKBase;
using UdonSharp;
using VRC.SDK3.Components.Video;

namespace DecentM.Video.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class VideoPlugin : PubsubSubscriber
    {
        public VideoSystem system;
        public VideoEvents events;

        protected virtual void OnDebugLog(string message) { }

        protected virtual void OnVideoPlayerInit() { }

        protected virtual void OnBrightnessChange(float alpha) { }

        protected virtual void OnVolumeChange(float volume, bool muted) { }

        protected virtual void OnMutedChange(bool muted, float volume) { }

        protected virtual void OnFpsChange(int fps) { }

        protected virtual void OnScreenResolutionChange(
            ScreenHandler screen,
            float width,
            float height
        ) { }

        protected virtual void OnScreenTextureChange() { }

        protected virtual void OnPlayerSwitch(VideoHandlerType type) { }

        protected virtual void OnPlaybackStart(float timestamp) { }

        protected virtual void OnPlaybackStop(float timestamp) { }

        protected virtual void OnPlaybackEnd() { }

        protected virtual void OnProgress(float timestamp, float duration) { }

        protected virtual void OnLoadBegin(VRCUrl url) { }

        protected virtual void OnLoadBegin() { }

        protected virtual void OnLoadReady(float duration) { }

        protected virtual void OnLoadError(VideoError error) { }

        protected virtual void OnUnload() { }

        protected virtual void OnLoadRequested(VRCUrl url) { }

        protected virtual void OnLoadApproved(VRCUrl url) { }

        protected virtual void OnLoadDenied(VRCUrl url, string reason) { }

        protected virtual void OnLoadRatelimitWaiting() { }

        protected virtual void OnAutoRetry(int attempt) { }

        protected virtual void OnAutoRetryLoadTimeout(int timeout) { }

        protected virtual void OnAutoRetryAbort() { }

        protected virtual void OnAutoRetryAllPlayersFailed() { }

        protected virtual void OnRemotePlayerLoaded(int loadedPlayers) { }

        protected virtual void OnSubtitleRender(string text) { }

        protected virtual void OnSubtitleClear() { }

        protected virtual void OnSubtitleLanguageOptionsChange(string[][] newOptions) { }

        protected virtual void OnSubtitleLanguageRequested(string language) { }

        protected virtual void OnMetadataChange(
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
        ) { }

        public sealed override void OnPubsubEvent(object name, object[] data)
        {
            switch (name)
            {
                #region Core

                case VideoEvent.OnDebugLog:
                {
                    string message = (string)data[0];
                    this.OnDebugLog(message);
                    return;
                }

                case VideoEvent.OnLoadRequested:
                {
                    VRCUrl url = (VRCUrl)data[0];
                    this.OnLoadRequested(url);
                    return;
                }

                case VideoEvent.OnLoadApproved:
                {
                    VRCUrl url = (VRCUrl)data[0];
                    this.OnLoadApproved(url);
                    return;
                }

                case VideoEvent.OnLoadDenied:
                {
                    VRCUrl url = (VRCUrl)data[0];
                    string reason = (string)data[1];
                    this.OnLoadDenied(url, reason);
                    return;
                }

                case VideoEvent.OnLoadBegin:
                {
                    VRCUrl url = (VRCUrl)data[0];
                    if (url == null)
                        this.OnLoadBegin();
                    else
                        this.OnLoadBegin(url);
                    return;
                }

                case VideoEvent.OnBrightnessChange:
                {
                    float alpha = (float)data[0];
                    this.OnBrightnessChange(alpha);
                    return;
                }

                case VideoEvent.OnVolumeChange:
                {
                    float volume = (float)data[0];
                    bool muted = (bool)data[1];
                    this.OnVolumeChange(volume, muted);
                    return;
                }

                case VideoEvent.OnMutedChange:
                {
                    bool muted = (bool)data[0];
                    float volume = (float)data[1];
                    this.OnMutedChange(muted, volume);
                    return;
                }

                case VideoEvent.OnFpsChange:
                {
                    int fps = (int)data[0];
                    this.OnFpsChange(fps);
                    return;
                }

                case VideoEvent.OnScreenResolutionChange:
                {
                    ScreenHandler screen = (ScreenHandler)data[0];
                    float width = (float)data[1];
                    float height = (float)data[2];
                    this.OnScreenResolutionChange(screen, width, height);
                    return;
                }

                case VideoEvent.OnLoadReady:
                {
                    float duration = (float)data[0];
                    this.OnLoadReady(duration);
                    return;
                }

                case VideoEvent.OnLoadError:
                {
                    VideoError error = (VideoError)data[0];
                    this.OnLoadError(error);
                    return;
                }

                case VideoEvent.OnLoadRatelimitWaiting:
                {
                    this.OnLoadRatelimitWaiting();
                    return;
                }

                case VideoEvent.OnUnload:
                {
                    this.OnUnload();
                    return;
                }

                case VideoEvent.OnPlaybackStart:
                {
                    float timestamp = (float)data[0];
                    this.OnPlaybackStart(timestamp);
                    return;
                }

                case VideoEvent.OnPlaybackStop:
                {
                    float timestamp = (float)data[0];
                    this.OnPlaybackStop(timestamp);
                    return;
                }

                case VideoEvent.OnProgress:
                {
                    float timestamp = (float)data[0];
                    float duration = (float)data[1];
                    this.OnProgress(timestamp, duration);
                    return;
                }

                case VideoEvent.OnPlaybackEnd:
                {
                    this.OnPlaybackEnd();
                    return;
                }

                case VideoEvent.OnVideoPlayerInit:
                {
                    this.OnVideoPlayerInit();
                    return;
                }

                case VideoEvent.OnPlayerSwitch:
                {
                    VideoHandlerType type = (VideoHandlerType)data[0];
                    this.OnPlayerSwitch(type);
                    return;
                }

                #endregion

                #region Plugins

                case VideoEvent.OnAutoRetry:
                {
                    int attempt = (int)data[0];
                    this.OnAutoRetry(attempt);
                    return;
                }

                case VideoEvent.OnAutoRetryLoadTimeout:
                {
                    int timeout = (int)data[0];
                    this.OnAutoRetryLoadTimeout(timeout);
                    return;
                }

                case VideoEvent.OnAutoRetryAbort:
                {
                    this.OnAutoRetryAbort();
                    return;
                }

                case VideoEvent.OnAutoRetryAllPlayersFailed:
                {
                    this.OnAutoRetryAllPlayersFailed();
                    return;
                }

                case VideoEvent.OnScreenTextureChange:
                {
                    this.OnScreenTextureChange();
                    return;
                }

                case VideoEvent.OnRemotePlayerLoaded:
                {
                    int loadedPlayers = (int)data[0];
                    this.OnRemotePlayerLoaded(loadedPlayers);
                    return;
                }

                case VideoEvent.OnMetadataChange:
                {
                    string title = (string)data[0];
                    string uploader = (string)data[1];
                    string siteName = (string)data[2];
                    int viewCount = (int)data[3];
                    int likeCount = (int)data[4];
                    string resolution = (string)data[5];
                    int fps = (int)data[6];
                    string description = (string)data[7];
                    string duration = (string)data[8];
                    TextAsset[] subtitles = (TextAsset[])data[9];

                    this.OnMetadataChange(
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
                    return;
                }

                case VideoEvent.OnSubtitleRender:
                {
                    string text = (string)data[0];
                    this.OnSubtitleRender(text);
                    return;
                }

                case VideoEvent.OnSubtitleClear:
                {
                    this.OnSubtitleClear();
                    return;
                }

                case VideoEvent.OnSubtitleLanguageOptionsChange:
                {
                    string[][] newOptions = (string[][])data;
                    this.OnSubtitleLanguageOptionsChange(newOptions);
                    return;
                }

                case VideoEvent.OnSubtitleLanguageRequested:
                {
                    string language = (string)data[0];
                    this.OnSubtitleLanguageRequested(language);
                    return;
                }

                default:
                {
                    break;
                }

                #endregion
            }
        }
    }
}
