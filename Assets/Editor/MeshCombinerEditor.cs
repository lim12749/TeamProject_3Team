using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class MeshCombiner : MonoBehaviour
{
    [Header("Combine Options")]
    public bool CreateMultiMaterialMesh = false;
    public bool CombineInactiveChildren = false;
    public bool DeactivateCombinedChildren = false;
    public bool DeactivateCombinedChildrenMeshRenderers = false;
    public bool GenerateUVMap = false;
    public bool DestroyCombinedChildren = false;

    [Header("Other Settings")]
    public string FolderPath = "CombinedMeshes";
    public MeshFilter[] meshFiltersToSkip;

    public void CombineMeshes(bool logInfo = false)
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(CombineInactiveChildren);
        List<CombineInstance> combine = new List<CombineInstance>();

        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.sharedMesh == null) continue;
            if (meshFiltersToSkip != null && System.Array.IndexOf(meshFiltersToSkip, mf) >= 0) continue;

            CombineInstance ci = new CombineInstance
            {
                mesh = mf.sharedMesh,
                transform = mf.transform.localToWorldMatrix
            };
            combine.Add(ci);

            if (DeactivateCombinedChildren)
                mf.gameObject.SetActive(false);
            else if (DeactivateCombinedChildrenMeshRenderers)
            {
                MeshRenderer rend = mf.GetComponent<MeshRenderer>();
                if (rend) rend.enabled = false;
            }

            if (DestroyCombinedChildren)
                DestroyImmediate(mf.gameObject);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.name = "CombinedMesh";
        combinedMesh.CombineMeshes(combine.ToArray(), !CreateMultiMaterialMesh);

        MeshFilter myMF = GetComponent<MeshFilter>();
        if (myMF == null) myMF = gameObject.AddComponent<MeshFilter>();
        myMF.sharedMesh = combinedMesh;

        MeshRenderer myMR = GetComponent<MeshRenderer>();
        if (myMR == null) myMR = gameObject.AddComponent<MeshRenderer>();

        if (GenerateUVMap)
        {
#if UNITY_EDITOR
            UnityEditor.Unwrapping.GenerateSecondaryUVSet(combinedMesh);
#endif
        }

        if (logInfo)
            Debug.Log($"<color=green><b>{combine.Count}</b> meshes combined into one mesh successfully.</color>", this);
    }
}
