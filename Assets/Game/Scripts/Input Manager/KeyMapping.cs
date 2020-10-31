using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeyMapping : MonoBehaviour
{
    Transform menuKeymapping;
    Event keyEvent;
    Text buttonText;
    KeyCode newKey;

    bool waitingForKey;

    void Start()
    {
        menuKeymapping = transform.Find("KeyMapping");
        //menuKeymapping.gameObject.SetActive(false);
        waitingForKey = false;

        for (int i = 0; i < menuKeymapping.childCount; i++)
        {
            if (menuKeymapping.GetChild(i).name == "UpKey")
                menuKeymapping.GetChild(i).GetComponentInChildren<Text>().text = InputManager.instance.moveUp.ToString();
            else if (menuKeymapping.GetChild(i).name == "LeftKey")
                menuKeymapping.GetChild(i).GetComponentInChildren<Text>().text = InputManager.instance.moveLeft.ToString();
            else if (menuKeymapping.GetChild(i).name == "DownKey")
                menuKeymapping.GetChild(i).GetComponentInChildren<Text>().text = InputManager.instance.moveDown.ToString();
            else if (menuKeymapping.GetChild(i).name == "RightKey")
                menuKeymapping.GetChild(i).GetComponentInChildren<Text>().text = InputManager.instance.moveRight.ToString();
            else if (menuKeymapping.GetChild(i).name == "DashKey")
                menuKeymapping.GetChild(i).GetComponentInChildren<Text>().text = InputManager.instance.dash.ToString();
        }
    }

    void OnGUI()
    {
        keyEvent = Event.current;

        //Executes if a button gets pressed and
        //the user presses a key
        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode; //Assigns newKey to the key user presses
            waitingForKey = false;
        }
    }

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }

    public void SendText(Text text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;

        yield return WaitForKey(); //Executes endlessly until user presses a key

        switch (keyName)
        {
            case "up":
                InputManager.instance.moveUp = newKey; //Set move up to new keycode
                buttonText.text = InputManager.instance.moveUp.ToString(); //Set button text to new key
                PlayerPrefs.SetString("upKey", InputManager.instance.moveUp.ToString()); //save new key to PlayerPrefs
                break;
            case "left":
                InputManager.instance.moveUp = newKey; //Set move up to new keycode
                buttonText.text = InputManager.instance.moveUp.ToString(); //Set button text to new key
                PlayerPrefs.SetString("leftKey", InputManager.instance.moveUp.ToString()); //save new key to PlayerPrefs
                break;
            case "down":
                InputManager.instance.moveUp = newKey; //Set move up to new keycode
                buttonText.text = InputManager.instance.moveUp.ToString(); //Set button text to new key
                PlayerPrefs.SetString("downKey", InputManager.instance.moveUp.ToString()); //save new key to PlayerPrefs
                break;
            case "right":
                InputManager.instance.moveUp = newKey; //Set move up to new keycode
                buttonText.text = InputManager.instance.moveUp.ToString(); //Set button text to new key
                PlayerPrefs.SetString("rightKey", InputManager.instance.moveUp.ToString()); //save new key to PlayerPrefs
                break;
            case "dash":
                InputManager.instance.moveUp = newKey; //Set move up to new keycode
                buttonText.text = InputManager.instance.moveUp.ToString(); //Set button text to new key
                PlayerPrefs.SetString("dashKey", InputManager.instance.moveUp.ToString()); //save new key to PlayerPrefs
                break;
        }

        yield return null;
    }
}
