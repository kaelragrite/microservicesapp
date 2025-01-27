﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.ApiCollection.Interfaces
{
    public interface ICatalogApi
    {
        Task<IEnumerable<CatalogModel>> GetCatalogs();
        
        Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
        
        Task<CatalogModel> GetCatalog(string id);

        Task<CatalogModel> CreateCatalog(CatalogModel model);
    }
}