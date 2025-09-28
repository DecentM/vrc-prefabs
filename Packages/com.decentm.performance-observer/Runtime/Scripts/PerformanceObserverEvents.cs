using DecentM.Pubsub;
using UdonSharp;

namespace DecentM.PerformanceObserver
{

    internal sealed class PerformanceObserverEvent
    {
        internal void OnPerformanceModeChange() { }
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
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
