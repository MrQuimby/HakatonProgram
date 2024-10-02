using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;



public class Judge
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _endpoint = "https://api.openai.com/v1/chat/completions";


    class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "";
        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
    }
    class Request
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; } = "";
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new();
    }

    class ResponseData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";
        [JsonPropertyName("object")]
        public string Object { get; set; } = "";
        [JsonPropertyName("created")]
        public ulong Created { get; set; }
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; } = new();
        [JsonPropertyName("usage")]
        public Usage Usage { get; set; } = new();
    }

    class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("message")]
        public Message Message { get; set; } = new();
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = "";
    }

    class Usage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }
        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }

    public Judge(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
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
            new Message { Role = "system", Content = "�� ������� �� ������ ��������� �����.  �������� ������ �� ����� �� 0 �� 100 ������, �������� ���������." },
            new Message
            {
                Role = "user",
                Content = $"������� ������� � ���������� �����������: \"{context}\", ������������ (�� ����� �� 0 �� 5): {complexity}, � ����������: \"{description}\", ������� ��� �� ��������� ���������� � ��������� ������ �� 0 �� 100 ������. ������ ����� �������� �� ��������� �������, ��� ���� ����� ������� ������ ��������� �������� � ����� ������� ������. � �������� ������ ������� ������ ����, ����������� �� ���������� ������ �����."
            }
        };

        var requestData = new Request
        {
            ModelId = "gpt-4o-mini ",
            Messages = messages
        };

        try
        {
            using var response = await _httpClient.PostAsJsonAsync(_endpoint, requestData);
            response.EnsureSuccessStatusCode(); // ������� ����������, ���� ������ ��� �� ��������

            var responseData = await response.Content.ReadFromJsonAsync<ResponseData>();
            var choices = responseData?.Choices ?? new List<Choice>();

            if (choices.Count == 0)
            {
                throw new Exception("API �� ������ ������� ��������� ������.");
            }

            var responseText = choices[0].Message.Content.Trim();
            if (int.TryParse(responseText, out int score))
            {
                return score;
            }
            else
            {
                throw new Exception($"API ������ �� �������� ��������: {responseText}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"������ HTTP �������: {ex.Message}");
        }
        catch (JsonException ex)
        {
            throw new Exception($"������ JSON: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"��������� ����������� ������: {ex.Message}");
        }
    }
}


