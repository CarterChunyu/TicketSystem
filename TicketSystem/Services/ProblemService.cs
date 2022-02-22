using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.IRepositories;
using TicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using TicketSystem.ViewModels;

namespace TicketSystem.Services
{
    public class ProblemService 
    {
        private readonly ITRepository<Problem> _problemRepository;
        //private readonly LoginService _loginService;
        //private readonly ProblemCatrgoryService _problemCatrgoryService;
        public ProblemService(ITRepository<Problem> tRepository)
        {
            _problemRepository = tRepository;
        }
        public async Task<int> AddProblemAsync(Problem problem)
        {
            return await _problemRepository.AddTAsync(problem);
        }
        public async Task<Problem> GetProblemByIdAsync(int id)
        {
            return await _problemRepository.GetAll().Include(p=>p.ProblemCategory)
                .Include(p=>p.Priority).Include(p=>p.Severity).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<int> EditProblemAsync(Problem problem)
        {
            return await _problemRepository.UpdateTAsync(problem);
        }
        public async Task<int> RemoveProblemAsync(Problem problem)
        {
            return await _problemRepository.RemoveTAsync(problem);
        }
        public IEnumerable<Problem> GetAllProblems()
        {
            return _problemRepository.GetAll().Include(p => p.ProblemCategory).Include(p => p.Priority)
                .Include(p => p.Severity);
        }
        public IEnumerable<Problem> GetAllProblemsByStatusAndCategoryAsync(bool isSolved,string CategoryName)
        {
            IEnumerable<Problem> problems = _problemRepository.GetAll().Include(p => p.ProblemCategory)
                 .Include(p => p.Priority).Include(p => p.Severity).Where(p => p.isSolved == isSolved);
            if (CategoryName != "All")
                problems = problems.Where(p => p.ProblemCategory.Name == CategoryName);
            return problems.OrderBy(p => p.ProblemCategory.Name).ThenBy(p=>p.Id);              
        }
        public async Task<bool> IsSummaryExistedAsync(string summary)
        {
           return await _problemRepository.GetAll().AnyAsync(p => p.Summary == summary);
        }



    }
}
