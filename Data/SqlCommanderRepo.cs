﻿using CommandApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandApi.Data
{
    public class SqlCommanderRepo : ICommanderRepo
    {
        private readonly CommandContext _context;
        public SqlCommanderRepo(CommandContext context)
        {
            _context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if(cmd==null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
             _context.Commands.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _context.Commands.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            //IQueryable<Command> commands = _context.Commands;
            return _context.Commands.ToList();
        }

        public Command GetCommandById(int id)
        {
           return _context.Commands.FirstOrDefault(p => p.Id ==id);
        }

        public bool SaveChanges()
        {
           return (_context.SaveChanges() >= 0);

        }

        public void UpdateCommand(Command cmd)
        {
            //_context.Update(cmd);
        }
    }
}
