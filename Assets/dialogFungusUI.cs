using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TomWill;

public class dialogFungusUI : MonoBehaviour
{
    [SerializeField] private GameObject selectBoss, selectItem;
    [SerializeField] [TextArea(0, 30)] private string[] bossDesc;
    [SerializeField] [TextArea(0, 30)] private string[] itemDesc;
    [SerializeField] private Text  boss, item;
    [SerializeField] private int index = 0, bossIndex, itemIndex;
    [SerializeField] private GameObject x, commander, heli, mysterious;


    // Start is called before the first frame update
    void Start()
    {
        TWLoading.OnSuccessLoad(() => {
            TWTransition.FadeOut();
        });
        bossIndex = 4;
        itemIndex = 5;
        TWAudioController.PlayBGM("BGM", "MainMenu", TWAudioController.PlayType.TRANSITION);
    }


   

    public void SelectBoss()
    {
        TWAudioController.PlaySFX("UI", "click");
        index++;
    }

    public void SelectItem()
    {
        TWAudioController.PlaySFX("UI", "click");
        index++;
    }

    IEnumerator End()
    {
        TimelineManager.instance.Director.Play();
        Debug.Log((float)TimelineManager.instance.Director.duration);
        yield return new WaitForSeconds((float)TimelineManager.instance.Director.duration);
        Debug.Log((float)TimelineManager.instance.Director.duration);
        Debug.Log(TimelineManager.instance.Director.state);
        TWTransition.FadeIn(() => TWLoading.LoadScene("BossTest"));
    }

    public void EndTimeline()
    {
        if (TimelineManager.instance.Director.state != UnityEngine.Playables.PlayState.Playing)
        {
            Debug.Log("Hi");
            TWTransition.FadeIn(() => TWLoading.LoadScene("BossTest"));
            TWAudioController.PlaySFX("UI", "transition");
        }
    }
}
