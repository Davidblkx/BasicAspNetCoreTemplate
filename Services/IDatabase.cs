using OwnAspNetCore.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace OwnAspNetCore.Services
{
    public interface IDatabase
    {
        string Path { get; }

        bool Contains<T>(int id) where T : IDbEntry;
        T Get<T>(int id) where T : IDbEntry;
        IEnumerable<T> GetTable<T>() where T : IDbEntry;
        IEnumerable<T> Search<T>(Expression<Func<T, bool>> predicate) where T : IDbEntry;

        int Insert<T>(T newItem) where T : IDbEntry;
        bool Update<T>(T updatedItem) where T : IDbEntry;

        bool Delete<T>(int id) where T : IDbEntry;
    }
}