using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class MissingScriptsCleaner
    {
        [MenuItem("Tools/Cleanup/Remove Missing Scripts In Selection")]
        public static void CleanSelection()
        {
            foreach (var obj in Selection.gameObjects)
            {
                CleanIn(obj);
            }
            Debug.Log("Done: Removed Missing Scripts in selection.");
        }

        private static void CleanIn(GameObject root)
        {
            var transforms = root.GetComponentsInChildren<Transform>(true);
            int removed = 0;
            foreach (var t in transforms)
            {
                // ฟังก์ชันนี้มีใน UnityEditor.GameObjectUtility
                removed += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(t.gameObject);
            }
            if (removed > 0)
                Debug.Log($"{root.name}: removed {removed} missing script(s).", root);
        }
    }
}