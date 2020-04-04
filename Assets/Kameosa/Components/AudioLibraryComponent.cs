using System.Collections;
using System.Collections.Generic;
using Kameosa.Collections.IList;
using UnityEngine;

namespace Kameosa
{
    namespace Components
    {
        public class AudioLibraryComponent : MonoBehaviour
        {
            [System.Serializable]
            public class AudioGroup
            {
                #region Inspector Variables
                [SerializeField]
                private string name;
                [SerializeField]
                private AudioClip[] audioClips;
                #endregion

                #region Properties
                public string Name
                {
                    get
                    {
                        return this.name;
                    }

                    set
                    {
                        this.name = value;
                    }
                }

                public AudioClip[] AudioClips
                {
                    get
                    {
                        return this.audioClips;
                    }

                    set
                    {
                        this.audioClips = value;
                    }
                }
                #endregion
            }

            #region Inspector Variables
            [SerializeField]
            private AudioGroup[] audioGroups;
            #endregion

            #region Private Variables
            private Dictionary<string, AudioClip[]> audioGroupDictionary = new Dictionary<string, AudioClip[]>();
            #endregion

            #region MonoBehaviour Functions
            private void Awake()
            {
                if (this.audioGroups != null)
                {
                    foreach (AudioGroup audioGroup in this.audioGroups)
                    {
                        this.audioGroupDictionary.Add(audioGroup.Name, audioGroup.AudioClips);
                    }
                }
            }
            #endregion

            #region Public Functions
            public AudioClip GetRandomAudioClipFromName(string name)
            {
                if (this.audioGroupDictionary.ContainsKey(name))
                {
                    return this.audioGroupDictionary[name].GetRandom();
                }

                return null;
            }
            #endregion
        }
    }
}
