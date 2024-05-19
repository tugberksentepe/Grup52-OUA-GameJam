


using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class api_Activate : MonoBehaviour
{
    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatOutputText;
    private string apiKey = "AIzaSyDr4774yt5SqYm3hQuGtBgBH6fYgUSXWuE"; // API anahtarýnýzý buraya ekleyin
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"; // Uç nokta URL'si

    private int questionCount = 0;

    private Queue<string> questions = new Queue<string>(new string[] {
        "Zaman yolcusu, ne zaman doðdun?",
        "Zaman yolcusu, en sevdiðin aný hatýrlýyor musun?",
        "Zaman yolcusu, en sevdiðin renk nedir?",
        "Zaman yolcusu, en büyük hayalin neydi?",
        "Zaman yolcusu, en büyük korkun neydi?"
    });

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        chatOutputText.text = "Merhaba! Benimle konuþmak için hazýr mýsýn? Lütfen bir soru sor ve geçmiþini keþfetmeme yardýmcý ol.";
    }

    public void OnSubmit()
    {
        string inputText = chatInputField.text;
        StartCoroutine(GetChatbotResponse(inputText));
    }

    private IEnumerator GetChatbotResponse(string inputText)
    {
        if (questionCount >= 5)
        {
            chatOutputText.text = "Üzgünüm, artýk baþka bir soru kabul etmiyorum.";
            yield break;
        }

        // API'ye JSON formatýnda veri gönderin
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + inputText + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                if (request.responseCode == 401)
                {
                    Debug.LogError("Unauthorized: Check your API key and permissions.");
                    chatOutputText.text = "Error: Unauthorized. Check your API key and permissions.";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Server Response: " + jsonResponse);

                    // JSON yanýtýný iþleyin ve çýktý metnini ayarlayýn
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    chatOutputText.text = response.candidates[0].content.parts[0].text; // Yanýt formatýna göre doðru alaný kullanýn

                    if (questionCount < 5)
                    {
                        questionCount++;
                        if (questionCount < 5)
                        {
                            chatOutputText.text += "\n" + questions.Dequeue();
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    private class ChatbotResponse
    {
        public Candidate[] candidates;
    }

    [System.Serializable]
    private class Candidate
    {
        public Content content;
    }

    [System.Serializable]
    private class Content
    {
        public Part[] parts;
    }

    [System.Serializable]
    private class Part
    {
        public string text;
    }
}









/*using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;



public class api_Activate : MonoBehaviour
{
    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatOutputText;
    private string apiKey = "AIzaSyDr4774yt5SqYm3hQuGtBgBH6fYgUSXWuE"; // API anahtarýnýzý buraya ekleyin
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"; // Uç nokta URL'si

    public void OnSubmit()
    {
        string inputText = chatInputField.text;
        StartCoroutine(GetChatbotResponse(inputText));
    }

    private IEnumerator GetChatbotResponse(string inputText)
    {
        // API'ye JSON formatýnda veri gönderin
        string jsonPayload = "{\"contents\":[{\"parts\":[{\"text\":\"" + inputText + "\"}]}]}";
        string fullUrl = apiUrl + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                chatOutputText.text = "Error: " + request.error;
            }
            else
            {
                if (request.responseCode == 401)
                {
                    Debug.LogError("Unauthorized: Check your API key and permissions.");
                    chatOutputText.text = "Error: Unauthorized. Check your API key and permissions.";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Server Response: " + jsonResponse);

                    // JSON yanýtýný iþleyin ve çýktý metnini ayarlayýn
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    chatOutputText.text = response.candidates[0].content.parts[0].text; // Yanýt formatýna göre doðru alaný kullanýn
                }
            }
        }
    }

    [System.Serializable]
    private class ChatbotResponse
    {
        public Candidate[] candidates;
    }

    [System.Serializable]
    private class Candidate
    {
        public Content content;
    }

    [System.Serializable]
    private class Content
    {
        public Part[] parts;
    }

    [System.Serializable]
    private class Part
    {
        public string text;
    }
}
*/
































