using BlogManagement.Data;
using Data.Repository.Interface;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class UserRepository : BaseRepository, IUserRepository
{
    private readonly BlogDbContext context;
    public UserRepository(BlogDbContext context): base(context)
    {
        this.context = context;
    }

    public async Task<User?> GetByEmailId(string emailId)
    {
        return await context.User.Include(it => it.Role).FirstOrDefaultAsync(it => it.EmailId == emailId); 
    }

    public async Task<(ICollection<User>, int)> GetAllUsers(int pageNo, int pageSize, string? searchTerm)
    {
        IQueryable<User> query = context.User;

        if (!String.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(it => it.FirstName.Contains(searchTerm) || it.LastName.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();

        var users = await query.Skip((pageNo - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return (users, totalCount);
    }
}

