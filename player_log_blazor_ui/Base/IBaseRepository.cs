using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_blazor_ui.Base
{
    public interface IBaseRepository<T> where T: class
    {
        Task<T> Get(string url, int id);
        Task<List<T>> GetAll(string url);
        Task<bool> Create(string url, T item);
        Task<bool> Update(string url, T item, int id);
        Task<bool> Delete(string url, int id);
    }
}
