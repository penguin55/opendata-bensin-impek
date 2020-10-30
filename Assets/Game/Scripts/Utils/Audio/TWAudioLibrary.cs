/* TWAudio version 2.0
 * Update Date : 21/08/2020
 * Created by TomWill
 */
using System.Linq;
using UnityEngine;

namespace TomWill
{
    public class TWAudioLibrary : MonoBehaviour
    {
        [System.Serializable]
        struct ClipDetail
        {
            public string name;
            public AudioClip clip;
        }

        [SerializeField] private ClipDetail[] bgmClips;
        [SerializeField] private ClipDetail[] sfxClips;

        public AudioClip GetBGMClip(string name)
        {
            if (bgmClips.Any(e => e.name == name))
            {
                return bgmClips.First(e => e.name == name).clip;
            }

            return null;
        }

        public AudioClip GetSFXClip(string name)
        {
            if (sfxClips.Any(e => e.name == name))
            {
                return sfxClips.First(e => e.name == name).clip;
            }

            return null;
        }
    }
}
