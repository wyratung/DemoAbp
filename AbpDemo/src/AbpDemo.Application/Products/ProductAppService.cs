using AbpDemo.Products;
using AbpDemo.Permissions;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Application.Services;
using System.Linq.Dynamic.Core;

namespace AbpDemo.Products
{
    [Authorize(AbpDemoPermissions.Products.Default)]
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> _repository;

        public ProductAppService(IRepository<Product, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            var Product = await _repository.GetAsync(id);
            return ObjectMapper.Map<Product, ProductDto>(Product);
        }

        public async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _repository.GetQueryableAsync();
            var query = queryable
                .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Name" : input.Sorting)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);


            var Products = await AsyncExecuter.ToListAsync(query);
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            return new PagedResultDto<ProductDto>(
                totalCount,
                ObjectMapper.Map<List<Product>, List<ProductDto>>(Products)
            );
        }

        [Authorize(AbpDemoPermissions.Products.Create)]
        public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            var Product = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);
            await _repository.InsertAsync(Product);
            return ObjectMapper.Map<Product, ProductDto>(Product);
        }

        [Authorize(AbpDemoPermissions.Products.Edit)]
        public async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
        {
            var Product = await _repository.GetAsync(id);
            ObjectMapper.Map(input, Product);
            await _repository.UpdateAsync(Product);
            return ObjectMapper.Map<Product, ProductDto>(Product);
        }

        [Authorize(Roles = "Manager")]
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}
