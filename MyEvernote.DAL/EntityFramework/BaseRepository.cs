using MyEvernote.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DAL.EntityFramework
{
    public class BaseRepository
    {
        protected static MyEvernoteDbContext context;
        private static object _lockSync = new object();
        protected BaseRepository()
        {
            CreateContext();
        }

        private static void CreateContext()
        {
            //Singeleton
            if (context == null)
            {
                lock(_lockSync)//Multithread uygulamalarda birden fazla thread girip dbContext oluşturmaması için lock
                {
                    if (context == null)
                        context = new MyEvernoteDbContext();
                }
            }
        }
    }
}
