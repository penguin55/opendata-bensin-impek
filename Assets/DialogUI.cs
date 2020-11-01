using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TomWill;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI;
    [SerializeField] [TextArea] private string[] chat;
    [SerializeField] [TextArea] private string[] charname;
    [SerializeField] private Text dialog, chara;
    [SerializeField] private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        TWLoading.OnSuccessLoad(() => {
            TWTransition.FadeOut();
        });
    }

    // Update is called once per frame
    void Update()
    {
        Dialog();
    }

    public void next()
    {
        index++;
    }

    public void Dialog()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogUI.SetActive(false);
            Time.timeScale = 1f;
        }
        if (index < chat.Length)
        {
            dialog.text = chat[index];
            chara.text = charname[index];
        }
            
        if (index >= chat.Length)
        {
            dialogUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
