using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureUrlResolver:IValueResolver<Product,ProductToReturnDto,string>
    {
        private readonly IConfiguration configuration;

        public ProductPictureUrlResolver(IConfiguration configuration) {
            this.configuration = configuration;
        }
        public string Resolve(Product source,ProductToReturnDto destination, string destMember,ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
                return $"{configuration["ApiBaseUrl"]}/{source.PictureUrl}";
            return string.Empty;
        }

    }
}
