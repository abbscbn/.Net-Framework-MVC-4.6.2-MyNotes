using MyNotes.Common;
using MyNotes.Core.DataAccess;
using MyNotes.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MyNotes.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class
    {

        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = context.Set<T>();
        }
        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if (obj is MyEntityBase)
            {

                MyEntityBase o = obj as MyEntityBase;

                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifedOn = now;
                o.ModifiedUsername = App.Common.getCurrentUsername(); // Şu anki kullanıcı adı
            }

            return Save();

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

        public int Count(Expression<Func<T, bool>> where)
        {
            return _objectSet.Count(where);
        }

        public int Save()
        {
            return context.SaveChanges();

        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {

                MyEntityBase o = obj as MyEntityBase;

                DateTime now = DateTime.Now;


                o.ModifedOn = now;
                o.ModifiedUsername = App.Common.getCurrentUsername(); // Şu anki kullanıcı adı
            }

            return Save();
        }
    }
}
