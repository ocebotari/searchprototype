using SimonsVossSearchPrototype.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonsVossSearchPrototype.Services
{
    public abstract class BaseSearchService
    {
        protected readonly IDataStorage _dbStorage;

        protected BaseSearchService(IDataStorage dbStorage)
        {
            _dbStorage = dbStorage;
        }
    }
}