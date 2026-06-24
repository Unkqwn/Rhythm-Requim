using UnityEngine;
using UnityEditor;

public class PrefabReplacer : ScriptableWizard
{
    public GameObject NewPrefab;

    [MenuItem("Tools/Replace Selected With Prefab")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<PrefabReplacer>("Replace Selection", "Replace");
    }

    void OnWizardCreate()
    {
        // Get all currently selected objects in the Hierarchy
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0 || NewPrefab == null)
        {
            Debug.LogError("Please select objects in the hierarchy and assign a prefab!");
            return;
        }

        // Register an Undo operation in Unity so you can hit Ctrl+Z if you make a mistake
        Undo.RegisterCompleteObjectUndo(selectedObjects, "Replace Prefabs");

        foreach (GameObject go in selectedObjects)
        {
            // Instantiate the new prefab at the exact same position, rotation, and parent scale
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(NewPrefab);
            Undo.RegisterCreatedObjectUndo(newObject, "Replace Prefabs");

            newObject.transform.SetParent(go.transform.parent);
            newObject.transform.position = go.transform.position;
            newObject.transform.rotation = go.transform.rotation;
            newObject.transform.localScale = go.transform.localScale;

            // Destroy the old cube asset completely
            Undo.DestroyObjectImmediate(go);
        }

        Debug.Log($"Successfully replaced {selectedObjects.Length} objects with the new prefab!");
    }
}