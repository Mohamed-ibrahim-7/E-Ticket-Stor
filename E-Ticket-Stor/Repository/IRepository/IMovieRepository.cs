using E_Ticket_Stor.Models;
using E_Ticket_Stor.Repositories.IRepositories;

namespace E_Ticket_Stor.Repository.IRepository
{
    public interface IMovieRepository : IRepository<Movie>
    {
        // أضف أي دوال مخصصة للفيديوهات هنا إذا احتجت
    }
}
