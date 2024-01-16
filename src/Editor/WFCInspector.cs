using UnityEditor;
using UnityEngine;

namespace flexington.WFC
{
    [CustomEditor(typeof(WFCComponent))]
    public class WFCInspector : Editor
    {
        public WFCComponent Target => (WFCComponent)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Read Input")) Target.ReadInput();
            if (GUILayout.Button("Show Input")) Target.ShowInputGrid();
            if (GUILayout.Button("Hide Input")) Target.HideInputGrid();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Pattern")) Target.CreatePattern();
            if (GUILayout.Button("Show Pattern")) Target.ShowPatternGrid();
            if (GUILayout.Button("Hide Pattern")) Target.HidePatternGrid();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Step")) Target.OutputStep();
            if (GUILayout.Button("Reset Step")) Target.ResetSteps();
            if (GUILayout.Button("Solve")) Target.OutputSolve();
            if (GUILayout.Button("Make TM")) Target.MakeTilemap();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}