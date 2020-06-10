
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using CommandApi.Classes;
using CommandApi.Data;
using CommandApi.Dtos;
using CommandApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CommandApi.Controllers
{
    [Route("api/command")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands([FromQuery] QueryParameters queryParameters)
        {
            var commandItems = _repository.GetAllCommands();

            IQueryable<Command> commands = commandItems.AsQueryable();
            //sort
            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if (typeof(Command).GetProperty(queryParameters.SortBy) != null)
                {
                    commandItems = commands.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                }
            }
            //search
            if(!string.IsNullOrEmpty(queryParameters.HowTo))
            {
                commandItems = commandItems.Where(c => c.HowTo.ToLower().Contains(queryParameters.HowTo.ToLower()));
            }
            //paging
            commandItems = commandItems
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id) //id comes from the request url
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }
            else
                return NotFound();


        }

        //POST /api/commands
        [HttpPost]
        //return CommandReadDto object
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDTO commandCreataeDTO)
        {
            var commandModel = _mapper.Map<Command>(commandCreataeDTO);//Command is Destination and source is CreateDTO
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);//destination CommandReadDto and source CreateDto

            //return Ok(commandReadDto);//status code = 200
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);

        }

        //Put api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(commandUpdateDto, commandModelFromRepo);
            _repository.UpdateCommand(commandModelFromRepo);
            _repository.SaveChanges();
            return NoContent();

        }

        //PATCH api/command/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);//return Command Object
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo); //Map Command object to  CommandUpdateDTO and 
            patchDoc.ApplyTo(commandToPatch, ModelState);//apply the Patch to the CommandUpdateDto object

            //Validation check
            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }
        //Delete api/command/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);//return Command Object
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();

        }
    }
}
