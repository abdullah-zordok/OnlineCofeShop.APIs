using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductsSpecs;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseApiController
    {

        //private readonly IGenericRepository<Product> _productsRepo;
        //private readonly IGenericRepository<ProductBrand> _brandsRepo;
        //private readonly IGenericRepository<ProductCategory> _categoriesRepo;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(/*IGenericRepository<Product> productsRepo,IGenericRepository<ProductBrand> brandsRepo,IGenericRepository<ProductCategory> categoriesRepo,*/
            IMapper mapper,IProductService productService)
        {
            //  _productsRepo = productsRepo;
            //  _brandsRepo = brandsRepo;
            //_categoriesRepo = categoriesRepo;
            _mapper = mapper;
            _productService = productService;
        }
        //[Authorize(AuthenticationSchemes ="")] but iput it in  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


         [HttpGet]// /api/Products
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)
            {
            var products=await _productService.GetProductsAsync(specParams);
            var count = await _productService.GetCountAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize,count,data));
            }
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status404NotFound)]

        [HttpGet("{id}")] // /api/Products/1
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product is null)

                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }
        [HttpGet("brands")]// /api/Products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands= await _productService.GetBrandsAsync(); //we dont need with spec because it has no navigational property or we dont want to include another 
            return Ok(brands);
        }
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories= await _productService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}
