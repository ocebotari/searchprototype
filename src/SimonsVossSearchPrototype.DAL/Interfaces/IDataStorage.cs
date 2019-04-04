using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Interfaces
{
    /// <summary>
    /// Json data storage
    /// </summary>
    public interface IDataStorage: IDisposable
    {
        /// <summary>
        /// Get collection
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="name">Collection name</param>
        /// <returns>Typed IRepositoryCollection</returns>
        IRepositoryCollection<T> GetCollection<T>(string name = null) where T : class;

        /// <summary>
        /// Get single item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="key">Item key</param>
        /// <returns>Typed item</returns>
        T GetItem<T>(string key);
    }
}
