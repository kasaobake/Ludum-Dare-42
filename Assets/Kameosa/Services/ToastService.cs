using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kameosa
{
    namespace Services
    {
        public static class ToastService
        {
            public static void ShowToast(string message)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                LogService.Info("Android: ShowToast(" + message + ")");
                new AndroidToast(message);
#elif UNITY_IOS && !UNITY_EDITOR
#else
                Debug.Log("[ToastService] Unknown device: ShowToast(" + message + ")");
#endif
            }
        }

        public class AndroidToast
        {
            private AndroidJavaObject currentActivity;
            private string message;
            private string duration;

            public AndroidToast(string message, string duration = "LENGTH_SHORT")
            {
                if (!String.IsNullOrEmpty(message))
                {
                    AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    this.currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    this.currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(ShowToastRunnable));
                    this.duration = duration;
                    this.message = message;
                }
                else
                {
                    Debug.LogWarning("[AndroidToast] Message is empty");
                }
            }

            private void ShowToastRunnable()
            {
                AndroidJavaObject context = this.currentActivity.Call<AndroidJavaObject>("getApplicationContext");
                AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
                AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", this.message);
                AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>(this.duration));

                toast.Call("show");
            }
        }
    }
}
