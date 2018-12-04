using MyEvernote.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Business
{
    public class Test
    {
        public Test()
        {
            MyEvernoteDbContext db = new MyEvernoteDbContext();
            db.Categories.ToList();
        }
    }
}