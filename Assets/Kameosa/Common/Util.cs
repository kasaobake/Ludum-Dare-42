using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kameosa
{
    namespace Common
    {
        public static class Util 
        {
            public const float DEFAULT_PANEL_MARGIN = 0f;

            //public const int DEFAULT_TEXT_FONT_SIZE = 48;
            //public const float DEFAULT_TEXT_HEIGHT = 100f;

            public const float DEFAULT_LAYOUT_GROUP_SPACING = 10f;

            public const float DEFAULT_BUTTON_WIDTH = 200f;
            public const float DEFAULT_BUTTON_HEIGHT = 60f;

            public static UnityEngine.GameObject CreateGameObject(string name, UnityEngine.GameObject parent = null)
            {
                UnityEngine.GameObject gameObject = UnityEngine.GameObject.Find(name);

                if (gameObject == null)
                {
                    gameObject = new UnityEngine.GameObject(name);

                    if (parent != null)
                    {
                        gameObject.transform.SetParent(parent.transform);
                    }
                }

                return gameObject;
            }

            public static UnityEngine.GameObject CreateCanvas(string name = "Canvas")
            {
                UnityEngine.GameObject canvas = CreateGameObject(name);
                canvas.AddComponent<Canvas>();
                canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

                canvas.AddComponent<CanvasScaler>();
                canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvas.GetComponent<CanvasScaler>().referenceResolution = Kameosa.Constants.Resolution.Mid["4:3"];
                canvas.AddComponent<GraphicRaycaster>();

                return canvas;
            }

            public static UnityEngine.GameObject CreatePanel(string name, UnityEngine.GameObject parent = null)
            {
                UnityEngine.GameObject panel = new UnityEngine.GameObject(name);
                panel.AddComponent<CanvasRenderer>();

                if (parent != null)
                {
                    panel.transform.SetParent(parent.transform, false);
                }
                else
                {
                    parent = CreateCanvas();
                    panel.transform.SetParent(parent.transform, false);
                }

                Color color = Color.white;
                color.a = 0.5f;

                panel.AddComponent<Image>();
                panel.GetComponent<Image>().color = color;

                panel.GetComponent<RectTransform>().anchorMin = UnityEngine.Vector2.zero;
                panel.GetComponent<RectTransform>().anchorMax = UnityEngine.Vector2.one;
                panel.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(-DEFAULT_PANEL_MARGIN, -DEFAULT_PANEL_MARGIN);

                return panel;
            }

            public static UnityEngine.GameObject CreateLayoutContainer(string name = "LayoutContainer", UnityEngine.GameObject parent = null, bool isHorizontalLayoutGroup = false)
            {
                UnityEngine.GameObject layoutContainer = new UnityEngine.GameObject(name);

                if (parent != null)
                {
                    layoutContainer.transform.SetParent(parent.transform, false);
                }
                else
                {
                    parent = CreateCanvas();
                    layoutContainer.transform.SetParent(parent.transform, false);
                }

                if (isHorizontalLayoutGroup)
                {
                    layoutContainer.AddComponent<HorizontalLayoutGroup>();
                    layoutContainer.GetComponent<HorizontalLayoutGroup>().spacing = DEFAULT_LAYOUT_GROUP_SPACING;
                    layoutContainer.GetComponent<HorizontalLayoutGroup>().childForceExpandHeight = false;
                    layoutContainer.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
                }
                else
                {
                    layoutContainer.AddComponent<VerticalLayoutGroup>();
                    layoutContainer.GetComponent<VerticalLayoutGroup>().spacing = DEFAULT_LAYOUT_GROUP_SPACING;
                    //layoutContainer.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = false;
                    layoutContainer.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
                }

                layoutContainer.GetComponent<RectTransform>().anchorMin = new UnityEngine.Vector2(0.05f, 0.05f);
                //layoutContainer.GetComponent<RectTransform>().anchorMin = UnityEngine.Vector2.zero;
                layoutContainer.GetComponent<RectTransform>().anchorMax = new UnityEngine.Vector2(0.95f, 0.95f);
                //layoutContainer.GetComponent<RectTransform>().anchorMax = UnityEngine.Vector2.one;
                layoutContainer.GetComponent<RectTransform>().sizeDelta = UnityEngine.Vector2.zero;

                return layoutContainer;
            }

            public static UnityEngine.GameObject CreateHorizontalLayoutContainer(string name = "LayoutContainer", UnityEngine.GameObject parent = null)
            {
                return CreateLayoutContainer(name, parent, true);
            }

            public static UnityEngine.GameObject CreateVerticalLayoutContainer(string name = "LayoutContainer", UnityEngine.GameObject parent = null)
            {
                return CreateLayoutContainer(name, parent, false);
            }

            public static UnityEngine.GameObject CreateButton(string name, UnityEngine.GameObject parent = null)
            {
                UnityEngine.GameObject button = new UnityEngine.GameObject(name);
                button.AddComponent<Button>();

                if (parent != null)
                {
                    button.transform.SetParent(parent.transform, false);
                }
                else
                {
                    parent = CreateCanvas();
                    button.transform.SetParent(parent.transform, false);
                }

                button.AddComponent<RectTransform>();
                button.GetComponent<RectTransform>().offsetMin = UnityEngine.Vector2.zero;
                button.GetComponent<RectTransform>().offsetMax = UnityEngine.Vector2.zero;
                button.GetComponent<RectTransform>().anchorMin = new UnityEngine.Vector2(0.5f, 0.5f);
                button.GetComponent<RectTransform>().anchorMax = new UnityEngine.Vector2(0.5f, 0.5f);
                button.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(DEFAULT_BUTTON_WIDTH, DEFAULT_BUTTON_HEIGHT);

                Image image = button.AddComponent<Image>();
                Color color = Color.white;
                image.color = color;

                UnityEngine.GameObject text = new UnityEngine.GameObject("Label");

                text.transform.SetParent(button.transform);
                text.AddComponent<Text>();
                text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                text.GetComponent<Text>().text = name;
                text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                text.GetComponent<Text>().color = Color.black;

                text.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector2(0f, 0f);
                text.GetComponent<RectTransform>().anchorMin = UnityEngine.Vector2.zero;
                text.GetComponent<RectTransform>().anchorMax = UnityEngine.Vector2.one;
                text.GetComponent<RectTransform>().sizeDelta = UnityEngine.Vector2.zero;

                return button;
            }

            public static string SceneNameFromSceneIndex(int sceneIndex)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
                int slash = path.LastIndexOf('/');
                string name = path.Substring(slash + 1);
                int dot = name.LastIndexOf('.');

                return name.Substring(0, dot);
            }
        }
    }
}
