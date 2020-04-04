using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Kameosa
{
    namespace Editor
    {
        namespace Project
        {
            public class Clean
            {
                [MenuItem("Kameosa/Project/Clean", false, -1)]
                private static void Main()
                {
                    if (Common.ConfirmationDialog("Are you sure you want to remove all default folders and their contents?"))
                    {
                        Debug.Log("Removing default folders.");
                        Common.DeleteFolder(Constants.ANIMATIONS_FOLDER_PATH);
                        Common.DeleteFolder(Constants.AUDIOS_FOLDER_PATH);
                        Common.DeleteFolder(Constants.FONTS_FOLDER_PATH);
                        Common.DeleteFolder(Constants.PREFABS_FOLDER_PATH);
                        Common.DeleteFolder(Constants.SCENES_FOLDER_PATH);
                        Common.DeleteFolder(Constants.SCRIPTS_GAMEPLAY_FOLDER_PATH);
                        Common.DeleteFolder(Constants.SCRIPTS_SCENES_FOLDER_PATH);
                        Common.DeleteFolder(Constants.SCRIPTS_MANAGERS_FOLDER_PATH);
                        Common.DeleteFolder(Constants.SPRITES_FOLDER_PATH);
                        Common.DeleteAsset(Path.Combine(Constants.SCRIPTS_FOLDER_PATH, "GameManager.cs"));
                    }
                }
            }
        }
    }
}
