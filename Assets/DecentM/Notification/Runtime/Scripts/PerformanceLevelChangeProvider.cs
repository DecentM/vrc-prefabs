using UnityEngine;
using UnityEngine.UI;

using DecentM.PerformanceObserver;
using DecentM.PerformanceObserver.Plugins;

namespace DecentM.Notification.Providers
{
    public class PerformanceLevelChangeProvider : PerformanceObserverPlugin
    {
        public Notification notifications;
        public Toggle toggle;

        public Sprite iconHigh;
        public Sprite iconMed;
        public Sprite iconLow;

        protected override void OnPerformanceModeChange(PerformanceObserverMode mode, float fps)
        {
            switch (mode)
            {
                case PerformanceObserverMode.Low:
                    this.OnPerformanceLow();
                    return;

                case PerformanceObserverMode.Medium:
                    this.OnPerformanceMedium();
                    return;

                case PerformanceObserverMode.High:
                    this.OnPerformanceHigh();
                    return;
            }
        }

        private void OnPerformanceHigh()
        {
            if (!this.toggle.isOn)
                return;
            this.notifications.SendNotification(this.iconHigh, "Performance mode changed to High");
        }

        private void OnPerformanceMedium()
        {
            if (!this.toggle.isOn)
                return;
            this.notifications.SendNotification(this.iconMed, "Performance mode changed to Medium");
        }

        private void OnPerformanceLow()
        {
            if (!this.toggle.isOn)
                return;
            this.notifications.SendNotification(this.iconLow, "Performance mode changed to Low");
        }
    }
}
