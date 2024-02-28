using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class _UpdateIntegrationTests: IClassFixture<BookManagerFixture>
    {
        private readonly BookManagerFixture _fixture;
        private readonly TestLibroDbContext _dbContext;
        private readonly IBookManager _bookManager;

        public _UpdateIntegrationTests()
        {
            _fixture = new BookManagerFixture();
            _dbContext = _fixture.DbContext;
            _bookManager = _fixture.BookManager;
        }


        [Fact]
        public async Task UpdateAsync_WithValidBook_ShouldUpdateBook()
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
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Single(_dbContext.Books);
            Assert.Equal("Test Book", bookInDb.Title);
            Assert.Equal("John Doe", bookInDb.Author);

            newBook.Title = "Updated Title";

            await _bookManager.UpdateAsync(newBook);
            var updatedBookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);

            // Assert
            Assert.NotNull(bookInDb);
            Assert.Single(_dbContext.Books);
            Assert.Equal("Updated Title", updatedBookInDb.Title);
            Assert.Equal("John Doe", updatedBookInDb.Author);

        }


        [Fact]
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
        {
            // Arrange
            var invalidBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "invalidISBN",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };


            //Act&Assert
            var exception = Assert.ThrowsAsync<ValidationException>(()=> _bookManager.UpdateAsync(invalidBook));
            Assert.Equal("Book is invalid.", exception.Result.Message);
        }

    }
}
