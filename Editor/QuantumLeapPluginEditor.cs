using UnityEngine;
using UnityEditor;
using QuantumLeap;

namespace QuantumLeap.Editor
{
    /// <summary>
    /// Editor utilities for the Quantum Leap Plugin
    /// </summary>
    public static class QuantumLeapPluginEditor
    {
        [MenuItem("Tools/Quantum Leap/Create Test GameObject")]
        public static void CreateTestGameObject()
        {
            GameObject testObject = new GameObject("QuantumLeapTest");
            QuantumLeapComponent component = testObject.AddComponent<QuantumLeapComponent>();
            
            Selection.activeGameObject = testObject;
            
            Debug.Log("Created QuantumLeap test GameObject. Configure the component in the Inspector and test API calls.");
        }

        [MenuItem("Tools/Quantum Leap/Test API Connection")]
        public static void TestApiConnection()
        {
            EditorCoroutineUtility.StartCoroutine(TestApiConnectionCoroutine(), null);
        }

        private static System.Collections.IEnumerator TestApiConnectionCoroutine()
        {
            Debug.Log("Testing API connection...");
            
            // Initialize the manager
            QuantumLeapManager.Initialize();
            
            // Wait for initialization
            yield return new WaitForSeconds(0.1f);
            
            try
            {
                var task = QuantumLeapManager.FetchDataAsync("https://jsonplaceholder.typicode.com/posts/1");
                
                while (!task.IsCompleted)
                {
                    yield return null;
                }

                if (task.Exception != null)
                {
                    Debug.LogError($"API test failed: {task.Exception.Message}");
                }
                else
                {
                    Debug.Log($"API test successful! Response: {task.Result}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"API test failed: {ex.Message}");
            }
        }

        [MenuItem("Tools/Quantum Leap/Plugin Info")]
        public static void ShowPluginInfo()
        {
            EditorUtility.DisplayDialog(
                "Quantum Leap Plugin", 
                "Version: 1.0.0\n\n" +
                "A Unity plugin for fetching data from APIs and logging it.\n\n" +
                "Features:\n" +
                "• HTTP API data fetching\n" +
                "• Configurable logging system\n" +
                "• Easy integration with Unity scenes\n" +
                "• Support for JSON responses\n" +
                "• Error handling and retry mechanisms\n\n" +
                "Repository: https://github.com/quantumleap/unity-plugin",
                "OK"
            );
        }
    }

    /// <summary>
    /// Utility class for running coroutines in the editor
    /// </summary>
    public static class EditorCoroutineUtility
    {
        public static void StartCoroutine(System.Collections.IEnumerator coroutine, object owner)
        {
            EditorApplication.CallbackFunction callback = null;
            callback = () =>
            {
                try
                {
                    if (!coroutine.MoveNext())
                    {
                        EditorApplication.update -= callback;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Editor coroutine error: {ex.Message}");
                    EditorApplication.update -= callback;
                }
            };
            EditorApplication.update += callback;
        }
    }
} 