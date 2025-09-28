using DecentM.Pubsub;
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;

namespace DecentM.PerformanceObserver
{
    public sealed class PerformanceObserverMode
    {
        public void High() { }
        public void Medium() { }
        public void Low() { }
    }

    /// <summary>
    /// Monitors the current framerate and broadcasts performance mode changes based on given thresholds.
    /// </summary>
    [
        UdonBehaviourSyncMode(BehaviourSyncMode.None),
        AddComponentMenu("DecentM/PerformanceObserver/PerformanceObserver"),
        RequireComponent(typeof(PerformanceObserverEvents))
    ]
    public sealed class PerformanceObserverSystem : UdonSharpBehaviour
    {
        private string currentMode = nameof(PerformanceObserverMode.High);

        private PerformanceObserverEvents events;

        private void Start()
        {
            this.events = this.GetComponent<PerformanceObserverEvents>();
        }

        [Header("FPS Targets")]

        [SerializeField, Range(40, 144)] private int high = 60;
        [SerializeField, Range(0, 40)] private int low = 30;

        [Header("Settings")]

        [SerializeField, Range(0, 40), Tooltip(
          "Wait for this amount of FPS to be gained to move back to a higher mode. This prevents going back-and-forth when a lower mode improves FPS slightly."
        )]
        private int headroom = 20;

        [SerializeField, Range(1, 20), Tooltip(
          "Count frames until this amount of seconds pass. The more this is, the slower modes will change. The less this is, the more volatile the modes will change."
        )] private float checkEverySeconds = 10;

        private int frameCounter = 0;
        private float elapsed = 0;
        private float lastFpsAverage = 0;

        /// <summary>
        /// Returns the average FPS calculated over the last check interval.
        /// </summary>
        [PublicAPI]
        public float GetFps()
        {
            return this.lastFpsAverage;
        }

        private void LateUpdate()
        {
            this.frameCounter++;
            this.elapsed += Time.unscaledDeltaTime;

            // Tally up the number of frames we had over the last x seconds, then reset
            if (this.elapsed > this.checkEverySeconds)
            {
                this.CheckFrames();
                this.elapsed = 0;
                this.frameCounter = 0;
            }
        }

        private void CheckFrames()
        {
            float fpsAverage = this.frameCounter / this.checkEverySeconds;
            this.lastFpsAverage = fpsAverage;
            // Check the current framerate and increase of decrease the level based on the current FPS + some headroom
            // to prevent switching back and forth quickly
            switch (this.currentMode)
            {
                default:
                case nameof(PerformanceObserverMode.High):
                    // Switch to medium mode if the FPS is lower than "high"
                    if (fpsAverage < this.high)
                    {
                        this.currentMode = nameof(PerformanceObserverMode.Medium);
                        this.events.OnPerformanceModeChange(this.currentMode, fpsAverage);
                    }
                    break;

                case nameof(PerformanceObserverMode.Medium):
                    // Switch to low mode if the FPS is lower than "low"
                    if (fpsAverage < this.low)
                    {
                        this.currentMode = nameof(PerformanceObserverMode.Low);
                        this.events.OnPerformanceModeChange(this.currentMode, fpsAverage);
                    }

                    // Switch to high mode if the FPS is higher than "high" plus headroom
                    if (fpsAverage > this.high + this.headroom)
                    {
                        this.currentMode = nameof(PerformanceObserverMode.High);
                        this.events.OnPerformanceModeChange(this.currentMode, fpsAverage);
                    }
                    break;

                case nameof(PerformanceObserverMode.Low):
                    // Switch to medium mode if the FPS is higher than "medium" plus headroom
                    if (fpsAverage > this.low + this.headroom)
                    {
                        this.currentMode = nameof(PerformanceObserverMode.Medium);
                        this.events.OnPerformanceModeChange(this.currentMode, fpsAverage);
                    }
                    break;
            }
        }
    }
}
