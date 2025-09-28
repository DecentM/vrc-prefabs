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

        protected override void OnPerformanceModeChange(string mode, float fps)
        {
            switch (mode)
            {
                case nameof(PerformanceObserverMode.Low):
                    this.OnPerformanceLow();
                    return;

                case nameof(PerformanceObserverMode.Medium):
                    this.OnPerformanceMedium();
                    return;

                case nameof(PerformanceObserverMode.High):
                    this.OnPerformanceHigh();
                    return;
            }
        }

        private void OnPerformanceHigh()
        {
            if (!this.toggle.isOn)
                return;
            this.notifications.SendNotification(this.iconHigh, $"Performance mode changed to {nameof(PerformanceObserverMode.High)}");
        }

        private void OnPerformanceMedium()
        {
            if (!this.toggle.isOn)
                return;
            this.notifications.SendNotification(this.iconMed, $"Performance mode changed to {nameof(PerformanceObserverMode.Medium)}");
        }

        private void OnPerformanceLow()
        {
            if (!this.toggle.isOn)
                return;
            this.notifications.SendNotification(this.iconLow, $"Performance mode changed to {nameof(PerformanceObserverMode.Low)}");
        }
    }
}
