using E_Ticket_Stor.Repositories;
using E_Ticket_Stor.Repository.IRepository;

namespace E_Ticket_Stor.Repository
{
    
    
        public class MovieRepository : Repository<Movie>, IMovieRepository
        {
            private readonly ApplicationDbContext _context;

            public MovieRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }
        }
    }

