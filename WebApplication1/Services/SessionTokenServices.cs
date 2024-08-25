using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ISessionTokenServices
    {
        public Task<SessionToken> Savetoken(SessionToken Token);
        Task<User> ViewToken(string StringToken);
    }

    public class SessionTokenServices : ISessionTokenServices
    {
        private readonly JwtContext DbContext;

        public SessionTokenServices(JwtContext dBContext)
        {
            this.DbContext = dBContext;
        }

        public async Task<SessionToken> Savetoken(SessionToken Token)
        {
            if (Token == null) throw new ArgumentNullException("token");

            await DbContext.AddAsync(Token);

            await DbContext.SaveChangesAsync();

            return Token;
        }
        public async Task<User> ViewToken(string StringToken)
        {
            if (StringToken == null) throw new ArgumentNullException("token");

            SessionToken sessionToken = DbContext.Sessiontokens
                .Where(token => token.Token == StringToken)
                .FirstOrDefault();

            if (sessionToken == null) return null;  

            return DbContext.Usuario
                .Where(user => user.Id == sessionToken.UserId)
                .FirstOrDefault();
        }
    }
}
