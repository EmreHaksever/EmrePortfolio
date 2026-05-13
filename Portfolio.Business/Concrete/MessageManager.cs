using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;
using Portfolio.DataAccess.Contexts; // Klasör yapına göre kontrol et
using Portfolio.Domain.Entities;   // Message burada

namespace Portfolio.Business.Concrete;

public class MessageManager : IMessageService
{
    private readonly PortfolioDbContext _context;
    private readonly IMapper _mapper;

    public MessageManager(PortfolioDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MessageDto>> GetAllMessagesAsync()
    {
        var messages = await _context.Messages
            .OrderByDescending(x => x.DateSent)
            .ToListAsync();

        return _mapper.Map<List<MessageDto>>(messages);
    }

    public async Task<MessageDto> GetMessageByIdAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null) return null;

        return _mapper.Map<MessageDto>(message);
    }

    public async Task CreateMessageAsync(CreateMessageDto createMessageDto)
    {
        var message = _mapper.Map<Message>(createMessageDto);

        // Yeni mesaj için sistem tarafında otomatik verileri set ediyoruz
        message.DateSent = DateTime.Now;
        message.IsRead = false;

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMessageAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message != null)
        {
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAsReadAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message != null)
        {
            message.IsRead = true;
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }
    }
}