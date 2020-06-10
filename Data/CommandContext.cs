using CommandApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandApi.Data
{
    public class CommandContext : DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> opt) : base(opt)     
        {

        }
        public DbSet<Command> Commands { get; set; }
    }
}
