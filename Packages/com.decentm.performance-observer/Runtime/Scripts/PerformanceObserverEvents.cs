using DecentM.Pubsub;
using UdonSharp;
using UnityEngine;

namespace DecentM.PerformanceObserver
{

    internal sealed class PerformanceObserverEvent
    {
        internal void OnPerformanceModeChange() { }
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None), AddComponentMenu("DecentM/PerformanceObserver/PerformanceObserverEvents")]
    public sealed class PerformanceObserverEvents : PubsubHost
    {
        public void OnPerformanceModeChange(string currentMode, float lastFpsAverage)
        {
            this.BroadcastEvent(
                nameof(PerformanceObserverEvent.OnPerformanceModeChange),
                currentMode,
                lastFpsAverage
            );
        }
    }
}
