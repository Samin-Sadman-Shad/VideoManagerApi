using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using VideoManagerApi.Data;
using VideoManagerApi.Models;

namespace VideoManagerApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProductVideoContext _dbContext;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(ProductVideoContext context, ILogger<UserRepository> logger) 
        {
            _dbContext = context;
            _logger = logger;
        }

        async Task<User> IUserRepository.GetUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            return user;
        }


        async Task<User> IUserRepository.GetUserByEmail(string email)
        {
            var user = _dbContext.Users.Local.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            return user;
        }

        async Task<List<User>> IUserRepository.GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        async Task<User> IUserRepository.CreateUser(User user)
        {
            try
            {
                //_dbContext.Users.Entry(user).State = EntityState.Added;
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
            return user;
        }

        async Task<User> IUserRepository.DeleteUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            _dbContext.Users.Entry(user).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
            return user;
        }



        async Task<User> IUserRepository.UpdateUser(User user)
        {
            var userEntity = _dbContext.Users.Attach(user);
            userEntity.State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return user;

        }
    }
}
