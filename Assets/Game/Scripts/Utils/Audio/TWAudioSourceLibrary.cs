/* TWAudio version 3.0
 * Update Date : 21/08/2020
 * Created by TomWill
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TomWill
{
    public class TWAudioSourceLibrary : MonoBehaviour
    {
        List<TWAudioSource> audioSources;

        public void Init()
        {
            audioSources = new List<TWAudioSource>();
        }

        public AudioSource GetAudio(string audioSourceName)
        {
            if (audioSources != null)
            {
                if (audioSources.Any(e => e.audioSourceName == audioSourceName))
                {
                    return audioSources.First(e => e.audioSourceName == audioSourceName).audio;
                } else
                {
                    return null;
                }
            }
            else Init();

            return null;
        }

        public void SetAudio(string audioSourceName, AudioSource source)
        {
            if (!audioSources.Any(e => e.audioSourceName == audioSourceName) && source != null)
            {
                if (audioSources != null)
                {
                    audioSources.Add(new TWAudioSource(audioSourceName, source));
                }
                else Init();
            }
        }
    }

    public class TWAudioSource
    {
        public string audioSourceName;
        public AudioSource audio;

        public TWAudioSource(string audioSourceName, AudioSource audio)
        {
            this.audioSourceName = audioSourceName;
            this.audio = audio;
        }
    }
}