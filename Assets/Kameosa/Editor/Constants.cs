using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kameosa
{
    namespace Editor
    {
        /// <summary>
        /// Editor helper.
        /// </summary>
        public static class Constants {
            private const string ASSETS_FOLDER = "Assets";
            private const string ANIMATIONS_FOLDER = "Animations";
            private const string AUDIOS_FOLDER = "Audios";
            private const string ENEMIES_FOLDER = "Enemies";
            private const string FONTS_FOLDER = "Fonts";
            private const string GAMEPLAY_FOLDER = "Gameplay";
            private const string MATERIALS_FOLDER = "Materials";
            private const string PLAYER_FOLDER = "Player";
            private const string PREFABS_FOLDER = "Prefabs";
            private const string SCENES_FOLDER = "Scenes";
            private const string SCRIPTS_FOLDER = "Scripts";
            private const string SPRITES_FOLDER = "Sprites";
            private const string SPAWNERS_DESTROYERS_FOLDER = "SpawnersDestroyers";
            private const string MANAGERS_FOLDER = "Managers";
            private const string STREAMING_ASSETS_FOLDER = "StreamingAssets";
            private const string UI_FOLDER = "UI";

            public const string ASSETS_FOLDER_PATH = "Assets";

            public const string ANIMATIONS_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\" + ANIMATIONS_FOLDER;
            public const string ANIMATIONS_GAMEPLAY_FOLDER_PATH = ANIMATIONS_FOLDER_PATH + "\\" + GAMEPLAY_FOLDER;
            public const string ANIMATIONS_ENEMIES_FOLDER_PATH = ANIMATIONS_GAMEPLAY_FOLDER_PATH + "\\" + ENEMIES_FOLDER;
            public const string ANIMATIONS_PLAYER_FOLDER_PATH = ANIMATIONS_GAMEPLAY_FOLDER_PATH + "\\" + PLAYER_FOLDER;
            public const string ANIMATIONS_UI_FOLDER_PATH = ANIMATIONS_FOLDER_PATH + "\\" + UI_FOLDER;

            public const string AUDIOS_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\" + AUDIOS_FOLDER;
            public const string FONTS_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\" + FONTS_FOLDER;
            public const string MATERIALS_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\" + MATERIALS_FOLDER;

            public const string PREFABS_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\" + PREFABS_FOLDER;
            public const string PREFABS_GAMEPLAY_FOLDER_PATH = PREFABS_FOLDER_PATH + "\\" + GAMEPLAY_FOLDER;
            public const string PREFABS_ENEMIES_FOLDER_PATH = PREFABS_GAMEPLAY_FOLDER_PATH + "\\" + ENEMIES_FOLDER;
            public const string PREFABS_PLAYER_FOLDER_PATH = PREFABS_GAMEPLAY_FOLDER_PATH + "\\" + PLAYER_FOLDER;
            public const string PREFABS_UI_FOLDER_PATH = PREFABS_FOLDER_PATH + "\\" + UI_FOLDER;

            public const string SCENES_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\" + SCENES_FOLDER;

            public const string SPRITES_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\" + SPRITES_FOLDER;
            public const string SPRITES_GAMEPLAY_FOLDER_PATH = SPRITES_FOLDER_PATH + "\\" + GAMEPLAY_FOLDER;
            public const string SPRITES_ENEMIES_FOLDER_PATH = SPRITES_GAMEPLAY_FOLDER_PATH + "\\" + ENEMIES_FOLDER;
            public const string SPRITES_PLAYER_FOLDER_PATH = SPRITES_GAMEPLAY_FOLDER_PATH + "\\" + PLAYER_FOLDER;
            public const string SPRITES_UI_FOLDER_PATH = SPRITES_FOLDER_PATH+ "\\" + UI_FOLDER;

            public const string SCRIPTS_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\" + SCRIPTS_FOLDER;
            public const string SCRIPTS_GAMEPLAY_FOLDER_PATH = SCRIPTS_FOLDER_PATH + "\\" + GAMEPLAY_FOLDER;
            public const string SCRIPTS_ENEMIES_FOLDER_PATH = SCRIPTS_GAMEPLAY_FOLDER_PATH + "\\" + ENEMIES_FOLDER;
            public const string SCRIPTS_PLAYER_FOLDER_PATH = SCRIPTS_GAMEPLAY_FOLDER_PATH + "\\" + PLAYER_FOLDER;
            public const string SCRIPTS_UI_FOLDER_PATH = SCRIPTS_FOLDER_PATH + "\\" + UI_FOLDER;
            public const string SCRIPTS_GAMEPLAY_SPAWNERS_DESTROYERS_FOLDER_PATH = SCRIPTS_GAMEPLAY_FOLDER_PATH + "\\" + SPAWNERS_DESTROYERS_FOLDER;
            public const string SCRIPTS_SCENES_FOLDER_PATH = SCRIPTS_FOLDER_PATH + "\\" + SCENES_FOLDER;
            public const string SCRIPTS_MANAGERS_FOLDER_PATH = SCRIPTS_FOLDER_PATH + "\\" + MANAGERS_FOLDER;

            public const string STREAMING_ASSETS_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\" + STREAMING_ASSETS_FOLDER;

            public const string KAMEOSA_FOLDER_PATH = ASSETS_FOLDER_PATH + "\\Kameosa";
            public const string KAMEOSA_EDITOR_FOLDER_PATH = KAMEOSA_FOLDER_PATH + "\\Editor";
            public const string KAMEOSA_EDITOR_SCRIPT_TEMPLATES_FOLDER_PATH = KAMEOSA_EDITOR_FOLDER_PATH + "\\Templates\\Scripts";
            public const string KAMEOSA_EDITOR_SCENE_TEMPLATES_FOLDER_PATH = KAMEOSA_EDITOR_FOLDER_PATH + "\\Templates\\Scenes";
            public const string KAMEOSA_EDITOR_PREFAB_TEMPLATES_FOLDER_PATH = KAMEOSA_EDITOR_FOLDER_PATH + "\\Templates\\Prefabs";

            public const string SCRIPT_TEMPLATE_EXTENSION = ".txt";
            public const string SCENE_TEMPLATE_EXTENSION = ".yaml";
            public const string PREFAB_TEMPLATE_EXTENSION = ".yaml";

            public const string SCRIPT_EXTENSION = ".cs";
            public const string SCENE_EXTENSION = ".unity";
            public const string PREFAB_EXTENSION = ".prefab";
        }
    }
}
