
using System.Windows.Input;

namespace VideoManagerApi.Login
{
    public record LoginCommand(string email);

    public record LoginRequest(string Email);

    class LoginCommandHandler
    {
        /// <summary>
        /// Login user with LoginRequest and generate a JWT
        /// </summary>
        /// <param name="command"></param>
        public async Task<string> Handle(LoginCommand command)
        {
            //get the user from database
            //if the user exist in database, generate jwt
            //return the jwt
            return null;
        }
    }
}