using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Data;
using Microsoft.EntityFrameworkCore;
using TicketSystem.IRepositories;

namespace TicketSystem.Repositories
{
    public class TRepository<T> : ITRepository<T> where T : class
    {
        private readonly TicketContext _ticketContext;
        public TRepository(TicketContext context)
        {
            _ticketContext = context;
        }
        public DbSet<T> GetAll()
        {
            return _ticketContext.Set<T>();
        }        
        public async Task<int> AddTAsync(T t)
        {
            await _ticketContext.AddAsync(t);
            return await _ticketContext.SaveChangesAsync();
        }
        public async Task<int> RemoveTAsync(T t)
        {
            _ticketContext.Remove(t);
            return await _ticketContext.SaveChangesAsync();
        }
        public async Task<int> UpdateTAsync(T t)
        {
            _ticketContext.Update(t);
            return await _ticketContext.SaveChangesAsync();
        }

        public async Task<int> AddRangeTAsync(IEnumerable<T> ts)
        {
            await _ticketContext.AddRangeAsync(ts);
            return await _ticketContext.SaveChangesAsync();
        }
        public async Task<int> RemoveRangeTAsync(IEnumerable<T> ts)
        {
            _ticketContext.RemoveRange(ts);
            return await _ticketContext.SaveChangesAsync();
        }
        public async Task<int> UpdateRangeTAsync(IEnumerable<T> t)
        {
            _ticketContext.UpdateRange(t);
            return await _ticketContext.SaveChangesAsync();
        }

    }
}
