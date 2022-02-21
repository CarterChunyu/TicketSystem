using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.IRepositories;
using TicketSystem.Models;

namespace TicketSystem.Services
{
    public class SeverityService
    {
        private readonly ITRepository<Severity> _severityRepository;
        public SeverityService(ITRepository<Severity> tRepository)
        {
            _severityRepository = tRepository;
        }
        public IEnumerable<Severity> GetAllSeverities()
        {
            return _severityRepository.GetAll();
        }
    }
}
