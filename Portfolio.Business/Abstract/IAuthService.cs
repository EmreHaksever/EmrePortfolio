using Portfolio.Business.DTOs; // LoginDto'yu tanıması için şart!

namespace Portfolio.Business.Abstract;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
}