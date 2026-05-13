using System.Net.Http.Json;
using Portfolio.UI.Models;

namespace Portfolio.UI.Services;

public class MessageService
{
    private readonly HttpClient _httpClient;

    public MessageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Admin panelinde mesajları listelemek için kullandığımız metot
    public async Task<List<MessageDto>> GetAllMessagesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<MessageDto>>("api/messages");
    }

    // İŞTE EKSİK OLAN VE HATAYA SEBEP OLAN METOT:
    public async Task<bool> CreateMessageAsync(CreateMessageDto messageDto)
    {
        // Backend'deki MessagesController'ın [HttpPost] metoduna veriyi gönderiyoruz
        var response = await _httpClient.PostAsJsonAsync("api/messages", messageDto);

        // Eğer HTTP 200 veya 201 döndüyse true, aksi halde false döner
        return response.IsSuccessStatusCode;
    }

    // Mesaj silme metodu
    public async Task<bool> DeleteMessageAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/messages/{id}");
        return response.IsSuccessStatusCode;
    }
}