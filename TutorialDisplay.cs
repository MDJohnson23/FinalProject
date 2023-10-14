using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisplay : MonoBehaviour
{
    public string displayMessage;
    public Font myFont;

    string clearDisplayMessage = "";

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            clearDisplayMessage = displayMessage;
        }
    }
    void OnTriggerExit(Collider other)
    {
        clearDisplayMessage = "";
    }
    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 50;
        myStyle.font = myFont;
        myStyle.wordWrap = true;
        float displayMessageLocation = Screen.height / 2;
        GUI.Label(new Rect(25, displayMessageLocation, 600, 50), clearDisplayMessage, myStyle);
    }
}
