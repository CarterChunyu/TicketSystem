using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Models;
using TicketSystem.ViewModels;

namespace TicketSystem.Profiles
{
    public class ControllerMappings:Profile
    {
        public ControllerMappings()
        {
            CreateMap<ProblemShowVM, Problem>();
            CreateMap<Problem, ProblemShowVM>();
            CreateMap<ProblemCreateVM, Problem>()
                .ForMember(x => x.Id, y => y.Ignore());
            CreateMap<Problem, ProblemCreateVM>();
            CreateMap<ProblemEditVM, Problem>();
            CreateMap<Problem, ProblemEditVM>();

            CreateMap<User, UserShowVM>()
                .ReverseMap();
            CreateMap<UserCreateVM, User>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForPath(x => x.Role, y => y.Ignore())
                .ReverseMap();
            CreateMap<UserEditVM, User>()
                .ForPath(x => x.Role, y => y.Ignore())
                .ReverseMap();        
            CreateMap<UserPasswordVM, User>()
                .ForMember(x => x.Password, y => y.MapFrom(p => p.NewPassword))
                .ReverseMap();


            CreateMap<RoleShowVM, Role>()
                .ReverseMap();
            CreateMap<RoleCreateVM, Role>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ReverseMap();
            CreateMap<RoleEditVM, Role>()
                .ReverseMap();


            CreateMap<Problem, ProblemShowVM>()
                .ReverseMap();
            CreateMap<ProblemCreateVM, Problem>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.isSolved, y => y.Ignore())
                .ForPath(x => x.Priority, y => y.Ignore())
                .ForPath(x => x.Severity, y => y.Ignore())
                .ForPath(x => x.ProblemCategory, y => y.Ignore())
                .ReverseMap();
            CreateMap<ProblemEditVM, Problem>()
                .ForPath(x => x.Priority, y => y.Ignore())
                .ForPath(x => x.Severity, y => y.Ignore())
                .ForPath(x => x.ProblemCategory, y => y.Ignore())
                .ReverseMap();
            CreateMap<Problem, ProblemSolvedVM>()
                .ReverseMap();

        }
    }
}
