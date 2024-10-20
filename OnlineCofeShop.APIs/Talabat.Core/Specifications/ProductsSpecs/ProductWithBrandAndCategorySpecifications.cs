using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductsSpecs
{
    public class ProductWithBrandAndCategorySpecifications :BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            : base(P=>
                        (string.IsNullOrEmpty(specParams.Search)|| P.Name.ToLower().Contains(specParams.Search))&&
                        (!specParams.brandId.HasValue||P.BrandId==specParams.brandId.Value) &&
                        (!specParams.categoryId.HasValue||P.CategoryId==specParams.categoryId.Value)

              ) 
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
            if (!string.IsNullOrEmpty(specParams.sort))
            {
                switch (specParams.sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }

            }
            else
                AddOrderBy(P => P.Name);
            //totalProducts =18 ~20
            //pagesize =5
            //pageindex=3
            ApplyPagination((specParams.PageIndex-1)*specParams.PageSize, specParams.PageSize);
            
        }
        public ProductWithBrandAndCategorySpecifications(int id)
            : base(P=>P.Id==id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
