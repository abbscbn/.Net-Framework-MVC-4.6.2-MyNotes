using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyNotes.DataAccessLayer.Abstract
{
    public interface IRepository<T>
    {
        int Insert(T obj);
        int Update(T obj);
        int Delete(T obj);
        int Save();

        List<T> List();

        IQueryable<T> ListQueryable();

        List<T> List(Expression<Func<T, bool>> where);

        T Find(Expression<Func<T, bool>> where);


    }
}
