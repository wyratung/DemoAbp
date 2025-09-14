using System;
using System.Linq;
using System.Threading.Tasks;
using AbpDemo.Products;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace AbpDemo.Books;

public abstract class DemoAppService_Tests<TStartupModule> : AbpDemoApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IBookAppService _bookAppService;
    private readonly IProductAppService _productAppService;

    protected DemoAppService_Tests()
    {
        _bookAppService = GetRequiredService<IBookAppService>();
        _productAppService = GetRequiredService<IProductAppService>();
    }
    #region Books
    [Fact]
    public async Task Should_Get_List_Of_Books()
    {
        //Act
        var result = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );

        //Assert
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(b => b.Name == "1984");
    }

    [Fact]
    public async Task Should_Create_A_Valid_Book()
    {
        //Act
        var result = await _bookAppService.CreateAsync(
            new CreateUpdateBookDto
            {
                Name = "New test book 42",
                Price = 10,
                PublishDate = DateTime.Now,
                Type = BookType.ScienceFiction
            }
        );

        //Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("New test book 42");
    }

    [Fact]
    public async Task Should_Not_Create_A_Book_Without_Name()
    {
        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _bookAppService.CreateAsync(
                new CreateUpdateBookDto
                {
                    Name = "",
                    Price = 10,
                    PublishDate = DateTime.Now,
                    Type = BookType.ScienceFiction
                }
            );
        });

        exception.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    // create a test to update a book
    public async Task Should_Update_A_Book()
    {
        //Arrange
        var listResult = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );
        var book = listResult.Items.First();
        //Act
        var result = await _bookAppService.UpdateAsync(
            book.Id,
            new CreateUpdateBookDto
            {
                Name = "Updated book name",
                Price = book.Price + 1,
                PublishDate = book.PublishDate,
                Type = book.Type
            }
        );
        //Assert
        result.Id.ShouldBe(book.Id);
        result.Name.ShouldBe("Updated book name");
        result.Price.ShouldBe(book.Price + 1);
    }

    [Fact]
    public async Task Should_Delete_A_Book()
    {
        //Arrange
        var listResult = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );
        var book = listResult.Items.First();
        //Act
        await _bookAppService.DeleteAsync(book.Id);
        //Assert
        var getListResult = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );
        getListResult.Items.ShouldNotContain(b => b.Id == book.Id);
    }

    [Fact]
    // delete books without looping
    public async Task Should_Delete_Books_Without_Looping()
    {
        //Arrange
        var listResult = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );
        var bookIds = listResult.Items.Select(b => b.Id).ToList();
        //Act
        await Task.WhenAll(bookIds.Select(id => _bookAppService.DeleteAsync(id)));
        //Assert
        var getListResult = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );
        getListResult.Items.ShouldBeEmpty();
    }
    [Fact]
    // create books with looping
    public async Task Should_Create_Books_With_Looping()
    {
        //Arrange
        var booksToCreate = new[]
        {
            new CreateUpdateBookDto
            {
                Name = "Book 1",
                Price = 10,
                PublishDate = DateTime.Now,
                Type = BookType.ScienceFiction
            },
            new CreateUpdateBookDto
            {
                Name = "Book 2",
                Price = 15,
                PublishDate = DateTime.Now,
                Type = BookType.Fantastic
            },
            new CreateUpdateBookDto
            {
                Name = "Book 3",
                Price = 20,
                PublishDate = DateTime.Now,
                Type = BookType.Adventure
            }
        };
        //Act
        foreach (var book in booksToCreate)
        {
            await _bookAppService.CreateAsync(book);
        }
        //Assert
        var getListResult = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );
        getListResult.Items.Count.ShouldBeGreaterThanOrEqualTo(3);
        getListResult.Items.ShouldContain(b => b.Name == "Book 1");
        getListResult.Items.ShouldContain(b => b.Name == "Book 2");
        getListResult.Items.ShouldContain(b => b.Name == "Book 3");

    }
    #endregion

    #region Product 
    // Test CreateAsync
    [Fact]
    public async Task Should_Create_A_Valid_Product()
    {
        //Act
        var result = await _productAppService.CreateAsync(
            new CreateUpdateProductDto
            {
                Name = "New test product 42",
                Price = 10,
            }
        );
        //Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("New test product 42");
    }

    // Test GetListAsync
    [Fact]
    public async Task Should_Get_List_Of_Products()
    {
        //Act
        var result = await _productAppService.GetListAsync(
            new Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto()
        );
        //Assert
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(b => b.Name == "Laptop");
    }
    [Fact]
    // Test GetAsync 
    public async Task Should_Get_A_Product()
    {
        // Arrange
        var createdProduct = await _productAppService.CreateAsync(
            new CreateUpdateProductDto
            {
                Name = "Test Product",
                Price = 20,
            }
        );
        // Act
        var fetchedProduct = await _productAppService.GetAsync(createdProduct.Id);
        // Assert
        fetchedProduct.ShouldNotBeNull();
        fetchedProduct.Id.ShouldBe(createdProduct.Id);
        fetchedProduct.Name.ShouldBe("Test Product");
    }

    // Test UpdateAsync
    [Fact]
    public async Task Should_Update_A_Product()
    {
        // Arrange
        var createdProduct = await _productAppService.CreateAsync(
            new CreateUpdateProductDto
            {
                Name = "Test Product",
                Price = 20,
            }
        );
        // Act
        var updatedProduct = await _productAppService.UpdateAsync(createdProduct.Id,
            new CreateUpdateProductDto
            {
                Name = "Updated Product",
                Price = 30,
            }
        );
        // Assert
        updatedProduct.ShouldNotBeNull();
        updatedProduct.Id.ShouldBe(createdProduct.Id);
        updatedProduct.Name.ShouldBe("Updated Product");
        updatedProduct.Price.ShouldBe(30);
    }

    // Test DeleteAsync
    [Fact]
    public async Task Should_Delete_A_Product()
    {
        //Arrange
        var listResult = await _productAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );
        var product = listResult.Items.First();
        //Act
        await _productAppService.DeleteAsync(product.Id);
        //Assert
        var getListResult = await _productAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );
        getListResult.Items.ShouldNotContain(b => b.Id == product.Id);

    }
    #endregion
}