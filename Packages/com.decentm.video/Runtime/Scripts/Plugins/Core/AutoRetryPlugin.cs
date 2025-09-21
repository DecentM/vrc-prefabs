using UnityEngine;
using VRC.SDK3.Components.Video;
using VRC.SDKBase;

namespace DecentM.Video.Plugins
{
    /// <summary>
    /// When loading the video fails, this plugin will cause the player to retry a number of times.
    /// If there are multiple players configured, it'll try them all, sequentially.
    /// </summary>
    internal sealed class AutoRetryPlugin : VideoPlugin
    {
        [Tooltip("Switch to the next player handler after this many failures. Each attempt takes 5 seconds."), SerializeField] private int failureCeiling = 2;
        [Tooltip("Abort loading if all players error. If false, the first player will be tried again."), SerializeField] private bool abortAfterAllPlayersFailed = true;
        
        private int failures = 0;

        [Tooltip("If a player doesn't start playing in this amount of seconds, consider the load attempt as errored.")] private int videoLoadTimeout = 10;

        private float timeoutClock = 0;
        private bool waitingForLoad = false;

        private void FixedUpdate()
        {
            if (!this.waitingForLoad)
                return;

            this.timeoutClock += Time.fixedDeltaTime;
            if (this.timeoutClock > this.videoLoadTimeout + ((this.failures + 1) * 5))
            {
                this.timeoutClock = 0;
                this.OnLoadError(VideoError.Unknown);
                this.events.OnAutoRetryLoadTimeout((this.failures + 1) * 5);
            }
        }

        public void AttemptRetry()
        {
            VRCUrl url = this.system.GetUrl();
            if (url == null)
                return;

            this.system.RequestVideo(url);
        }

        protected override void OnLoadRequested(VRCUrl url)
        {
            this.timeoutClock = 0;
            this.waitingForLoad = true;
        }

        protected override void OnLoadError(VideoError videoError)
        {
            VRCUrl url = this.system.GetUrl();

            if (url == null || string.IsNullOrEmpty(this.system.GetUrl().ToString()))
                return;

            switch (videoError)
            {
                // Continue for rate limit errors and unknown ones
                // (we repurposed unknown to mean player timeout as well)
                case VideoError.Unknown:
                case VideoError.RateLimited:
                case VideoError.PlayerError:
                    break;
                // Don't retry for errors that are unlikely to be temporary
                case VideoError.InvalidURL:
                case VideoError.AccessDenied:
                    this.timeoutClock = 0;
                    this.failures = 0;
                    this.events.OnAutoRetryAbort();
                    return;
            }

            this.waitingForLoad = false;
            this.failures++;

            // If we reach a limit, try with the next player handler as the current one may be broken.
            if (this.failures >= this.failureCeiling)
            {
                this.failures = 0;

                // If the next player handler index is 0, it means we've gone around all of them
                // TODO: This above statement isn't true if we didn't start playback using the first player
                if (this.system.NextPlayerHandler() == 0)
                {
                    if (abortAfterAllPlayersFailed)
                    {
                        this.system.Stop();
                        this.events.OnAutoRetryAbort();
                        return;
                    }
                    else
                    {
                        this.events.OnAutoRetryAllPlayersFailed();
                    }
                }
            }

            // Schedule a retry after the rate limit expires
            this.events.OnAutoRetry(this.failures);
            this.SendCustomEventDelayedSeconds(nameof(AttemptRetry), 5.1f);
        }

        protected override void OnLoadReady(float duration)
        {
            // Reset the failure count after a video loads successfully
            this.failures = 0;
            this.waitingForLoad = false;
        }

        protected override void OnStop()
        {
            this.timeoutClock = 0;
            this.failures = 0;
            this.waitingForLoad = false;
        }
    }
}