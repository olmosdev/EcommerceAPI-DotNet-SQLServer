using EcommerceAPI.Models;
using EcommerceAPI.Models.Dtos;

namespace EcommerceAPI.Repository.IRepository;

public interface IUserRepository
{
    ICollection<User> GetUsers();
    User? GetUser(int id);
    bool IsUniqueUser(string username);
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    Task<User> Register(CreateUserDto createUserDto);
}
