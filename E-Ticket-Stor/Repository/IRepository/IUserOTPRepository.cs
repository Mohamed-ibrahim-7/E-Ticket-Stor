using E_Ticket_Stor.Models;
using E_Ticket_Stor.Repositories.IRepositories;

namespace E_Ticket_Stor.Repository.IRepository
{
    public interface IUserOTPRepository : IRepository<UserOTP>
    {
        // أضف هنا أي دوال مخصصة للتعامل مع OTP إن لزم الأمر
    }
}
