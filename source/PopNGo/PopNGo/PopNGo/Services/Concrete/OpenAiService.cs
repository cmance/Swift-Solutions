using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PopNGo.Models;

public class OpenAiPayload {
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; }
    [JsonPropertyName("temperature")]
    public int Temperature { get; set; }
    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }
    [JsonPropertyName("top_p")]
    public int TopP { get; set; }
    [JsonPropertyName("frequency_penalty")]
    public int FrequencyPenalty { get; set; }
    [JsonPropertyName("presence_penalty")]
    public int PresencePenalty { get; set; }
}

public class Message {
    [JsonPropertyName("role")]
    public string Role { get; set; }
    [JsonPropertyName("content")]
    public List<Content> Content { get; set; }
}

public class Content {
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class OpenAiApiResponse {
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("object")]
    public string Object { get; set; }
    [JsonPropertyName("created")]
    public int Created { get; set; }
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerprint { get; set; }
}

public class Usage {
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

public class ResponseMessage {
    [JsonPropertyName("role")]
    public string Role { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class Choice {
    [JsonPropertyName("message")]
    public ResponseMessage Message { get; set; }
    [JsonPropertyName("logprobs")]
    public object Logprobs { get; set; }
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
    [JsonPropertyName("index")]
    public int Index { get; set; }
}

namespace PopNGo.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenAiService> _logger;

        public OpenAiService(HttpClient httpClient, ILogger<OpenAiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> FindMostRelevantWordFromString(string description)
        {
            // Uses a system message to find the most relevant word from a string. String can include title and description.

            var openAiPayload = new OpenAiPayload
            {
                Model = "gpt-3.5-turbo",
                Messages = new List<Message>
                {
                    new Message
                    {
                        Role = "system",
                        Content = new List<Content>
                        {
                            new Content
                            {
                                Type = "text",
                                Text = "You are an assistant that extracts the single most relevant word from a given event text description. Only provide the most relevant word without any additional text or explanation. This word is going to be used to find similar events of the same category, make sure the word isn't so specific to the event that a similar one couldn't be found. The word should not be related to the date of the event."
                            }
                        }
                    },
                    new Message
                    {
                        Role = "user",
                        Content = new List<Content>
                        {
                            new Content
                            {
                                Type = "text",
                                Text = description
                            }
                        }
                    }
                },
                Temperature = 1,
                MaxTokens = 256,
                TopP = 1,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };
            
            var jsonHttpContent = JsonSerializer.Serialize(openAiPayload);

            var httpContent = new StringContent(jsonHttpContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("v1/chat/completions", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to fetch data with status code: {0}, reason: {1}, content: {2}",
                                 response.StatusCode, response.ReasonPhrase, errorContent);
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                var openAiApiResponse = JsonSerializer.Deserialize<OpenAiApiResponse>(responseBody, options);
                return openAiApiResponse.Choices[0].Message.Content; // Return the most relevant word
            }
            catch (JsonException ex)
            {
                _logger.LogError("Failed to deserialize the response: {0}", ex.Message);
                return null;
            }
        }
    }
}
