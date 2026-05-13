using Portfolio.Business.DTOs;

namespace Portfolio.Business.Abstract;

public interface IMessageService
{
    // Tüm mesajları listelemek için (Admin Paneli)
    Task<List<MessageDto>> GetAllMessagesAsync();

    // Belirli bir mesajın detayını görmek için
    Task<MessageDto> GetMessageByIdAsync(int id);

    // Yeni bir mesaj göndermek için (Ziyaretçi Formu)
    Task CreateMessageAsync(CreateMessageDto createMessageDto);

    // Mesajı sistemden silmek için
    Task DeleteMessageAsync(int id);

    // Mesajı "Okundu" olarak işaretlemek için (Kullanıcı deneyimi için önemli)
    Task MarkAsReadAsync(int id);
}