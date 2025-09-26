using System;
using UnityEngine;
using VRC.SDKBase;
using UdonSharp;
using VRC.SDK3.Components.Video;
using DecentM.Collections;

namespace DecentM.Video.Plugins
{
    /// <summary>
    /// Allows inspecting all pubsub messages in the video system in the editor.
    /// <br />
    /// Attach this plugin to a video player, assign a Queue, and all the events will be visible in the Queue inspector.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    internal sealed class DebugPlugin : VideoPlugin
    {
        [SerializeField]
        private Queue/*<string>*/ logs;

        private void Log(params string[] messages)
        {
            if (this.logs.Count > 20)
            {
                this.logs.Dequeue();
            }

            this.logs.Enqueue(String.Join(" ", messages));
        }

        protected override void __Start()
        {
            this.Log(nameof(_Start));
        }

        protected override void OnVideoPlayerInit()
        {
            this.Log(nameof(OnVideoPlayerInit));
        }

        protected override void OnPlayerSwitch(string type)
        {
            this.Log(nameof(OnPlayerSwitch), type);
        }

        protected override void OnLoadReady(float duration)
        {
            this.Log(nameof(OnLoadReady), duration.ToString());
        }

        protected override void OnLoadBegin()
        {
            this.Log(nameof(OnLoadBegin));
        }

        protected override void OnLoadBegin(VRCUrl url)
        {
            this.Log(nameof(OnLoadBegin), "(with URL)");
        }

        protected override void OnLoadError(VideoError videoError)
        {
            this.Log(nameof(OnLoadError), videoError.ToString());
        }

        protected override void OnProgress(float timestamp, float duration)
        {
            this.Log(nameof(OnProgress), timestamp.ToString(), duration.ToString());
        }

        protected override void OnStop()
        {
            this.Log(nameof(OnStop));
        }

        protected override void OnPlay(float timestamp)
        {
            this.Log(nameof(OnPlay), timestamp.ToString());
        }

        protected override void OnPause(float timestamp)
        {
            this.Log(nameof(OnPause), timestamp.ToString());
        }

        protected override void OnBrightnessChange(float alpha)
        {
            this.Log(nameof(OnBrightnessChange), alpha.ToString());
        }

        protected override void OnVolumeChange(float volume)
        {
            this.Log(nameof(OnVolumeChange), volume.ToString());
        }

        protected override void OnFpsChange(int fps)
        {
            this.Log(nameof(OnFpsChange), fps.ToString());
        }

        protected override void OnScreenResolutionChange(
            float width,
            float height
        )
        {
            this.Log(
                nameof(OnScreenResolutionChange),
                width.ToString(),
                height.ToString()
            );
        }

        protected override void OnLoadRequested(VRCUrl url)
        {
            this.Log(nameof(OnLoadRequested), "(with URL)");
        }

        protected override void OnScreenTextureChange()
        {
            this.Log(nameof(OnScreenTextureChange));
        }

        protected override void OnCustomVideoEvent(string name)
        {
            this.Log(nameof(OnCustomVideoEvent));
        }

        protected override void OnCustomVideoEvent(string name, object[] data)
        {
            this.Log(nameof(OnCustomVideoEvent), $"{data.Length}");
        }
    }
}
