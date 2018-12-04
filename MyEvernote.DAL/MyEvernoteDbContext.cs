using MyEvernote.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DAL
{
    public class MyEvernoteDbContext : DbContext
    {
        public DbSet<EvernoteUser> EvernoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likeds { get; set; }

        public MyEvernoteDbContext()
        {
            Database.SetInitializer(new MyInitializer());
        }
    }
}
