using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using SpeechLib;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using System;

namespace OpenAI
{
    public class DallE : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private GameObject loadingLabel;

        private OpenAIApi openai = new OpenAIApi();
        SpVoice voice = new();
        public ConfidenceLevel confidence = ConfidenceLevel.Medium;
        protected PhraseRecognizer recognizer;
        protected string word = " ";
        private DictationRecognizer dictationRecognizer;

        private void Start()
        {
            image.gameObject.SetActive(false);
            loadingLabel.SetActive(false);
            button.onClick.AddListener(SendImageRequest);
            StartKeywordRecognition();
        }


        private async void SendImageRequest()
        {
            image.sprite = null;
            button.enabled = false;
            inputField.enabled = false;
            loadingLabel.SetActive(true);
            
            var response = await openai.CreateImage(new CreateImageRequest
            {
                Prompt = inputField.text,
                Size = ImageSize.Size512
            });

            if (response.Data != null && response.Data.Count > 0)
            {
                using(var request = new UnityWebRequest(response.Data[0].Url))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Access-Control-Allow-Origin", "*");
                    request.SendWebRequest();

                    while (!request.isDone) await Task.Yield();

                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(request.downloadHandler.data);
                    var sprite = Sprite.Create(texture, new Rect(0, 0, 512, 512), Vector2.zero, 1f);
                    image.sprite = sprite;
                }
            }
            else
            {
                Debug.LogWarning("No image was created from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
            loadingLabel.SetActive(false);
            image.gameObject.SetActive(true);
        }
        
        private void StartKeywordRecognition()
        {
            string[] keywords = new string[] { "quick image generation" };

            if (keywords != null)
            {
                recognizer = new KeywordRecognizer(keywords, confidence);
                recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
                recognizer.Start();
                Debug.Log("Keyword recognition is running: " + recognizer.IsRunning);
            }
        }
   
        private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            word = args.text;
            Debug.Log("Keyword recognized: " + word);

            if (word.ToLower() == "quick image generation")
            {
                SceneManager.LoadScene("2Scene");
            }
        }

        }


    }
