using System.Net.Http.Json;
using Portfolio.UI.Models;

namespace Portfolio.UI.Services;

public class MessageService
{
    private readonly HttpClient _httpClient;
    public MessageService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<List<MessageDto>> GetAllMessagesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<MessageDto>>("api/messages");
    }

    public async Task<bool> DeleteMessageAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/messages/{id}");
        return response.IsSuccessStatusCode;
    }

    // Mesajı okundu olarak işaretlemek için (Opsiyonel)
    public async Task<bool> MarkAsReadAsync(int id)
    {
        var response = await _httpClient.PostAsync($"api/messages/mark-as-read/{id}", null);
        return response.IsSuccessStatusCode;
    }
}