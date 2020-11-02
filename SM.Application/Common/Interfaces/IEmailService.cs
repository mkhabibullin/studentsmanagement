using SM.Application.Common.Models.Email;
using System.Threading.Tasks;

namespace SM.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
