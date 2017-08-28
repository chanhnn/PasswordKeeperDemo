using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace PasswordKeeperDemo.Helpers
{
    public class AbstractDao<T, TId> : IGenericDao<T, TId> where T : new()
    {
        protected readonly SQLiteAsyncConnection Database;

        public AbstractDao()
        {
            Database = App.GetCurrentConnection();
        }

        public Task<List<T>> GetItemsAsync()
        {
            return Database.Table<T>().ToListAsync();
        }

        public Task<T> GetItemAsync(TId id)
        {
            return Database.GetAsync<T>(id);
        }

        public Task<int> InsertItemAsync(T item)
        {
            return Database.InsertAsync(item);
        }

        public Task<int> UpdateItemAsync(T item)
        {
            return Database.UpdateAsync(item);
        }

        public Task<int> DeleteItemAsync(T item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
