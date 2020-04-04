using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Kameosa
{
    namespace Services
    {
        public static class PlayerPrefsService
        {
            private const int TRUE = 1;
            private const int FALSE = 0;

            private const string PLAYERPREF_INITIALIZED = "PLAYERPREF_INITIALIZED";

            public static void Initialize()
            {
                SetBool(PLAYERPREF_INITIALIZED, true);
            }

            public static bool IsInitialize()
            {
                return PlayerPrefs.HasKey(PLAYERPREF_INITIALIZED);
            }

            public static void SetBool(string label, bool value)
            {
                PlayerPrefs.SetInt(label, value ? TRUE : FALSE);
            }

            public static bool GetBool(string label, bool defaultValue = false)
            {
                return PlayerPrefs.GetInt(label, defaultValue ? 1 : 0) == TRUE;
            }

            public static void SetFloat(string label, float value)
            {
                PlayerPrefs.SetFloat(label, value);
            }

            public static float GetFloat(string label, float defaultValue = 0f)
            {
                return PlayerPrefs.GetFloat(label, defaultValue);
            }

            public static void SetInt(string label, int value)
            {
                PlayerPrefs.SetInt(label, value);
            }

            public static int GetInt(string label, int defaultValue = 0)
            {
                return PlayerPrefs.GetInt(label, defaultValue);
            }

            public static void SetString(string label, string value)
            {
                PlayerPrefs.SetString(label, value);
            }

            public static string GetString(string label, string defaultValue = "")
            {
                return PlayerPrefs.GetString(label, defaultValue);
            }
        }
   }
}
