using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class MeshCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MonoBehaviour mb = (MonoBehaviour)target;
        if (mb == null)
        {
            DrawDefaultInspector();
            return;
        }

        // MeshCombiner 타입 확인 (직접 참조 안 함)
        Type type = mb.GetType();
        if (type.Name != "MeshCombiner")
        {
            DrawDefaultInspector();
            return;
        }

        DrawDefaultInspector();
        GUILayout.Space(10);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("▶ Combine Meshes"))
        {
            MethodInfo combineMethod = type.GetMethod("CombineMeshes", BindingFlags.Public | BindingFlags.Instance);
            if (combineMethod != null)
            {
                combineMethod.Invoke(mb, null);
                Debug.Log("✅ MeshCombinerEditor: CombineMeshes() 실행됨");
            }
            else
            {
                Debug.LogWarning("⚠️ CombineMeshes() 메서드를 찾을 수 없습니다!");
            }
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("🧹 Clear Combined Mesh"))
        {
            MeshFilter mf = mb.GetComponent<MeshFilter>();
            if (mf != null)
            {
                mf.sharedMesh = null;
                Debug.Log("🗑️ Cleared combined mesh.");
            }
        }

        GUI.backgroundColor = Color.white;
    }
}
