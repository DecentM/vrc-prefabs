using DecentM.Pubsub;
using System;

namespace DecentM.PerformanceObserver.Plugins
{
    public abstract class PerformanceObserverPlugin : PubsubSubscriber
    {
        [NonSerialized] protected PerformanceObserverSystem system;
        [NonSerialized] protected PerformanceObserverEvents events;

        protected virtual void _Start() { }

        private void Start()
        {
            this.system = this.GetComponentInParent<PerformanceObserverSystem>();
            this.events = this.GetComponentInParent<PerformanceObserverEvents>();
            this.Subscribe(this.events);
            this._Start();
        }

        protected virtual void OnPerformanceModeChange(string mode, float fps) { }

        public sealed override void OnPubsubEvent(object name, object[] data)
        {
            switch (name)
            {
                #region Core

                case nameof(PerformanceObserverEvent.OnPerformanceModeChange):
                {
                    string mode = (string)data[0];
                    float fps = (float)data[1];
                    this.OnPerformanceModeChange(mode, fps);
                    return;
                }

                #endregion
            }
        }
    }
}
