using System.Collections.Generic;
using System.Threading.Tasks;
using PlayHarmoniez.Models;


namespace PlayHarmoniez.Models
{
    public interface ILogin
    {
        Task<IEnumerable<User>> getuser();
        Task<User> AuthenticateUser(string Username, string Password,bool AdminCheck);
    }
}
