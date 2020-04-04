using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kameosa
{
    namespace Editor
    {
        public static class Common
        {
            public static bool ConfirmationDialog(string confirmation, string title = "Warning!", string yes = "Yes", string no = "No")
            {
                return EditorUtility.DisplayDialog(title, confirmation, yes, no);
            }

            public static bool InformationDialog(string title, string message, string ok = "Ok")
            {
                return EditorUtility.DisplayDialog(title, message, ok);
            }

            // Folders, Files, Assets

            public static void CreateFolder(string path)
            {
                List<string> folders = new List<string>(path.Split('\\'));

                string parentPath = "";                

                for (int i = 0; i < folders.Count; i++) 
                {
                    if (i != 0 && !AssetDatabase.IsValidFolder(Path.Combine(parentPath, folders[i])))
                    {
                        AssetDatabase.CreateFolder(parentPath, folders[i]);
                        string folderPath =  Path.Combine(parentPath, folders[i]);

                        if (Preferences.isPrintLog)
                        {
                            Debug.Log("Creating " + folderPath);
                        }

                        CopyAsset(Path.Combine(Constants.KAMEOSA_EDITOR_SCRIPT_TEMPLATES_FOLDER_PATH, "gitkeep.txt"), Path.Combine(folderPath, ".gitkeep"));
                    }

                    parentPath = i == 0 ? folders[i] : Path.Combine(parentPath, folders[i]);
                }
            }

            public static void DeleteFolder(string path)
            {
                DeleteAsset(path);
            }

            public static void CopyAsset(string source, string destination, bool force = false)
            {
                if (Preferences.isPrintLog)
                {
                    Debug.Log("Copying " + source + " to " + destination);
                }

                string destinationFolderPath = destination.Substring(0, destination.LastIndexOf('\\'));
                CreateFolder(destinationFolderPath);

                if (!force && System.IO.File.Exists(destination))
                {
                    if (Preferences.isPrintLog)
                    {
                        Debug.Log(destination + " already exists! Aborting...");
                    }

                    return;
                }

                AssetDatabase.CopyAsset(source, destination);
            }

            public static void DeleteAsset(string path)
            {
                if (!AssetDatabase.DeleteAsset(path))
                {
                    Debug.LogError(path + " is not a valid path! Aborting...");
                }
            }



            // Scenes

            //public static void CreateScene(string name)
            //{
            //    if (Preferences.isPrintLog)
            //    {
            //        Debug.Log("Creating " + name + " Scene.");
            //    }

            //    EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            //    Camera.main.name = "MainCamera";

            //    string path = GetScenePath(name);

            //    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), path);
            //    AddSceneToBuildSettings(path);
            //}

            //public static void CopySceneFromTemplate(string scene, bool force = false)
            //{
            //    if (Preferences.isPrintLog)
            //    {
            //        Debug.Log("Copying " + scene + " from template");
            //    }

            //    string sourcePath = Path.Combine(Constants.KAMEOSA_EDITOR_SCENE_TEMPLATES_FOLDER_PATH, scene + Constants.SCENE_EXTENSION);
            //    string destinationPath = Path.Combine(Constants.SCENES_FOLDER_PATH, scene + Constants.SCENE_EXTENSION);

            //    if (!force && System.IO.File.Exists(destinationPath))
            //    {
            //        if (Preferences.isPrintLog)
            //        {
            //            Debug.Log(destinationPath + " already exists! Aborting...");
            //        }

            //        return;
            //    }

            //    AssetDatabase.CopyAsset(sourcePath, destinationPath);
            //}

            //private static void AddSceneToBuildSettings(string path)
            //{
            //    UnityEditor.EditorBuildSettingsScene[] scenes = UnityEditor.EditorBuildSettings.scenes;
            //    UnityEditor.EditorBuildSettingsScene[] newScenes = new UnityEditor.EditorBuildSettingsScene[scenes.Length + 1];

            //    System.Array.Copy(scenes, newScenes, scenes.Length);

            //    UnityEditor.EditorBuildSettingsScene scene = new UnityEditor.EditorBuildSettingsScene(path, true);
            //    newScenes[newScenes.Length - 1] = scene;
            //    UnityEditor.EditorBuildSettings.scenes = newScenes;
            //}

            //public static void OpenScene(string sceneName)
            //{
            //    EditorSceneManager.OpenScene(Path.Combine(Editor.Constants.SCENES_FOLDER_PATH, sceneName + ".unity"));
            //}



            // GameObjects, Prefabs

            //public static string CreatePrefabFromTemplate(string prefabName, string folderName = "", bool force = false)
            //{
            //    if (Preferences.isPrintLog)
            //    {
            //        Debug.Log("Creating " + prefabName + Constants.PREFAB_EXTENSION + " from template");
            //    }

            //    string sourcePath = Path.Combine(Constants.KAMEOSA_EDITOR_PREFAB_TEMPLATES_FOLDER_PATH, prefabName + Constants.PREFAB_TEMPLATE_EXTENSION);
            //    string destinationPath = string.IsNullOrEmpty(folderName)
            //        ? Constants.PREFABS_FOLDER_PATH
            //        : Path.Combine(Constants.PREFABS_FOLDER_PATH, folderName);
            //    destinationPath = Path.Combine(destinationPath, prefabName + Constants.PREFAB_EXTENSION);

            //    CopyAsset(sourcePath, destinationPath, force);

            //    return destinationPath;
            //}

            //public static UnityEngine.Object InstantiatePrefab(string prefabPath)
            //{
            //    if (Preferences.isPrintLog)
            //    {
            //        Debug.Log("Instantiating prefab at " + prefabPath);
            //    }

            //    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(UnityEngine.GameObject));

            //    if (UnityEngine.GameObject.Find(prefab.name) != null)
            //    {
            //        UnityEngine.Object.DestroyImmediate(UnityEngine.GameObject.Find(prefab.name));
            //    }

            //    PrefabUtility.InstantiatePrefab(prefab);

            //    return prefab;
            //}

            //public static void SetupCamera()
            //{
            //    Camera.main.clearFlags = CameraClearFlags.SolidColor;
            //    Camera.main.backgroundColor = Color.gray;
            //}

            //public static UnityEngine.GameObject CreateText(string name, string s, UnityEngine.GameObject parent = null, Canvas canvas = null)
            //{
            //    UnityEngine.GameObject text = CreateGameObject(name);

            //    if (parent != null)
            //    {
            //        text.transform.SetParent(parent.transform, false);
            //    }
            //    else
            //    {
            //        if (canvas == null)
            //        {
            //            canvas = UnityEngine.GameObject.FindObjectOfType<Canvas>();

            //            if (canvas == null)
            //            {
            //                canvas = CreateCanvas().GetComponent<Canvas>();
            //            }
            //        }

            //        text.transform.SetParent(canvas.transform, false);
            //    }

            //    text.AddComponent<Text>();
            //    text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            //    text.GetComponent<Text>().text = s;
            //    text.GetComponent<Text>().fontSize = Constants.DEFAULT_TEXT_FONT_SIZE;

            //    text.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector2(0f, -50f);
            //    text.GetComponent<RectTransform>().anchorMin = new UnityEngine.Vector2(0f, 1f);
            //    text.GetComponent<RectTransform>().anchorMax = UnityEngine.Vector2.one;
            //    text.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(0f, Constants.DEFAULT_TEXT_HEIGHT);

            //    return text;
            //}


            //public static void CreateEventSystem()
            //{
            //    UnityEngine.GameObject eventSystem = new UnityEngine.GameObject("EventSystem", typeof(EventSystem));
            //    eventSystem.AddComponent<StandaloneInputModule>();
            //}

          




            // Scripts

            //public static void CreateScriptFromTemplate(string scriptName, string folderPath = Constants.SCRIPTS_FOLDER_PATH, bool force = false)
            //{
            //    if (Preferences.isPrintLog)
            //    {
            //        Debug.Log("Creating " + scriptName + Constants.SCRIPT_EXTENSION + " from template");
            //    }

            //    string sourcePath = GetScriptTemplatePath(scriptName);
            //    string destinationPath = Path.Combine(folderPath, scriptName + Constants.SCRIPT_EXTENSION);

            //    if (Preferences.isPrintLog)
            //    {
            //        Debug.Log(sourcePath);
            //        Debug.Log(destinationPath);
            //    }

            //    CopyAsset(sourcePath, destinationPath, force);
            //    AssetDatabase.ImportAsset(destinationPath);
            //}

            //public static void AttachScriptToPrefab(string scriptPath, string prefabPath)
            //{
            //    MonoScript script = AssetDatabase.LoadMainAssetAtPath(scriptPath) as MonoScript;
            //    UnityEngine.GameObject prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(UnityEngine.GameObject)) as UnityEngine.GameObject;

            //    prefab.AddComponent(script.GetClass());
            //}

            //public static void AttachScriptToGameObject(string scriptPath, UnityEngine.GameObject gameObject)
            //{
            //    MonoScript script = AssetDatabase.LoadMainAssetAtPath(scriptPath) as MonoScript;

            //    if (Preferences.isPrintLog)
            //    {
            //        Debug.Log(scriptPath);
            //        Debug.Log(script.GetClass().ToString());
            //    }

            //    gameObject.AddComponent(script.GetClass());
            //}

            //public static string GetScriptTemplatePath(string fileName)
            //{
            //    return Path.Combine(Constants.KAMEOSA_EDITOR_SCRIPT_TEMPLATES_FOLDER_PATH, fileName + Constants.SCRIPT_TEMPLATE_EXTENSION);
            //}

            //public static string GetSceneControllerScriptPath(string fileName)
            //{
            //    return Path.Combine(Constants.SCRIPTS_SCENES_FOLDER_PATH, fileName + Constants.SCRIPT_EXTENSION);
            //}

            //public static string GetManagerScriptPath(string fileName)
            //{
            //    return Path.Combine(Constants.SCRIPTS_MANAGERS_FOLDER_PATH, fileName + Constants.SCRIPT_EXTENSION);
            //}

            //public static string GetScriptPath(string fileName, string folderPath)
            //{
            //    return Path.Combine(folderPath, fileName + Constants.SCRIPT_EXTENSION);
            //}

            //public static string GetScenePath(string sceneName)
            //{
            //    return Path.Combine(Constants.SCENES_FOLDER_PATH, sceneName + Constants.SCENE_EXTENSION);
            //}
        }
    }
}