using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordKeeperDemo.Helpers
{
    public interface IGenericDao<T, TId>
    {
        Task<List<T>> GetItemsAsync();

        Task<T> GetItemAsync(TId id);

        Task<int> InsertItemAsync(T item);

        Task<int> UpdateItemAsync(T item);

        Task<int> DeleteItemAsync(T item);
    }
}
