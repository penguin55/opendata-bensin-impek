using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TomWill;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI, selectBoss, selectItem;
    [SerializeField] [TextArea] private string[] chat;
    [SerializeField] [TextArea] private string[] charname;
    [SerializeField] [TextArea] private string[] bossDesc;
    [SerializeField] [TextArea] private string[] itemDesc;
    [SerializeField] private Text dialog, chara, boss, item;
    [SerializeField] private int index = 0, bossIndex,itemIndex;
    // Start is called before the first frame update
    void Start()
    {
        TWLoading.OnSuccessLoad(() => {
            TWTransition.FadeOut();
        });
        bossIndex = 4;
        itemIndex = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Dialog();
        BossDesc();
        ItemDesc();
    }

    public void button1()
    {
        if(index ==12) bossIndex = 0;
        if (index == 15) itemIndex = 0;
    }

    public void button2()
    {
        if (index == 12) bossIndex = 1;
        if (index == 15) itemIndex = 1;
    }
    public void button3()
    {
        if (index == 12) bossIndex = 2;
        if (index == 15) itemIndex = 2;
    }
    public void button4()
    {
        if (index == 12) bossIndex = 3;
        if (index == 15) itemIndex = 3;
    }

    public void button5()
    {
        if (index == 12) bossIndex = 4;
        if (index == 15) itemIndex = 4;
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
        index++;
    }

    public void SelectBoss()
    {
        index++;
        selectBoss.SetActive(false);
        dialogUI.SetActive(true);
    }

    public void SelectItem()
    {
        index++;
        selectItem.SetActive(false);
        dialogUI.SetActive(true);
    }

    public void Dialog()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (index < 12)
            {
                index = 12;
            }
            if (index >= 12 && index < 13)
            {
                dialogUI.SetActive(false);
                selectBoss.SetActive(true);
            }
            if(index == 13)
            {
                index = 15;
            }
            else if (index >= 15 && index < 16)
            {
                dialogUI.SetActive(false);
                selectItem.SetActive(true);
            }
        }
        if (index < chat.Length)
        {
            dialog.text = chat[index];
            chara.text = charname[index];
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
        if (index > chat.Length)
        {
            TWTransition.FadeIn(() => TWLoading.LoadScene("BossTest"));
        }
    }
}
