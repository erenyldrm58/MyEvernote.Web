﻿using MyEvernote.Core.DataAccess;
using MyEvernote.DAL.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyEvernote.Business.Abstract
{
    public abstract class ManagerBase<T> : IDataAccess<T> where T : class
    {
        private Repository<T> repo = new Repository<T>();

        public virtual int Delete(T obj)
        {
            return repo.Delete(obj);
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return repo.Find(where);
        }

        public int Insert(T obj)
        {
            return repo.Insert(obj);
        }

        public List<T> List()
        {
            return repo.List();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return repo.List(where);
        }

        public IQueryable<T> ListQueryable()
        {
            return repo.ListQueryable();
        }

        public int Save()
        {
            return repo.Save();
        }

        public int Update(T obj)
        {
            return repo.Update(obj);
        }
    }
}
