﻿using DarkBestiary.Components;
using UnityEditor;

namespace DarkBestiary.Editor
{
    [CustomEditor(typeof(BehavioursComponent))]
    public class BehavioursComponentEditor : UnityEditor.Editor
    {
        private BehavioursComponent behaviours;

        private void OnEnable()
        {
            this.behaviours = target as BehavioursComponent;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (var behaviour in this.behaviours.Behaviours)
            {
                EditorGUILayout.LabelField(
                    " #" + behaviour.Id + " " + behaviour.GetType() +
                    behaviour.Name + " x" + behaviour.StackCount +
                    $" ({behaviour.RemainingDuration}/{behaviour.Duration})");
            }
        }
    }
}
