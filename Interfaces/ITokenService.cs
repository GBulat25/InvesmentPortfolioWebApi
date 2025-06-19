using StocksWebApi.Models;

namespace StocksWebApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
