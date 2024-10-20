using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.ProductsSpecs;

namespace Talabat.Core.Services.Contract
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product?>> GetProductsAsync(ProductSpecParams specParams);

        //Retrieve a single product based on its productId,so not use there IReadOnlyList<Product>
        Task<Product?> GetProductAsync(int productId);

        Task<int> GetCountAsync(ProductSpecParams specParams);

        //Retrieve all Brands
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();

        Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();

    }
}
