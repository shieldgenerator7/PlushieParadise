using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CrusherColliderFixer))]
public class CrusherColliderFixerEditor : Editor {

    CrusherColliderFixer ccf;

    // Use this for initialization
    private void OnEnable()
    {
        ccf = (CrusherColliderFixer)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUI.enabled = !EditorApplication.isPlaying;
        if (GUILayout.Button("Fix all crusher colliders"))
        {
            ccf.fixAllCrusherColliders();
        }
    }
}
