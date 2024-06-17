using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Name_Input : MonoBehaviour
{
    public Button buttonClickMale;
    public Button buttonClickFemale;

    public InputField inputField;
    FileInfo name_check = new FileInfo("name_check.txt");

    public void Start()
    {
        Hide();
        if (name_check.Length == 0)
        {
            Show();
            buttonClickMale.onClick.AddListener(GetInputFromClickHandlerMale);
            buttonClickFemale.onClick.AddListener(GetInputFromClickHandlerFemale);
        }
    }

    public void StartProcess()
    {
        
    }

    public void GetInputFromClickHandlerMale()
    {
        System.IO.File.WriteAllText("name_check.txt", "Mister " + inputField.text);
        Hide();
    }

    public void GetInputFromClickHandlerFemale()
    {
        System.IO.File.WriteAllText("name_check.txt", "Missis " + inputField.text);
        Hide();
    }

    public void Show()
    {
        inputField.gameObject.SetActive(true);
        buttonClickMale.gameObject.SetActive(true);
        buttonClickFemale.gameObject.SetActive(true);
    }

    public void Hide()
    {
        inputField.gameObject.SetActive(false);
        buttonClickFemale.gameObject.SetActive(false);
        buttonClickMale.gameObject.SetActive(false);
    }
}
