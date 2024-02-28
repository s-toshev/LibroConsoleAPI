using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibroConsoleAPI.IntegrationTests
{
    public class _DeleteBookIntegrationTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManagerFixture _fixture;
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public _DeleteBookIntegrationTests()
        {
            _fixture = new BookManagerFixture();
            _bookManager = _fixture.BookManager;
            _dbContext = _fixture.DbContext;
        }




        [Fact]
        public async Task DeleteBookAsync_WithValidISBN_ShouldRemoveBookFromDb()
        {
            //Arrange
            var newBook = new Book()
            {
                Title = "TheBook",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            //Act
            await _bookManager.AddAsync(newBook);
            Assert.Single(_dbContext.Books);
            Assert.Equal("TheBook", newBook.Title);
            Assert.Equal("1234567890123", newBook.ISBN);

            await _bookManager.DeleteAsync(newBook.ISBN);

            var deletedBook = _dbContext.Books.FirstOrDefault();
            //Assert

            Assert.Empty(_dbContext.Books);
            Assert.Null(deletedBook);



        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task DeleteBookAsync_TryToDeleteWithNullOrWhiteSpaceISBN_ShouldThrowException(string invalidISBN)
        {
            //Arrange
            var newBook = new Book()
            {
                Title = "TheBook",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            //Act
            await _bookManager.AddAsync(newBook);
            Assert.NotEmpty(_dbContext.Books);
            Assert.Equal("TheBook", newBook.Title);
            Assert.Equal("1234567890123", newBook.ISBN);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _bookManager.DeleteAsync(invalidISBN));


            //Assert        
            Assert.Equal("ISBN cannot be empty.", exception.Message);

        }
    }
}
