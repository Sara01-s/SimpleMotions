using UnityEditor;
using UnityEngine;

public class MissingReferenceFinder : MonoBehaviour {

    [MenuItem("Tools/Find Missing References in Scene")]
    public static void FindMissingReferencesInScene() {
        Debug.Log("Looking for missing references...");
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects) {
            Component[] objComponents = obj.GetComponents<Component>();

            foreach (var component in objComponents) {
                if (component == null) {
                    Debug.LogError($"Missing Component in GameObject: {obj.name}", obj);
                    continue;
                }

                var serializedObj = new SerializedObject(component);
                SerializedProperty sp = serializedObj.GetIterator();

                while (sp.NextVisible(true)) {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference 
						&& sp.objectReferenceValue == null 
						&& sp.objectReferenceInstanceIDValue != 0) 
					{
                        Debug.LogError($"Missing Reference in {obj.name}, Component: {component.GetType().Name}, Property: {sp.name}", obj);
                    }
                }
            }
        }

        Debug.Log("Finished searching for missing references in the scene.");
    }
}
