using DecentM.Shared.Editor;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;

namespace DecentM.PerformanceObserver.Editor
{
    [CustomEditor(typeof(PerformanceObserverSystem))]
    public class PerformanceObserverInspector : Inspector
    {
        private const int AbsoluteHigh = 144;
        private const int AbsoluteLow = 10;
        private const int VerticalSpacing = 30;

        public override void OnInspectorGUI()
        {
            UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(this.target);

            this.DrawDefaultInspector();

            Rect debugArea = EditorGUILayout.BeginVertical(GUILayout.Height(VerticalSpacing));

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            float newFps = EditorGUILayout.Slider(
                new GUIContent(
                    "Editor framerate (for debugging)",
                    "Sets the target FPS in the editor, for testing. Has no actual effect in game."
                ),
                Application.targetFrameRate,
                AbsoluteLow,
                AbsoluteHigh
            );
            EditorGUI.EndDisabledGroup();

            Application.targetFrameRate = Mathf.RoundToInt(newFps);
            EditorGUILayout.EndVertical();
        }
    }
}
