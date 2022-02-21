using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.IRepositories;
using TicketSystem.Models;
using TicketSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.Services
{
    public class RoleService
    {
        private readonly ITRepository<Role> _roleRepository;

        public  RoleService(ITRepository<Role> tRepository)
        {
            _roleRepository = tRepository;
        }
        public IEnumerable<Role> GetAllRoles()
        {
            return _roleRepository.GetAll();
        }
        public async Task<Role> GetRoleAsync(int Id)
        {
            return await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<int> AddRoleAsync(Role role)
        {
            return await _roleRepository.AddTAsync(role);
        }
        public async Task<int> RemoveRoleAsync(Role role)
        {
            return await _roleRepository.RemoveTAsync(role);
        }
        public async Task<int> UpdateRoleAsync(Role role)
        {
            return await _roleRepository.UpdateTAsync(role);
        }
        public async Task<bool> RoleExistedAsync(int id)
        {
            return await _roleRepository.GetAll().AnyAsync(p => p.Id == id);
        }
    }
}
