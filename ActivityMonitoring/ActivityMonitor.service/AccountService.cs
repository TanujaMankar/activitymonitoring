using ActivityMonitor.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityMonitor.service
{
    public interface IAccountService
    {
        Task<UserModal> Login(string username, string password,CancellationToken cancellationToken);
        Task Logout(long userId, CancellationToken cancellationToken);

        Task <UserModal> Register(RegisterModal model,CancellationToken cancellationToken);

        Task ResetPassword(ResetPasswordModal model, CancellationToken cancellationToken);

        Task ForgotPassword(string Email, CancellationToken cancellationToken);

        Task DeleteAccount(long userId, CancellationToken cancellationToken);

    }
    public class AccountService : IAccountService
    {

        public Task<UserModal> Register(RegisterModal model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserModal> Login(string username, string password, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAccount(long userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ForgotPassword(string Email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

 

        public Task Logout(long userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(ResetPasswordModal model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
