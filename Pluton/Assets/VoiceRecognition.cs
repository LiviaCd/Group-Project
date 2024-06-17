using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using SpeechLib;
using System;
using OpenAI;
using System.IO;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class VoiceRecognition : MonoBehaviour
{
    SpVoice voice = new();
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    protected PhraseRecognizer recognizer;
    protected string word = " ";
    private DictationRecognizer dictationRecognizer;
    Boolean wake = false;
    private bool shouldRespond = false;
    [SerializeField] private string url;
    //private ChatGPT chatGPT;
    private FileInfo name_check = new FileInfo("name_check.txt");
    private string name = File.ReadAllText(@"C:\\Users\\Professional\\Pluton\\name_check.txt");


    private void Start()
    {
        if (name_check.Length == 0)
        {
            //wake = false;
            voice.Speak("Hello! I'm Pluto, your personal assistant with voice recognition");
            voice.Speak("First, write your name and pick your gender");
            // Start keyword recognition
            StartKeywordRecognition();
            // Start dictation recognition
            StartDictationRecognition();
        }
        else
        {
            voice.Speak("Welcome back" + name);
            // Start keyword recognition
            StartKeywordRecognition();
            // Start dictation recognition
            StartDictationRecognition();
        }
        Name_Input nameInput = gameObject.GetComponent<Name_Input>();
        nameInput.StartProcess();

        //chatGPT = FindObjectOfType<ChatGPT>();
    }
    public void OpenUrl()
    {
        Application.OpenURL(url);
    }

    private void StartKeywordRecognition()
    {
        string[] keywords = new string[] { "hello", "hey pluto", "hi", "wake", "sleep", "open youtube", "open google", "open translate", "open else", "open gmail", "open outlook", "open message area", "open image generation" };

        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
            Debug.Log("Keyword recognition is running: " + recognizer.IsRunning);
        }
    }
    public Boolean IsSpeaking()
    {
        return wake;
    }
    public void StartDictationRecognition()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
        dictationRecognizer.Start();
        Debug.Log("Dictation recognition is running: " + dictationRecognizer.Status);
    }
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        word = text;
        Debug.Log("Dictation result: " + word);
    }


    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.LogError("Dictation error: " + error);
    }


    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        Debug.Log("Keyword recognized: " + word);

        if (word.ToLower() == "hey pluto")
        {
            voice.Speak("I am here");
            wake = true;
        }

        if (wake)
        {
            switch (word.ToLower())
            {
                case "hello":
                    voice.Speak("Hello");
                    wake = false;
                    break;
                case "sleep":
                    voice.Speak("Bye" + name);
                    wake = false;
                    break;
                case "open youtube":
                    voice.Speak("Ok");
                    url = "https://www.youtube.com/";
                    OpenUrl();
                    wake = false;
                    break;
                case "open google":
                    voice.Speak("Ok");
                    url = "https://www.google.com/";
                    OpenUrl();
                    wake = false;
                    break;
                case "open translate":
                    voice.Speak("Ok");
                    url = "https://translate.google.com/?hl=ru&tab=TT";
                    OpenUrl();
                    wake = false;
                    break;
                case "open else":
                    voice.Speak("Ok");
                    url = "https://else.fcim.utm.md/";
                    OpenUrl();
                    wake = false;
                    break;
                case "open gmail":
                    voice.Speak("Ok");
                    url = "https://mail.google.com/";
                    OpenUrl();
                    wake = false;
                    break;
                case "open outlook":
                    voice.Speak("Ok");
                    url = "https://www.microsoft.com/ru-ru/microsoft-365/outlook/email-and-calendar-software-microsoft-outlook";
                    OpenUrl();
                    wake = false;
                    break;
                case "ask pluto":
                    voice.Speak("What do you want to ask?");
                    break;
                case "open paint":
                    System.Diagnostics.Process.Start("paint.exe");
                    break;
                case "open to do list":
                    System.Diagnostics.Process.Start("to_do_list.txt"); 
                    break;
                case "what time is it?":
                    voice.Speak(DateTime.Now.ToString("HH:mm")); 
                    break;
                case "what's the date?":
                    voice.Speak(DateTime.Now.ToString("dd:MM")); 
                    break;
                case "what day of the week is it?":
                    voice.Speak(DateTime.Now.ToString("ddd")); 
                    break;
                case "open message area":
                    SceneManager.LoadScene("ChatGPT Sample");
                    break;
                case "quick message area":
                    SceneManager.LoadScene("2Scene");
                    break;
                case "open image generation":
                    SceneManager.LoadScene("DallE Sample");
                    break;
                case "quick image generation":
                    SceneManager.LoadScene("2Scene");
                    break;
            }
        }
    }

    void Update()
    {
        // Reset the flag after responding
        if (shouldRespond)
        {
            voice.Speak("Hello, my name is Pluto");
            shouldRespond = false;
        }
    }

    private void OnDestroy()
    {
        // Stop the recognizers when the script is destroyed
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.Stop();
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
        }

        if (dictationRecognizer != null && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
            dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
            dictationRecognizer.DictationError -= DictationRecognizer_DictationError;
        }
    }
}
