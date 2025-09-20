using UnityEditor;
using UnityEngine;

using DecentM.Shared.Editor;

namespace DecentM.PerformanceObserver.Editor
{
    [CustomEditor(typeof(PerformanceObserver))]
    public class PerformanceObserverInspector : Inspector
    {
        private const int AbsoluteHigh = 90;
        private const int AbsoluteLow = 20;
        private const int VerticalSpacing = 30;

        public override void OnInspectorGUI()
        {
            PerformanceObserver observer = (PerformanceObserver)target;

            Rect hArea = EditorGUILayout.BeginVertical(GUILayout.Height(VerticalSpacing));
            float newHigh = EditorGUILayout.Slider(
                new GUIContent(
                    "High",
                    "Current performance will be considered high above this value"
                ),
                observer.high,
                observer.low + 1,
                AbsoluteHigh
            );
            observer.high = Mathf.RoundToInt(newHigh);
            EditorGUILayout.EndVertical();

            Rect lArea = EditorGUILayout.BeginVertical(GUILayout.Height(VerticalSpacing));
            float newLow = EditorGUILayout.Slider(
                new GUIContent(
                    "Low",
                    "Current performance will be considered low below this value"
                ),
                observer.low,
                AbsoluteLow,
                observer.high - 1
            );
            observer.low = Mathf.RoundToInt(newLow);
            EditorGUILayout.EndVertical();

            Rect debugArea = EditorGUILayout.BeginVertical(GUILayout.Height(VerticalSpacing));
            float newFps = EditorGUILayout.Slider(
                new GUIContent(
                    "Editor framerate (for debugging)",
                    "Sets the target FPS in the editor, for testing. Has no actual effect in game."
                ),
                Application.targetFrameRate,
                AbsoluteLow,
                300
            );
            Application.targetFrameRate = Mathf.RoundToInt(newFps);
            EditorGUILayout.EndVertical();

            DrawDefaultInspector();
        }
    }
}
