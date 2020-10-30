/* TWAudio version 2.0
 * Update Date : 21/08/2020
 * Created by TomWill
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TomWill
{
    public class TWAudioQueue
    {
        public struct SourceData
        {
            public SourceData(int ID, AudioSource audio)
            {
                this.ID = ID;
                this.audio = audio;
            }

            public int ID;
            public AudioSource audio;
        }
        private List<SourceData> activeQueue;

        #region Setting AudioQueue
        public void Init()
        {
            activeQueue = new List<SourceData>();
        }

        public void AddSourceQueue(AudioSource audio)
        {
            if (!ContainData(audio))
            {
                SourceData newSource = new SourceData(activeQueue.Count, audio);
                activeQueue.Add(newSource);
            }
        }
        #endregion

        #region Public Function
        public SourceData ChangeActiveChannel()
        {
            ShiftQueue();
            return activeQueue[0];
        }

        public SourceData GetCurrentActive()
        {
            return activeQueue[0];
        }

        public SourceData GetLastActive()
        {
            return activeQueue[1];
        }

        public List<SourceData> GetAllLastActive()
        {
            return activeQueue.Count > 1 ? activeQueue.GetRange(1, activeQueue.Count - 1) : activeQueue;
        }

        public List<SourceData> GetAllSource()
        {
            return activeQueue;
        }
        #endregion

        #region Internal Function
        private void ShiftQueue()
        {
            List<SourceData> newTemp = new List<SourceData>();

            newTemp.Insert(0, activeQueue[activeQueue.Count - 1]);
            newTemp.AddRange(activeQueue.GetRange(0, activeQueue.Count - 1));

            activeQueue = newTemp;
        }

        private bool ContainData(AudioSource source)
        {
            return activeQueue.Any(e => e.audio == source);
        }
        #endregion
    }
}
