using Data.Repository.Interface;
using Entities;
using Services.Dto;
using static Entities.Constants;

namespace Services;

public class UserService
{
    private readonly IUserRepository userRepository;
    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<(User?, string)> Add(CreateNewUserRequest dto)
    {
        try
        {
            var userByEmail = await userRepository.GetByEmailId(dto.EmailId);
            if(userByEmail is not null)
            {
                return (null, Users.USER_ALREADY_EXISTS_WITH_THIS_EMAIL);
            }

            dto.Password = HashPassword(dto.Password);
            
            var newUser = new User(dto.FirstName, dto.LastName, dto.EmailId, dto.Password, Guid.NewGuid().ToString());

            userRepository.Add(newUser);

            await userRepository.SaveAsync();

            return (newUser, Users.USER_CREATED_SUCCESSFULLY);
        }
        catch
        {
            return (null, Users.ERROR_OCCURED_CREATING_USER);
        }
        
    }

    public async Task<(User?, string)> GetUser(LoginRequest dto)
    {
        var user = await userRepository.GetByEmailId(dto.EmailId);
        if(user is null)
        {
            return (user, Users.INVALID_USER_CRENDENTIAL);
        }

        if(!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            return (null, Users.INVALID_USER_CRENDENTIAL);
        }
        
        if(!user.IsEmailVerified)
        {
            return (user, Users.VERIFY_EMAIL);
        }

        return (user, string.Empty);
    }

    public async Task<(ICollection<User>, int)> GetAllUsers(GetAllUsersRequest dto)
    {
        if(dto.PageNo <= 0)
        {
            dto.PageNo = 1;
        }

        if(dto.PageSize <= 0)
        {
            dto.PageSize = 5;
        }

        return await userRepository.GetAllUsers(dto.PageNo, dto.PageSize, dto.SearchTerm);
    }

    public async Task<User?> GetUserProfile(string emailId)
    {
        return await userRepository.GetByEmailId(emailId);
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);;
    }
}
