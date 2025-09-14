using System;
using System.Threading.Tasks;
using AbpDemo.Books;
using AbpDemo.Products;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace AbpDemo;

public class AbpDemoDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Book, Guid> _bookRepository;
    private readonly IRepository<Product, Guid> _productRepository;

    public AbpDemoDataSeederContributor(IRepository<Book, Guid> bookRepository, IRepository<Product,Guid> productRepository)
    {
        _bookRepository = bookRepository;
        _productRepository = productRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _bookRepository.GetCountAsync() <= 0)
        {
            await _bookRepository.InsertAsync(
                new Book
                {
                    Name = "1984",
                    Type = BookType.Dystopia,
                    PublishDate = new DateTime(1949, 6, 8),
                    Price = 19.84f
                },
                autoSave: true
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    Name = "The Hitchhiker's Guide to the Galaxy",
                    Type = BookType.ScienceFiction,
                    PublishDate = new DateTime(1995, 9, 27),
                    Price = 42.0f
                },
                autoSave: true
            );
        }
        if (await _productRepository.GetCountAsync() <= 0)
        {
            await _productRepository.InsertAsync(
                new Product
                {
                    Name = "Laptop",
                    Price = 999.99m
                },
                autoSave: true
            );
            await _productRepository.InsertAsync(
                new Product
                {
                    Name = "Smartphone",                    
                    Price = 699.99m
                },
                autoSave: true
            );
        }
    }
}