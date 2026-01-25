using MyNotes.Core.DataAccess;
using MyNotes.DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyNotes.BusinessLayer.Abstract
{
    public class ManagerBase<T> : IDataAccess<T> where T : class
    {
        private Repository<T> repo = new Repository<T>();

        public int Count(Expression<Func<T, bool>> where)
        {
            return repo.Count(where);
        }

        public int Delete(T obj)
        {
            return repo.Delete(obj);
        }

        public int ExecuteSql(string sql, params object[] parameters)
        {
            return repo.ExecuteSql(sql, parameters);
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
