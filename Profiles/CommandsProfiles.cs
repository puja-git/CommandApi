using AutoMapper;
using CommandApi.Dtos;
using CommandApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandApi.Profiles
{
    public class CommandsProfiles : Profile
    {
        public CommandsProfiles()
        {
            //Source -> Destination 
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDTO,Command>();
            CreateMap<CommandUpdateDto, Command>();
            CreateMap<Command, CommandUpdateDto>();
        }
    }
}
