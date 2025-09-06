using UnityEngine;
using DecentM.Pubsub;

namespace DecentM.PerformanceObserver
{
    public enum PerformanceObserverMode
    {
        High,
        Medium,
        Low,
    }

    public enum PerformanceObserverEvent
    {
        OnPerformanceModeChange,
    }

    public class PerformanceObserver : PubsubHost
    {
        private PerformanceObserverMode currentMode = PerformanceObserverMode.High;

        // Hidden in inspector, because our custom inspector already draws a UI for these.
        // [HideInInspector]
        public int high = 60;

        // [HideInInspector]
        public int low = 30;

        [Header("Settings")]
        [Tooltip(
            "To prevent a race condition where high FPS from our improvements causes objects being re-enabled too soon, re-enabling a higher performance mode requires higher FPS than going downwards. The higher this value is, the less likely it is to cause the race condition. Best to leave it at default."
        )]
        public int headroom = 20;

        public float checkEverySeconds = 10;

        private int frameCounter = 0;
        private float elapsed = 0;

        private float lastFpsAverage = 0;

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
                case PerformanceObserverMode.High:
                    // Switch to medium mode if the FPS is lower than "high"
                    if (fpsAverage < this.high)
                    {
                        this.currentMode = PerformanceObserverMode.Medium;
                        this.OnModeChange();
                    }
                    break;

                case PerformanceObserverMode.Medium:
                    // Switch to low mode if the FPS is lower than "low"
                    if (fpsAverage < this.low)
                    {
                        this.currentMode = PerformanceObserverMode.Low;
                        this.OnModeChange();
                    }

                    // Switch to high mode if the FPS is higher than "high" plus headroom
                    if (fpsAverage > this.high + this.headroom)
                    {
                        this.currentMode = PerformanceObserverMode.High;
                        this.OnModeChange();
                    }
                    break;

                case PerformanceObserverMode.Low:
                    // Switch to medium mode if the FPS is higher than "medium" plus headroom
                    if (fpsAverage > this.low + this.headroom)
                    {
                        this.currentMode = PerformanceObserverMode.Medium;
                        this.OnModeChange();
                    }
                    break;
            }
        }

        private void OnModeChange()
        {
            this.BroadcastEvent(
                PerformanceObserverEvent.OnPerformanceModeChange,
                this.currentMode,
                this.lastFpsAverage
            );
        }
    }
}
