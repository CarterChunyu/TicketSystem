using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketSystem.IRepositories
{
    public interface ITRepository<T> where T : class
    {
        Task<int> AddRangeTAsync(IEnumerable<T> ts);
        Task<int> AddTAsync(T t);
        DbSet<T> GetAll();
        Task<int> RemoveRangeTAsync(IEnumerable<T> ts);
        Task<int> RemoveTAsync(T t);
        Task<int> UpdateRangeTAsync(IEnumerable<T> t);
        Task<int> UpdateTAsync(T t);
    }
}