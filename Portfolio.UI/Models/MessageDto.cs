namespace Portfolio.UI.Models;

public class MessageDto
{
    public int Id { get; set; }

    // Gönderen kişinin adı ve soyadı
    public string FullName { get; set; }

    // İletişim kurulacak e-posta adresi
    public string Email { get; set; }

    // Mesajın konusu (Örn: İş teklifi, Teşekkür vs.)
    public string Subject { get; set; }

    // Mesajın asıl içeriği
    public string Content { get; set; }

    // Mesajın gönderildiği tarih ve saat
    public DateTime DateSent { get; set; }

    // Mesajın okunup okunmadığı bilgisi (Gelen kutusunda kalın/ince yazı ayarı için)
    public bool IsRead { get; set; }
}