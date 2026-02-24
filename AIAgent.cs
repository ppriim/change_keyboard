using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeyboardLayoutSwitcher
{
    public static class AIAgent
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        
        public static async Task<string> CorrectTextAsync(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            
            // Check if AI is enabled and API key exists
            if (!ConfigManager.Settings.EnableAI || string.IsNullOrWhiteSpace(ConfigManager.Settings.OpenAIApiKey))
            {
                return KeyboardMapper.Convert(input); // Fallback to naive deterministic mapper
            }

            try
            {
                var requestBody = new
                {
                    model = ConfigManager.Settings.AIModel,
                    messages = new[]
                    {
                        new { 
                            role = "system", 
                            content = "You are a specialized keyboard layout typo corrector for Thai and English users. The user typed on the wrong keyboard layout (Thai instead of English, or vice versa) or mixed them. Your task is to output ONLY the corrected string that makes the most sense contextually. Resolve ambiguous characters (like the dash '-' which is also Thai letter 'ข' and 'ช') based on the context of the sentence. NEVER add conversational text. Output the raw corrected string only." 
                        },
                        new { 
                            role = "user", 
                            content = input 
                        }
                    },
                    temperature = 0.0 // Keep it deterministic
                };

                string jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
                {
                    Content = content
                };
                
                requestMessage.Headers.Add("Authorization", $"Bearer {ConfigManager.Settings.OpenAIApiKey}");

                using var response = await _httpClient.SendAsync(requestMessage);
                
                if (!response.IsSuccessStatusCode)
                {
                    // Fallback to naive mapper on API error
                    return KeyboardMapper.Convert(input);
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                
                using var document = JsonDocument.Parse(jsonResponse);
                string correctedText = document.RootElement
                                        .GetProperty("choices")[0]
                                        .GetProperty("message")
                                        .GetProperty("content")
                                        .GetString();

                if (!string.IsNullOrEmpty(correctedText))
                {
                    return correctedText.Trim();
                }

                return KeyboardMapper.Convert(input);
            }
            catch
            {
                // Fallback on exception
                return KeyboardMapper.Convert(input);
            }
        }
    }
}
