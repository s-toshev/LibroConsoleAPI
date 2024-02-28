using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibroConsoleAPI.IntegrationTests
{
    public class _AddBookIntegrationTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManagerFixture _fixture;
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public _AddBookIntegrationTests()
        {
            _fixture = new BookManagerFixture();
            _bookManager = _fixture.BookManager;
            _dbContext = _fixture.DbContext;
        }

        [Fact]
        public async Task AddBookAsync_ShouldAddBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Single(_dbContext.Books);
            Assert.Equal("Test Book", bookInDb.Title);
            Assert.Equal("John Doe", bookInDb.Author);
        }

        //Arrange


        //Act


        //Assert



        [Fact]
        public async Task AddBookAsync_TryToAddBookWithInvalidTitle_ShouldThrowException()
        {
            //Arrange
            var invalidBook = new Book
            {
                Title = new string('A', 256),
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            //Act
            string exceptionMessage = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(invalidBook)).Result.Message;

            //Assert
            Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(invalidBook));
            Assert.Equal("Book is invalid.", exceptionMessage);
            Assert.Empty(_dbContext.Books);


        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithInvalidAuthor_ShouldThrowException()
        {
            //Arrange
            var invalidBook = new Book
            {
                Title = "The Title",
                Author = new string('k', 101),
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            //Act
            string exceptionMessage = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(invalidBook)).Result.Message;

            //Assert
            Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(invalidBook));
            Assert.Equal("Book is invalid.", exceptionMessage);
            Assert.Empty(_dbContext.Books);


        }


       

    }
}
