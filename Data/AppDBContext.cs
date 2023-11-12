using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using testGRPC.Models;

namespace testGRPC.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
        public DbSet<ToDoItem> toDoItems => Set<ToDoItem>();
    }
}