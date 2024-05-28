using Entities;

namespace Data.Repository.Interface
{
    public interface IUserRepository : IBaseRepository
    {
        Task<User?> GetByEmailId(string emailId);
        Task<(ICollection<User>, int)> GetAllUsers(int pageNo, int pageSize, string? searchTerm);
    }
}