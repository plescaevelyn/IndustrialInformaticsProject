using Microsoft.EntityFrameworkCore;
using PlayHarmoniez.App_Data;


namespace PlayHarmoniez.Models
{
    public class AuthenticateLogin : ILogin
    {
        private readonly DataContext _dataContext;
        public AuthenticateLogin(DataContext dataContext) {
            _dataContext = dataContext;
        }
        public async Task<User> AuthenticateUser(string Username, string Password,bool AdminCheck)
        {
            var succeeded = await _dataContext.Users.FirstOrDefaultAsync(authUser => authUser.Username == Username && authUser.Password == Password);
            return succeeded;
        }

        public async Task<IEnumerable<User>> getuser()
        {
            return await _dataContext.Users.ToListAsync();
        }
    }
}
