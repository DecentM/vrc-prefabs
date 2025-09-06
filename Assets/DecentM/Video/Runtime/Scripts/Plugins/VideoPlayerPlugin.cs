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
        [SerializeField]
        protected VideoSystem system;
        [SerializeField]
        protected VideoEvents events;

        protected virtual void __Awake() { }

        protected override void _Awake()
        {
            this.__Awake();

            if (this.events == null)
            {
                Debug.LogError("Events is null on " + this.gameObject.name);
                return;
            }

            this.pubsubHosts = new PubsubHost[1];
            this.pubsubHosts[0] = this.events;
        }

        protected virtual void OnDebugLog(string message) { }

        protected virtual void OnVideoPlayerInit() { }

        protected virtual void OnBrightnessChange(float alpha) { }

        protected virtual void OnVolumeChange(float volume) { }

        protected virtual void OnFpsChange(int fps) { }

        protected virtual void OnScreenResolutionChange(
            ScreenHandler screen,
            float width,
            float height
        ) { }

        protected virtual void OnScreenTextureChange() { }

        protected virtual void OnPlayerSwitch(string type) { }

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

        protected virtual void OnOwnershipChanged(int previousOwnerId, VRCPlayerApi nextOwner) { }

        protected virtual void OnOwnershipSecurityChanged(bool locked) { }

        protected virtual void OnOwnershipRequested() { }

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

                case nameof(VideoEvent.OnDebugLog):
                {
                    string message = (string)data[0];
                    this.OnDebugLog(message);
                    return;
                }

                case nameof(VideoEvent.OnLoadRequested):
                {
                    VRCUrl url = (VRCUrl)data[0];
                    this.OnLoadRequested(url);
                    return;
                }

                case nameof(VideoEvent.OnLoadApproved):
                {
                    VRCUrl url = (VRCUrl)data[0];
                    this.OnLoadApproved(url);
                    return;
                }

                case nameof(VideoEvent.OnLoadDenied):
                {
                    VRCUrl url = (VRCUrl)data[0];
                    string reason = (string)data[1];
                    this.OnLoadDenied(url, reason);
                    return;
                }

                case nameof(VideoEvent.OnLoadBegin):
                {
                    VRCUrl url = (VRCUrl)data[0];
                    if (url == null)
                        this.OnLoadBegin();
                    else
                        this.OnLoadBegin(url);
                    return;
                }

                case nameof(VideoEvent.OnBrightnessChange):
                {
                    float alpha = (float)data[0];
                    this.OnBrightnessChange(alpha);
                    return;
                }

                case nameof(VideoEvent.OnVolumeChange):
                {
                    float volume = (float)data[0];
                    this.OnVolumeChange(volume);
                    return;
                }

                case nameof(VideoEvent.OnFpsChange):
                {
                    int fps = (int)data[0];
                    this.OnFpsChange(fps);
                    return;
                }

                case nameof(VideoEvent.OnScreenResolutionChange):
                {
                    ScreenHandler screen = (ScreenHandler)data[0];
                    float width = (float)data[1];
                    float height = (float)data[2];
                    this.OnScreenResolutionChange(screen, width, height);
                    return;
                }

                case nameof(VideoEvent.OnLoadReady):
                {
                    float duration = (float)data[0];
                    this.OnLoadReady(duration);
                    return;
                }

                case nameof(VideoEvent.OnLoadError):
                {
                    VideoError error = (VideoError)data[0];
                    this.OnLoadError(error);
                    return;
                }

                case nameof(VideoEvent.OnLoadRatelimitWaiting):
                {
                    this.OnLoadRatelimitWaiting();
                    return;
                }

                case nameof(VideoEvent.OnUnload):
                {
                    this.OnUnload();
                    return;
                }

                case nameof(VideoEvent.OnPlaybackStart):
                {
                    float timestamp = (float)data[0];
                    this.OnPlaybackStart(timestamp);
                    return;
                }

                case nameof(VideoEvent.OnPlaybackStop):
                {
                    float timestamp = (float)data[0];
                    this.OnPlaybackStop(timestamp);
                    return;
                }

                case nameof(VideoEvent.OnProgress):
                {
                    float timestamp = (float)data[0];
                    float duration = (float)data[1];
                    this.OnProgress(timestamp, duration);
                    return;
                }

                case nameof(VideoEvent.OnPlaybackEnd):
                {
                    this.OnPlaybackEnd();
                    return;
                }

                case nameof(VideoEvent.OnVideoPlayerInit):
                {
                    this.OnVideoPlayerInit();
                    return;
                }

                case nameof(VideoEvent.OnPlayerSwitch):
                {
                    string type = (string)data[0];
                    this.OnPlayerSwitch(type);
                    return;
                }

                #endregion

                #region Plugins

                case nameof(VideoEvent.OnAutoRetry):
                {
                    int attempt = (int)data[0];
                    this.OnAutoRetry(attempt);
                    return;
                }

                case nameof(VideoEvent.OnAutoRetryLoadTimeout):
                {
                    int timeout = (int)data[0];
                    this.OnAutoRetryLoadTimeout(timeout);
                    return;
                }

                case nameof(VideoEvent.OnAutoRetryAbort):
                {
                    this.OnAutoRetryAbort();
                    return;
                }

                case nameof(VideoEvent.OnAutoRetryAllPlayersFailed):
                {
                    this.OnAutoRetryAllPlayersFailed();
                    return;
                }

                case nameof(VideoEvent.OnOwnershipChanged):
                {
                    int previousOwnerId = (int)data[0];
                    VRCPlayerApi nextOwner = (VRCPlayerApi)data[1];
                    this.OnOwnershipChanged(previousOwnerId, nextOwner);
                    return;
                }

                case nameof(VideoEvent.OnOwnershipSecurityChanged):
                {
                    bool locked = (bool)data[0];
                    this.OnOwnershipSecurityChanged(locked);
                    return;
                }

                case nameof(VideoEvent.OnOwnershipRequested):
                {
                    this.OnOwnershipRequested();
                    return;
                }

                case nameof(VideoEvent.OnScreenTextureChange):
                {
                    this.OnScreenTextureChange();
                    return;
                }

                case nameof(VideoEvent.OnRemotePlayerLoaded):
                {
                    int loadedPlayers = (int)data[0];
                    this.OnRemotePlayerLoaded(loadedPlayers);
                    return;
                }

                case nameof(VideoEvent.OnMetadataChange):
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

                case nameof(VideoEvent.OnSubtitleRender):
                {
                    string text = (string)data[0];
                    this.OnSubtitleRender(text);
                    return;
                }

                case nameof(VideoEvent.OnSubtitleClear):
                {
                    this.OnSubtitleClear();
                    return;
                }

                case nameof(VideoEvent.OnSubtitleLanguageOptionsChange):
                {
                    string[][] newOptions = (string[][])data;
                    this.OnSubtitleLanguageOptionsChange(newOptions);
                    return;
                }

                case nameof(VideoEvent.OnSubtitleLanguageRequested):
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
