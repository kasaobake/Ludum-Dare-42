using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kameosa
{
    namespace Editor
    {
        namespace Project
        {
            public class SetupDefaultFolders
            {
                [MenuItem("Kameosa/Project/Setup Default Folders", false, -1)]
                private static void Main()
                {
                    Debug.Log("Setting up default folders.");
                    Common.CreateFolder(Constants.ASSETS_FOLDER_PATH);

                    Common.CreateFolder(Constants.ANIMATIONS_FOLDER_PATH);
                    Common.CreateFolder(Constants.ANIMATIONS_GAMEPLAY_FOLDER_PATH);
                    Common.CreateFolder(Constants.ANIMATIONS_ENEMIES_FOLDER_PATH);
                    Common.CreateFolder(Constants.ANIMATIONS_PLAYER_FOLDER_PATH);
                    Common.CreateFolder(Constants.ANIMATIONS_UI_FOLDER_PATH);

                    Common.CreateFolder(Constants.AUDIOS_FOLDER_PATH);
                    Common.CreateFolder(Constants.FONTS_FOLDER_PATH);
                    Common.CreateFolder(Constants.MATERIALS_FOLDER_PATH);

                    Common.CreateFolder(Constants.PREFABS_FOLDER_PATH);
                    Common.CreateFolder(Constants.PREFABS_GAMEPLAY_FOLDER_PATH);
                    Common.CreateFolder(Constants.PREFABS_ENEMIES_FOLDER_PATH);
                    Common.CreateFolder(Constants.PREFABS_PLAYER_FOLDER_PATH);
                    Common.CreateFolder(Constants.PREFABS_UI_FOLDER_PATH);

                    Common.CreateFolder(Constants.SCENES_FOLDER_PATH);

                    Common.CreateFolder(Constants.SPRITES_FOLDER_PATH);
                    Common.CreateFolder(Constants.SPRITES_GAMEPLAY_FOLDER_PATH);
                    Common.CreateFolder(Constants.SPRITES_ENEMIES_FOLDER_PATH);
                    Common.CreateFolder(Constants.SPRITES_PLAYER_FOLDER_PATH);
                    Common.CreateFolder(Constants.SPRITES_UI_FOLDER_PATH);

                    Common.CreateFolder(Constants.SCRIPTS_FOLDER_PATH);
                    Common.CreateFolder(Constants.SCRIPTS_GAMEPLAY_FOLDER_PATH);
                    Common.CreateFolder(Constants.SCRIPTS_ENEMIES_FOLDER_PATH);
                    Common.CreateFolder(Constants.SCRIPTS_PLAYER_FOLDER_PATH);
                    Common.CreateFolder(Constants.SCRIPTS_UI_FOLDER_PATH);
                    Common.CreateFolder(Constants.SCRIPTS_SCENES_FOLDER_PATH);
                    Common.CreateFolder(Constants.SCRIPTS_MANAGERS_FOLDER_PATH);
                    
                    Common.CreateFolder(Constants.STREAMING_ASSETS_FOLDER_PATH);
                }
            }
        }
    }
}
