using E_Ticket_Stor.Repositories;
using E_Ticket_Stor.Repository.IRepository;

namespace E_Ticket_Stor.Repository
{
    
    
        public class UserOTPRepository : Repository<UserOTP>, IUserOTPRepository
        {
            private readonly ApplicationDbContext _context;

            public UserOTPRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }
        }
    }
