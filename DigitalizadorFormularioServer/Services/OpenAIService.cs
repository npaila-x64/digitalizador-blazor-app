using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

public class OpenAIResponse
{
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }
}

public class Choice
{
    [JsonPropertyName("message")]
    public Message Message { get; set; }
}

public class Message
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "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";


    public OpenAIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetResponseString(string imageBase64)
    {
        var endpoint = "https://api.openai.com/v1/chat/completions";
        var data = new
        {
            model = "gpt-4-vision-preview",
            messages =
            new[]
                {
                    new
                    {
                        role = "user",
                        content =
                            new object[]
                                {
                                    new
                                    {
                                        type = "text",
                                        text = "¿Puedes transcribir la siguiente imagen en un formato JSON correctamente formateado, enfocándote en cada campo, es decir, APELLIDO PATERNO, APELLIDO MATERNO, NOMBRES, RUT, ESPECIALIDAD DEL FUNCIONARIO, AÑOS DE SERVICIOS, NOMBRE DE LA UNIDAD, TELÉFONO, MONTO, DETALLE SITUACIÓN ECONOMICA y PETICIÓN ESPECÍFICA? Por favor, Asegurate de que sea exactamente lo que dice la imagen, tambien Asegúrate de que el JSON esté correctamente formateado según los estándares para este formato. Solo dame el JSON como respuesta para copiarlo y pegarlo para su posterior uso."
                                    },
                                    new
                                    {
                                        type = "image_url",
                                        image_url = "data:image/jpeg;base64," + imageBase64
                                    }
                                }
                    }
                },
            max_tokens=2048,
            temperature = 0
        };

        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json"),
            Headers = { { "Authorization", $"Bearer {_apiKey}" } }
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);
        return openAIResponse.Choices.First().Message.Content;
    }
}
