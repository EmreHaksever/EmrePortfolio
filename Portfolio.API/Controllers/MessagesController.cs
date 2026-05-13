using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;

namespace Portfolio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    [Authorize] // Sadece admin görebilir
    public async Task<IActionResult> GetAllMessages()
    {
        var messages = await _messageService.GetAllMessagesAsync();
        return Ok(messages);
    }

    [HttpPost] // Herkes mesaj gönderebilir
    public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDto createMessageDto)
    {
        await _messageService.CreateMessageAsync(createMessageDto);
        return Ok(new { message = "Mesajınız başarıyla iletildi." });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        await _messageService.DeleteMessageAsync(id);
        return Ok(new { message = "Mesaj başarıyla silindi." });
    }

    [HttpPost("mark-as-read/{id}")]
    [Authorize]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _messageService.MarkAsReadAsync(id);
        return Ok(new { message = "Mesaj okundu olarak işaretlendi." });
    }
}