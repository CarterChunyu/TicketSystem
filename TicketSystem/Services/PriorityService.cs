using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.IRepositories;
using TicketSystem.Models;

namespace TicketSystem.Services
{
    public class PriorityService
    {
        private readonly ITRepository<Priority> _priorityRepository;
        public PriorityService(ITRepository<Priority> tRepository)
        {
            _priorityRepository = tRepository;
        }
        public IEnumerable<Priority> GetAllPriorities()
        {
            return _priorityRepository.GetAll();
        }
    }
}
