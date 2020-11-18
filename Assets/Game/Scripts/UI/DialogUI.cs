using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TomWill;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI, selectBoss, selectItem;
    [SerializeField] [TextArea(0,30)] private string[] chat;
    [SerializeField] [TextArea(0, 30)] private string[] charname;
    [SerializeField] [TextArea(0, 30)] private string[] bossDesc;
    [SerializeField] [TextArea(0, 30)] private string[] itemDesc;
    [SerializeField] private Text dialog, chara, boss, item;
    [SerializeField] private int index = 0, bossIndex,itemIndex;
    [SerializeField] private GameObject x, commander, heli, mysterious;


    // Start is called before the first frame update
    void Start()
    {
        TWLoading.OnSuccessLoad(() => {
            TWTransition.FadeOut();
        });
        bossIndex = 4;
        itemIndex = 5;
        Dialog();
    }


    public void ChangeImage()
    {
        if (charname[index].Contains("X "))
        {
            x.SetActive(true);
            commander.SetActive(false);
            heli.SetActive(false);
            mysterious.SetActive(false);
        }
        if (charname[index].Contains("Colonel"))
        {
            x.SetActive(false);
            commander.SetActive(true);
            heli.SetActive(false);
            mysterious.SetActive(false);
        }
    }

    public void button1()
    {
        TWAudioController.PlaySFX("UI", "click");
        if (index ==12) bossIndex = 0;
        if (index == 15) itemIndex = 0;
        BossDesc();
        ItemDesc();
    }

    public void button2()
    {
        TWAudioController.PlaySFX("UI", "click");
        if (index == 12) bossIndex = 1;
        if (index == 15) itemIndex = 1;
        BossDesc();
        ItemDesc();
    }
    public void button3()
    {
        TWAudioController.PlaySFX("UI", "click");
        if (index == 12) bossIndex = 2;
        if (index == 15) itemIndex = 2;
        BossDesc();
        ItemDesc();
    }
    public void button4()
    {
        TWAudioController.PlaySFX("UI", "click");
        if (index == 12) bossIndex = 3;
        if (index == 15) itemIndex = 3;
        BossDesc();
        ItemDesc();
    }

    public void button5()
    {
        TWAudioController.PlaySFX("UI", "click");
        if (index == 12) bossIndex = 4;
        if (index == 15) itemIndex = 4;
        ItemDesc();
    }

    public void ItemDesc()
    {
        item.text = itemDesc[itemIndex];
    }
    public void BossDesc()
    {
        boss.text = bossDesc[bossIndex];
    }
    public void next()
    {
        TWAudioController.PlaySFX("UI", "click");
        index++;
        Dialog();
    }

    public void SelectBoss()
    {
        TWAudioController.PlaySFX("UI", "click");
        index++;
        selectBoss.SetActive(false);
        dialogUI.SetActive(true);
    }

    public void SelectItem()
    {
        TWAudioController.PlaySFX("UI", "click");
        index++;
        selectItem.SetActive(false);
        dialogUI.SetActive(true);
    }

    public void Dialog()
    {
        if (index < chat.Length)
        {
            dialog.text = chat[index];
            chara.text = charname[index];
            ChangeImage();
        }

        if (index >= 12 && index < 13)
        {
            dialogUI.SetActive(false);
            selectBoss.SetActive(true);
            
        }
        if (index >= 15 && index < 16)
        {
            dialogUI.SetActive(false);
            selectItem.SetActive(true);
        }
        if (index >= chat.Length)
        {
            index = 0;
            dialogUI.SetActive(false);
            commander.SetActive(false);
            x.SetActive(false);
            heli.SetActive(false);
            StartCoroutine(End());
        }
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
            TWTransition.FadeIn(() => TWLoading.LoadScene("BossTest"));
            TWAudioController.PlaySFX("UI", "transition");
        }
    }
}
