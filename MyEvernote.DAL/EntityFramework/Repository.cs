using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MyEvernote.DAL.EntityFramework
{
    public class Repository<T> : BaseRepository, IDataAccess<T> where T : class
    {
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = context.Set<T>();
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if (obj is BaseEntity)
            {
                BaseEntity b = obj as BaseEntity;
                DateTime now = DateTime.Now;

                b.ModifiedOn = b.CreatedOn = now;
                b.ModifiedBy = App.common.GetCurrentUsername();
            }
            return Save();
        }
        public int Update(T obj)
        {
            if (obj is BaseEntity)
            {
                BaseEntity b = obj as BaseEntity;
                DateTime now = DateTime.Now;

                b.ModifiedOn = now;
                b.ModifiedBy = "system"; //TODO : işlem yapan kullanıcı adı yazılmalı
            }

            return Save();
        }
        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return context.SaveChanges();
        }
        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}
