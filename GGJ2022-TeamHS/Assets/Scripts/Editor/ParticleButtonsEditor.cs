using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(ParticleButtons))]
public class ParticleButtonsEditor : ButtonEditor
{
    private SerializedProperty s_particles;

    protected override void OnEnable()
    {
        base.OnEnable();
        s_particles = serializedObject.FindProperty("particles");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(s_particles, new GUIContent("Particles"), true);
        serializedObject.ApplyModifiedProperties();
    }
}
