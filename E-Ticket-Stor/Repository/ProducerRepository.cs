using E_Ticket_Stor.Repositories;
using E_Ticket_Stor.Repository.IRepository;


namespace E_Ticket_Stor.Repository
{
    public class ProducerRepository : Repository<Producer>, IProducerRepository
    {
        private readonly ApplicationDbContext _context;

        public ProducerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

