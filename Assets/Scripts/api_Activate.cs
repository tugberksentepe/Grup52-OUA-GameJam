


using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class api_Activate : MonoBehaviour
{
    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatOutputText;
    private string apiKey = "AIzaSyDr4774yt5SqYm3hQuGtBgBH6fYgUSXWuE"; // API anahtar�n�z� buraya ekleyin
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"; // U� nokta URL'si

    private int questionCount = 0;

    private Queue<string> questions = new Queue<string>(new string[] {
        "Zaman yolcusu, ne zaman do�dun?",
        "Zaman yolcusu, en sevdi�in an� hat�rl�yor musun?",
        "Zaman yolcusu, en sevdi�in renk nedir?",
        "Zaman yolcusu, en b�y�k hayalin neydi?",
        "Zaman yolcusu, en b�y�k korkun neydi?"
    });

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        chatOutputText.text = "Merhaba! Benimle konu�mak i�in haz�r m�s�n? L�tfen bir soru sor ve ge�mi�ini ke�fetmeme yard�mc� ol.";
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
            chatOutputText.text = "�zg�n�m, art�k ba�ka bir soru kabul etmiyorum.";
            yield break;
        }

        // API'ye JSON format�nda veri g�nderin
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

                    // JSON yan�t�n� i�leyin ve ��kt� metnini ayarlay�n
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    chatOutputText.text = response.candidates[0].content.parts[0].text; // Yan�t format�na g�re do�ru alan� kullan�n

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
    private string apiKey = "AIzaSyDr4774yt5SqYm3hQuGtBgBH6fYgUSXWuE"; // API anahtar�n�z� buraya ekleyin
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"; // U� nokta URL'si

    public void OnSubmit()
    {
        string inputText = chatInputField.text;
        StartCoroutine(GetChatbotResponse(inputText));
    }

    private IEnumerator GetChatbotResponse(string inputText)
    {
        // API'ye JSON format�nda veri g�nderin
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

                    // JSON yan�t�n� i�leyin ve ��kt� metnini ayarlay�n
                    ChatbotResponse response = JsonUtility.FromJson<ChatbotResponse>(jsonResponse);
                    chatOutputText.text = response.candidates[0].content.parts[0].text; // Yan�t format�na g�re do�ru alan� kullan�n
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
































