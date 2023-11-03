using Xunit;
using FC.Codeflix.Category.Domain.Exceptions;
using DomainEntity = FC.Codeflix.Category.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        // Arrange
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        // Act
        var dateBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description);

        var dateAfter = DateTime.Now;

        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateBefore);
        Assert.True(category.CreatedAt < dateAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        // Arrange
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        // Act
        var dateBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);

        var dateAfter = DateTime.Now;

        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateBefore);
        Assert.True(category.CreatedAt < dateAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")] 
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Category name", null!);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameLessThan3Characteres))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ac")]
    public void InstantiateErrorWhenNameLessThan3Characteres(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should not be less than 3 characteres", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameGreaterThan255Characteres))]
    [Trait("Domain", "Category - Aggregates")]    
    public void InstantiateErrorWhenNameGreaterThan255Characteres()
    {
        var invalidName = string.Join(
            null, Enumerable.Range(0, 256).Select(_ => 'a').ToArray()
        );

        Action action = () => new DomainEntity.Category(invalidName, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should not be greater than 255 characteres", exception.Message);
    }
}
