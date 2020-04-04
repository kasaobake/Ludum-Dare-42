using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kameosa
{
    namespace Common
    {
        /// <summary>
        /// Singleton class
        /// </summary>
        /// <typeparam name="T">Type of the singleton</typeparam>
        public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
        {
            private static T instance;

            #region Properties 
            /// <summary>
            /// The static reference to the instance
            /// </summary>
            public static T Instance
            {
                get
                {
#if UNITY_EDITOR
                    if (Singleton<T>.instance == null)
                    {
                        GameObject gameObject = new GameObject(typeof(T).Name);
                        gameObject.AddComponent<T>();
                    }
#endif

                    return Singleton<T>.instance;
                }
                protected set
                {
                    Singleton<T>.instance = value;
                }
            }

            /// <summary>
            /// Gets whether an instance of this singleton exists
            /// </summary>
            public static bool IsInstanceExists
            {
                get
                {
                    return Singleton<T>.instance != null;
                }
            }
            #endregion

            #region Actions
            public static event Action OnInstanceSet;
            #endregion

            #region MonoBehaviour Functions
            /// <summary>
            /// Awake method to associate singleton with instance
            /// </summary>
            protected virtual void Awake()
            {
                if (Singleton<T>.instance != null)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Singleton<T>.instance = (T)this;

                    if (OnInstanceSet != null)
                    {
                        OnInstanceSet();
                    }
                }

                DontDestroyOnLoad(this.gameObject);
            }

            /// <summary>
            /// OnDestroy method to clear singleton association
            /// </summary>
            protected virtual void OnDestroy()
            {
                if (Singleton<T>.instance == this)
                {
                    Singleton<T>.instance = null;
                }
            }
            #endregion
        }
    }
}
