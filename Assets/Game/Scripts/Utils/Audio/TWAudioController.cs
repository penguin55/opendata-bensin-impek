/* TWAudio version 2.0
 * Update Date : 21/08/2020
 * Created by TomWill
 */
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TomWill
{
    public class TWAudioController : MonoBehaviour
    {
        public enum PlayType
        {
            AUTO,
            OVERRIDE_PLAY,
            DEFAULT
        }

        private static TWAudioController instance;
        private TWAudioLibrary audioLibrary;
        private TWAudioQueue bgmQueue;
        private Coroutine[] coroutines;
        private AudioSource sfxSource;
        private float timeFade;

        #region Setting AudioController
        public static TWAudioController Instance
        {
            get
            {
                if (instance == null)
                {
                    // Create a variable AudioController and call CreateInstance method directly from that class
                    Debug.Log("Please call CreateInstance method before using this Controller");
                }

                return instance;
            }
        }

        public void CreateInstance()
        {
            createInstance();
        }

        public static void SetUpController([NotNull] TWAudioLibrary library)
        {
            // Only support 2 channel for fading
            Instance?.setUpController(library, 2);
            Instance?.SetTimeFade();
        }

        public void SetTimeFade(float time = 2)
        {
            Instance?.setTimeFade(time);
        }
        #endregion

        #region Public Function
        public static void PlayBGM(string musicName, PlayType playType, bool loop = true)
        {
            Debug.Log(musicName);
            Instance?.playBGM(musicName, playType, loop);
        }

        public static void StopBGMPlayed(bool immediatly = true)
        {
            Instance?.stopBGMPlayed(immediatly);
        }

        public static void StopSpecificBGM(string name, bool immediatly = true)
        {
            Instance?.stopSpecificBGM(name, immediatly);
        }

        public static void PlaySFX(string musicName)
        {
            Instance.playSFX(musicName);
        }
        #endregion

        #region Internal Function
        private void createInstance()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void playBGM(string musicName, PlayType playType, bool loop)
        {
            switch (playType)
            {
                case PlayType.AUTO:
                    checkBusyChannel();
                    playBGMAuto(musicName, loop);
                    break;
                case PlayType.OVERRIDE_PLAY:
                    playBGMOverride(musicName, loop);
                    break;
                case PlayType.DEFAULT:
                    playBGMDefault(musicName, loop);
                    break;
            }
        }

        private void playSFX(string musicName)
        {
            sfxSource.PlayOneShot(audioLibrary.GetSFXClip(musicName));
        }

        private void playBGMAuto(string musicName, bool loop)
        {
            TWAudioQueue.SourceData activeSource = bgmQueue.ChangeActiveChannel();
            TWAudioQueue.SourceData lastSource = bgmQueue.GetLastActive();

            if (lastSource.audio.isPlaying)
            {
                coroutines[lastSource.ID] = StartCoroutine(fadeAudio(lastSource.audio, false, () => { coroutines[lastSource.ID] = null; }));
            }

            if (!activeSource.audio.isPlaying && activeSource.audio.clip == null)
            {
                activeSource.audio.clip = audioLibrary.GetBGMClip(musicName);
                activeSource.audio.loop = loop;
                activeSource.audio.volume = 0;
                activeSource.audio.Play();

                coroutines[activeSource.ID] = StartCoroutine(fadeAudio(activeSource.audio, true, ()=> { coroutines[activeSource.ID] = null; }));
            }
        }

        private void playBGMDefault(string musicName, bool loop)
        {
            TWAudioQueue.SourceData activeSource = bgmQueue.ChangeActiveChannel();
            if (!activeSource.audio.isPlaying && activeSource.audio.clip == null)
            {
                activeSource.audio.clip = audioLibrary.GetBGMClip(musicName);
                activeSource.audio.loop = loop;
                activeSource.audio.volume = 1;
                activeSource.audio.Play();

                return;
            }
        }

        private void playBGMOverride(string musicName, bool loop)
        {
            TWAudioQueue.SourceData activeSource = bgmQueue.ChangeActiveChannel();
            if (!activeSource.audio.isPlaying && activeSource.audio.clip == null)
            {
                activeSource.audio.clip = audioLibrary.GetBGMClip(musicName);
                activeSource.audio.loop = loop;
                activeSource.audio.volume = 0;
                activeSource.audio.Play();

                StartCoroutine(fadeAudio(activeSource.audio, true));

                return;
            }
            
        }

        private void stopBGMPlayed(bool immediatly)
        {
            List<TWAudioQueue.SourceData> audioSources = bgmQueue.GetAllSource();

            for (int i = 0; i < audioSources.Count; i++)
            {
                if (immediatly)
                {
                    audioSources[i].audio.volume = 0;
                    audioSources[i].audio.Stop();
                    audioSources[i].audio.clip = null;
                }
                else
                {
                    coroutines[audioSources[i].ID] = StartCoroutine(fadeAudio(audioSources[i].audio, false, () => { coroutines[audioSources[i].ID] = null; }));
                }
            }
        }

        private void stopSpecificBGM(string name, bool immediatly)
        {
            List<TWAudioQueue.SourceData> audioSources = bgmQueue.GetAllSource();
            AudioClip playedClip = audioLibrary.GetBGMClip(name);

            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i].audio.clip == playedClip)
                {
                    if (immediatly)
                    {
                        audioSources[i].audio.volume = 0;
                        audioSources[i].audio.Stop();
                        audioSources[i].audio.clip = null;
                    }
                    else
                    {
                        coroutines[audioSources[i].ID] = StartCoroutine(fadeAudio(audioSources[i].audio, false, () => { coroutines[audioSources[i].ID] = null; }));
                    }
                    return;
                }
            }
        }


        private void checkBusyChannel()
        {
            if (!availableChannel())
            {
                forceStopBGM();
            }
        }

        private bool availableChannel()
        {
            List<TWAudioQueue.SourceData> audioSources = bgmQueue.GetAllSource();

            foreach (TWAudioQueue.SourceData source in audioSources)
            {
                if (!source.audio.isPlaying) return true;
            }

            return false;
        }

        private void forceStopBGM()
        {
            TWAudioQueue.SourceData sourceLast = bgmQueue.GetLastActive();

            if (coroutines[sourceLast.ID] != null) StopCoroutine(coroutines[sourceLast.ID]);
            sourceLast.audio.volume = 0;
            sourceLast.audio.clip = null;
            sourceLast.audio.Stop();
            coroutines[sourceLast.ID] = null;
        }

        private void setUpController(TWAudioLibrary library, int bgmChannel)
        {
            instance.bgmQueue = new TWAudioQueue();
            instance.bgmQueue.Init();
            instance.coroutines = new Coroutine[bgmChannel];

            for (int i = 0; i < bgmChannel; i++)
            {
                AudioSource source = instance.gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                instance.bgmQueue.AddSourceQueue(source);
            }

            instance.sfxSource = instance.gameObject.AddComponent<AudioSource>();
            instance.sfxSource.playOnAwake = false;
            instance.sfxSource.loop = false;

            instance.audioLibrary = library;
        }

        private void setTimeFade(float timeFade)
        {
            this.timeFade = timeFade;
        }

        private IEnumerator fadeAudio(AudioSource source, bool fadeIn, UnityAction callback = null)
        {
            float timeElapse = timeFade;

            while (timeElapse > 0)
            {
                source.volume = fadeIn ? (1f - timeElapse / timeFade) : (timeElapse / timeFade);
                yield return null;
                timeElapse -= Time.deltaTime;
            }

            source.volume = fadeIn ? 1f : 0f;

            if (!fadeIn)
            {
                source.Stop();
                source.clip = null;
            }

            if (callback != null)
            {
                callback.Invoke();
            }
        }
        #endregion
    }
}