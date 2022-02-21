using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketSystem.IRepositories;
using TicketSystem.Models;


namespace TicketSystem.Services
{
    public class UserService  
    {
        private readonly ITRepository<User> _userRepository;
        public UserService(ITRepository<User> tRepository)
        {
            _userRepository = tRepository;
        }
        public async Task<bool> IsAccountExistedAsync(string name)
        {
            return await _userRepository.GetAll().AnyAsync(p => p.Name == name);
        }
        public async Task<bool> IsPasswordCorrect(int id,string password)
        {
            User user =await _userRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
            return user.Password == password;
        }
        public async Task<int> AddUserAsync(User user)
        {
            return await _userRepository.AddTAsync(user);
        }
        public async Task<User> GetUserByAccountAsync(string account)
        {
            return await _userRepository.GetAll().Include(p => p.Role)
                .FirstOrDefaultAsync(p => p.Account == account);
        }
        public async Task<User> GetUserByAccountPasswordAsync(string account,string password)
        {
            return  await _userRepository.GetAll().Include(p=>p.Role)
                .FirstOrDefaultAsync(p => p.Account.ToUpper() == account.ToUpper()
                && p.Password.ToUpper() == password.ToUpper());
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetAll().Include(p=>p.Role).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<int> UpdateUserAsync(User user)
        {
           return await _userRepository.UpdateTAsync(user);
        } 

        public async Task<int> RemoveUserAsync(User user)
        {
            return await _userRepository.RemoveTAsync(user);
        }

        public IEnumerable<User> GetAllUSers()
        {
            return _userRepository.GetAll().Include(p=>p.Role);
        }
        //public async Task<int> AddUserAsync(User user)
        //{
        //    return await _userRepository.AddTAsync(user);
        //}
        //public async Task<int> RemoveUserAsync(User user)
        //{
        //    return await _userRepository.RemoveTAsync(user);
        //}
        //public async Task<int> UpdateUserAsync(User user)
        //{
        //    return await _userRepository.UpdateTAsync(user);
        //}

        //public async Task<int> AddRangeUsersAsync(IEnumerable<User> users)
        //{
        //    return await _userRepository.AddRangeTAsync(users);
        //}
        //public async Task<int> RemoveRangeUsersAsync(IEnumerable<User> users)
        //{
        //    return await _userRepository.RemoveRangeTAsync(users);
        //}
        //public async Task<int> UpdateRangeUserAsync(IEnumerable<User> users)
        //{
        //    return await _userRepository.UpdateRangeTAsync(users);
        //}
    }
}
