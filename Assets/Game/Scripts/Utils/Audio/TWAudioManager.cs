/* TWAudio version 2.0
 * Update Date : 21/08/2020
 * Created by TomWill
 */
using UnityEngine;

namespace TomWill
{
    [RequireComponent(typeof(TWAudioController), typeof(TWAudioLibrary))]
    public class TWAudioManager : MonoBehaviour
    {
        [SerializeField] private TWAudioController audioController;
        [SerializeField] private TWAudioLibrary audioLibrary;

        void Awake()
        {
            if (TWAudioController.Instance == null)
            {
                audioController = GetComponent<TWAudioController>();
                audioLibrary = GetComponent<TWAudioLibrary>();

                audioController.CreateInstance();
                TWAudioController.SetUpController(audioLibrary);
            }

            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}