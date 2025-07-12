using E_Ticket_Stor.Repositories;
using E_Ticket_Stor.Repository.IRepository;

namespace E_Ticket_Stor.Repository
{
    
    
        public class CinemaRepository : Repository<Cinema>, ICinemaRepository
        {
            private readonly ApplicationDbContext _context;

            public CinemaRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }
        }
   }

