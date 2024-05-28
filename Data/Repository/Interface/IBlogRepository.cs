using Entities.Models;

namespace Data.Repository.Interface
{
    public interface IBlogRepository : IBaseRepository
    {
        Task<(ICollection<Blog>, int)> GetAllByCreatedById(int pageNo, int pageSize, string? searchTerm, long requestorId);
        Task<(ICollection<Blog>, int)> GetAll(int pageNo, int pageSize, string? searchTerm);
        Task<Blog?> GetById(int id);
        Task<(ICollection<Blog>, int)> GetAllBlogs(int pageNo, int pageSize, string? searchTerm, long requestorId);
    }
}