using UdonSharp;
using UnityEngine;

namespace DecentM.PerformanceObserver.Plugins
{
    /// <summary>
    /// Disables specified GameObjects when performance mode changes.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), AddComponentMenu("DecentM/PerformanceObserver/Plugins/DisableGameObjectsPlugin")]

    internal sealed class DisableGameObjectsPlugin : PerformanceObserverPlugin
    {
        [SerializeField] private GameObject[] enableOnHigh;
        [SerializeField] private GameObject[] enableOnMedium;
        [SerializeField] private GameObject[] enableOnLow;

        protected override void _Start()
        {
            this.OnPerformanceModeChange(nameof(PerformanceObserverMode.High), 0);
        }

        private void DisableMany(GameObject[] gameObjects)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] == null)
                    continue;

                gameObjects[i].SetActive(false);
            }
        }

        private void EnableMany(GameObject[] gameObjects)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] == null)
                    continue;

                gameObjects[i].SetActive(true);
            }
        }

        protected override void OnPerformanceModeChange(string mode, float fps)
        {
            switch (mode)
            {
                case nameof(PerformanceObserverMode.High):
                    this.DisableMany(this.enableOnMedium);
                    this.DisableMany(this.enableOnLow);
                    this.EnableMany(this.enableOnHigh);
                    break;
                case nameof(PerformanceObserverMode.Medium):
                    this.DisableMany(this.enableOnHigh);
                    this.DisableMany(this.enableOnLow);
                    this.EnableMany(this.enableOnMedium);
                    break;
                case nameof(PerformanceObserverMode.Low):
                    this.DisableMany(this.enableOnHigh);
                    this.DisableMany(this.enableOnMedium);
                    this.EnableMany(this.enableOnLow);
                    break;
            }
        }
    }
}
