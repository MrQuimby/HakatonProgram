
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Judge : MonoBehaviour
{
    private readonly string _apiKey;
    private readonly string _endpoint = "https://api.openai.com/v1/chat/completions";

    [Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    private class Request
    {
        public string model;
        public List<Message> messages;
    }

    [Serializable]
    private class ResponseData
    {
        public string id;
        public string @object;
        public ulong created;
        public List<Choice> choices;
        public Usage usage;
    }

    [Serializable]
    private class Choice
    {
        public int index;
        public Message message;
        public string finish_reason;
    }

    [Serializable]
    private class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }

    public Judge(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<int> GetScoreAsync(string context, int complexity, string description)
    {
        // �������� ������� ������
        if (complexity < 0 || complexity > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(complexity), "��������� ������ ���� � ��������� �� 0 �� 5.");
        }

        var messages = new List<Message>
        {
            new Message { role = "system", content = "�� ������� �� ������ ��������� �����. �������� ������ �� ����� �� 0 �� 100 ������, �������� ���������." },
            new Message
            {
                role = "user",
                content = $"������� ������� � ���������� �����������: \"{context}\", ������������ (�� ����� �� 0 �� 5): {complexity}, � ����������: \"{description}\", ������� ��� �� ��������� ���������� � ��������� ������ �� 0 �� 100 ������. ������ ����� �������� �� ��������� �������, ��� ���� ����� ������� ������ ��������� �������� � ����� ������� ������. � �������� ������ ������� ������ ����, ����������� �� ���������� ������ �����."
            }
        };

        var requestData = new Request
        {
            model = "gpt-4", // �������� �� ���������� ������������� ������
            messages = messages
        };

        var jsonRequestData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(_endpoint, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", $"Bearer {_apiKey}");
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield(); // Wait until the request is done
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception("������ ��� �������� ������� � API: " + request.error);
            }

            var responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
            var choices = responseData?.choices ?? new List<Choice>();

            if (choices.Count == 0)
            {
                throw new Exception("API �� ������ ������� ��������� ������.");
            }

            var responseText = choices[0].message.content.Trim();
            if (int.TryParse(responseText, out int score))
            {
                return score;
            }
            else
            {
                throw new Exception("API ������ ������������ �����.");
            }
        }
    }
}