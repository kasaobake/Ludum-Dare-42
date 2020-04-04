using System.Collections;
using Kameosa.Common;
using Kameosa.Components;
using Kameosa.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kameosa
{
    namespace Managers
    {
        [RequireComponent(typeof(AudioLibraryComponent))]
        public class AudioManager : Singleton<AudioManager>
        {
            private const int AUDIO_SOURCE_COUNT = 2;
            private const string MASTER_VOLUME = "MASTER_VOLUME";
            private const string EFFECT_VOLUME = "EFFECT_VOLUME";
            private const string MUSIC_VOLUME = "MUSIC_VOLUME";

            #region Inspector Variables
            [SerializeField]
            [Range(0f, 1f)]
            private float masterVolume = 1f;
            [SerializeField]
            [Range(0f, 1f)]
            private float effectVolume = 1f;
            [SerializeField]
            [Range(0f, 1f)]
            private float musicVolume = 1f;
            #endregion

            #region Private Variables
            private AudioSource audioSource2D;
            private AudioSource[] audioSources;
            private int currentAudioSourceIndex;
            private AudioLibraryComponent audioLibraryComponent;
            #endregion

            #region Properties
            public float MasterVolume
            {
                get
                {
                    return this.masterVolume;
                }

                set
                {
                    this.masterVolume = value;
                    CurrentAudioSource.volume = value;
                    PreviousAudioSource.volume = value;
                    PlayerPrefsService.SetFloat(MASTER_VOLUME, value);
                }
            }

            public float EffectVolume
            {
                get
                {
                    return this.effectVolume;
                }

                set
                {
                    this.effectVolume = value;
                    PlayerPrefsService.SetFloat(EFFECT_VOLUME, value);
                }
            }

            public float MusicVolume
            {
                get
                {
                    return this.musicVolume;
                }

                set
                {
                    this.musicVolume = value;
                    CurrentAudioSource.volume = this.musicVolume * this.masterVolume;
                    PreviousAudioSource.volume = this.musicVolume * this.masterVolume;
                    PlayerPrefsService.SetFloat(MUSIC_VOLUME, value);
                }
            }

            public AudioSource CurrentAudioSource
            {
                get
                {
                    return this.audioSources[this.currentAudioSourceIndex];
                }
            }

            public AudioSource PreviousAudioSource
            {
                get
                {
                    return this.audioSources[1 - this.currentAudioSourceIndex];
                }
            }
            #endregion

            #region MonoBehaviour Functions
            protected override void Awake()
            {
                base.Awake();

                this.audioLibraryComponent = GetComponent<AudioLibraryComponent>();

                this.audioSource2D = new GameObject("AudioSource2D").AddComponent<AudioSource>();
                this.audioSources = new AudioSource[2];

                for (int i = 0; i < AUDIO_SOURCE_COUNT; i++)
                {
                    GameObject newAudioSouce = new GameObject("AudioSource" + (i + 1));
                    this.audioSources[i] = newAudioSouce.AddComponent<AudioSource>();
                    this.audioSources[i].loop = true;
                    newAudioSouce.transform.parent = transform;
                }

                this.masterVolume = PlayerPrefsService.GetFloat(MASTER_VOLUME);
                this.effectVolume = PlayerPrefsService.GetFloat(EFFECT_VOLUME);
                this.musicVolume = PlayerPrefsService.GetFloat(MUSIC_VOLUME);
            }

            private void Start()
            {
            }
            #endregion

            #region Public Functions
            public void PlaySound(AudioClip audioClip, UnityEngine.Vector3 position)
            {
                if (audioClip != null)
                {
                    AudioSource.PlayClipAtPoint(audioClip, position, this.effectVolume * this.masterVolume);
                }
            }

            public void PlaySound(string name, UnityEngine.Vector3 position)
            {
                PlaySound(this.audioLibraryComponent.GetRandomAudioClipFromName(name), position);
            }

            public void PlaySound(AudioClip audioClip)
            {
                this.audioSource2D.PlayOneShot(audioClip, this.effectVolume * this.masterVolume);
            }

            public void PlaySound(string name)
            {
                PlaySound(this.audioLibraryComponent.GetRandomAudioClipFromName(name));
            }

            public void PlayMusic(AudioClip audioClip, float fadeDuration = 1f)
            {
                this.currentAudioSourceIndex = 1 - this.currentAudioSourceIndex;
                Debug.Log(this.currentAudioSourceIndex + " going to play " + audioClip.name);
                CurrentAudioSource.clip = audioClip;
                CurrentAudioSource.Play();

                StartCoroutine(AnimateMusicCrossfade(fadeDuration));
            }

            public void PlayMusic(string name, float fadeDuration = 1f)
            {
                AudioClip audioClip = this.audioLibraryComponent.GetRandomAudioClipFromName(name);
                PlayMusic(audioClip, fadeDuration);
            }
            #endregion

            #region Private Functions
            //private void PlayOnce()
            //{
            //    if (!this.audioSource.isPlaying)
            //    {
            //        this.audioSource.Play();
            //    }
            //}

            //private void Stop()
            //{
            //    if (this.audioSource.isPlaying)
            //    {
            //        this.audioSource.Stop();
            //    }
            //}

            //private void Mute()
            //{
            //    this.audioSource.mute = true;
            //}

            //private void Unmute()
            //{
            //    this.audioSource.mute = false;
            //}

            private IEnumerator AnimateMusicCrossfade(float duration)
            {
                float percent = 0f;

                while (percent < 1f)
                {
                    percent += (Time.deltaTime / duration);
                    CurrentAudioSource.volume = Mathf.Lerp(0f, this.musicVolume * this.masterVolume, percent);
                    PreviousAudioSource.volume = Mathf.Lerp(this.musicVolume * this.masterVolume, 0f, percent);

                    yield return null;
                }

                PreviousAudioSource.Stop();
            }
            #endregion
        }
    }
}