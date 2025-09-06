using DecentM.Pubsub;

namespace DecentM.PerformanceObserver.Plugins
{
    public abstract class PerformanceObserverPlugin : PubsubSubscriber
    {
        protected virtual void OnPerformanceModeChange(PerformanceObserverMode mode, float fps) { }

        public sealed override void OnPubsubEvent(object name, object[] data)
        {
            switch (name)
            {
                #region Core

                case PerformanceObserverEvent.OnPerformanceModeChange:
                {
                        PerformanceObserverMode mode = (PerformanceObserverMode)data[0];
                    float fps = (float)data[1];
                    this.OnPerformanceModeChange(mode, fps);
                    return;
                }

                #endregion
            }
        }
    }
}
