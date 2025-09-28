using DecentM.Pubsub;
using VRC.SDKBase;
using UdonSharp;
using VRC.SDK3.Components.Video;
using System;

namespace DecentM.Video.Plugins
{
    /// <summary>
    /// Base class, providing a fully typed interface for video events, and access to the video system
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class VideoPlugin : PubsubSubscriber
    {
        [NonSerialized] protected VideoSystem system;
        [NonSerialized] protected VideoEvents events;

        protected virtual void __Start() { }

        protected override sealed void _Start()
        {
            this.system = this.GetComponentInParent<VideoSystem>();
            this.events = this.GetComponentInParent<VideoEvents>();
            this.pubsubHosts = new PubsubHost[1];
            this.pubsubHosts[0] = this.events;

            this.__Start();
        }

        protected virtual void OnVideoPlayerInit() { }

        protected virtual void OnBrightnessChange(float alpha) { }

        protected virtual void OnVolumeChange(float volume) { }

        protected virtual void OnFpsChange(int fps) { }

        protected virtual void OnScreenResolutionChange(
            float width,
            float height
        ) { }

        protected virtual void OnScreenTextureChange() { }

        protected virtual void OnPlayerSwitch(string type) { }

        protected virtual void OnPlay(float timestamp) { }

        protected virtual void OnPause(float timestamp) { }

        protected virtual void OnStop() { }

        protected virtual void OnProgress(float timestamp, float duration) { }

        protected virtual void OnLoadBegin(VRCUrl url) { }

        protected virtual void OnLoadBegin() { }

        protected virtual void OnLoadReady(float duration) { }

        protected virtual void OnLoadError(VideoError error) { }

        protected virtual void OnLoadRequested(VRCUrl url) { }

        protected virtual void OnCustomVideoEvent(string name, object[] data) { }

        protected virtual void OnCustomVideoEvent(string name) { }

        public sealed override void OnPubsubEvent(object name, object[] data)
        {
            switch (name)
            {
                #region Core

                case nameof(VideoEvent.OnLoadRequested):
                {
                    VRCUrl url = (VRCUrl)data[0];
                    this.OnLoadRequested(url);
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
                    float width = (float)data[0];
                    float height = (float)data[1];
                    this.OnScreenResolutionChange(width, height);
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

                case nameof(VideoEvent.OnPlay):
                {
                    float timestamp = (float)data[0];
                    this.OnPlay(timestamp);
                    return;
                }

                case nameof(VideoEvent.OnPause):
                {
                    float timestamp = (float)data[0];
                    this.OnPause(timestamp);
                    return;
                }

                case nameof(VideoEvent.OnProgress):
                {
                    float timestamp = (float)data[0];
                    float duration = (float)data[1];
                    this.OnProgress(timestamp, duration);
                    return;
                }

                case nameof(VideoEvent.OnStop):
                {
                    this.OnStop();
                    return;
                }

                case nameof(VideoEvent.OnVideoPlayerInit):
                {
                    this.OnVideoPlayerInit();
                    return;
                }

                case nameof(VideoEvent.OnPlayerChange):
                {
                    string type = (string)data[0];
                    this.OnPlayerSwitch(type);
                    return;
                }

                case nameof(VideoEvent.OnCustomVideoEvent):
                {
                    bool withData = data.Length > 1;

                    if (withData)
                    {
                        string eventName = (string)data[0];
                        this.OnCustomVideoEvent(eventName);
                    }
                    else
                    {
                        string eventName = (string)data[0];
                        object[] eventData = (object[])data[1];
                        this.OnCustomVideoEvent(eventName, eventData);
                    }

                    return;
                }

                #endregion


                default:
                {
                    break;
                }
            }
        }
    }
}
