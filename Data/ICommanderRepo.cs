using System.Collections.Generic;
using CommandApi.Models;

namespace CommandApi.Data
{
    public interface ICommanderRepo
    {
        bool SaveChanges();
        IEnumerable<Command> GetAllCommands(); //Read - GET Endpoint
        Command GetCommandById(int id); //Read - GET Endpoint
        void CreateCommand(Command cmd);//Insert - POST Endpoint
        void UpdateCommand(Command cmd);//Update - PUT/PATCH Endpoint
        void DeleteCommand(Command cmd);//Delete - DELETE Endpoint

    }

}