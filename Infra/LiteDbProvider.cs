using OwnAspNetCore.Services;
using OwnAspNetCore.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OwnAspNetCore.Infra
{
    public class LiteDbProvider : IDatabase
    {
        private const string _dbName = "data.db";
        private LiteDatabase _db;

        public LiteDbProvider()
        {
            var mapper = BsonMapper.Global;
                mapper.Entity<IDbEntry>()
                    .Id(x => x.Id, true);

            _db = new LiteDatabase(_dbName, mapper);
        }

        public string Path => _dbName;

        public bool Contains<T>(int id) where T : IDbEntry
        {
            return GetCollection<T>().Exists(x => x.Id == id);
        }

        public bool Delete<T>(int id) where T : IDbEntry
        {
            return GetCollection<T>().Delete(id);
        }

        public T Get<T>(int id) where T : IDbEntry
        {
            return GetCollection<T>().FindById(id);
        }

        public IEnumerable<T> GetTable<T>() where T : IDbEntry
        {
            return GetCollection<T>().FindAll();
        }

        public int Insert<T>(T newItem) where T : IDbEntry
        {
            return GetCollection<T>().Insert(newItem);
        }

        public bool Update<T>(T updatedItem) where T : IDbEntry
        {
            return GetCollection<T>().Update(updatedItem);
        }

        private string GetCollectionName<T>()
        {
            return typeof(T).Name.ToUpper();
        }
        private LiteCollection<T> GetCollection<T>()
            where T : IDbEntry
        {
            return _db.GetCollection<T>(GetCollectionName<T>());
        }

        public IEnumerable<T> Search<T>(Expression<Func<T, bool>> predicate) where T : IDbEntry
        {
            return GetCollection<T>().Find(predicate);
        }
    }
}