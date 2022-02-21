using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.IRepositories;
using TicketSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.Services
{
    public class ProblemCatrgoryService
    {
        private readonly ITRepository<ProblemCategory> _problemCategoryRepository;
        public ProblemCatrgoryService(ITRepository<ProblemCategory> tRepository)
        {
            _problemCategoryRepository = tRepository;
        }
        public IEnumerable<ProblemCategory> GetAllProblemCategories()
        {
            return _problemCategoryRepository.GetAll();
        }
        public IEnumerable<string> GetAllNamewithAll()
        {
            List<string> names = _problemCategoryRepository.GetAll().Select(p => p.Name).ToList();
            names.Add("All");
            return names;
        }
        public async Task<ProblemCategory> GetProblemCategorybyId(int id)
        {
            return await _problemCategoryRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
