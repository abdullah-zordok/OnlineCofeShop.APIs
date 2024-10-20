using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductsSpecs;

namespace Talabat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public Task<IReadOnlyList<Product?>> GetProductsAsync(ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);
            var products =  _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            return products;
        }
        public Task<Product?> GetProductAsync(int productId)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(productId);
            var product = _unitOfWork.Repository<Product>().GetByEntityWithSpecAsync(spec);
            return product;
        }

       
        public async Task<int> GetCountAsync(ProductSpecParams specParams)
        {
            var countspec=new ProductWithFilterationForCountSpecifications(specParams);
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countspec);
            return count;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
          => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
        => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();

     
       
    }
}
