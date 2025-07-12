using E_Ticket_Stor.Repositories;
using E_Ticket_Stor.Repository.IRepository;

namespace E_Ticket_Stor.Repository
{
    
    
        public class ActorRepository : Repository<Actor>, IActorRepository
        {
            private readonly ApplicationDbContext _context;

            public ActorRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }
        }
    }

