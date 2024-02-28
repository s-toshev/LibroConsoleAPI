using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class _GetAllIntegrationTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManagerFixture _fixture;
        private readonly TestLibroDbContext _dbContext;
        private readonly BookManager _bookManager;

        public _GetAllIntegrationTests()
        {
            _fixture = new BookManagerFixture();
            _dbContext = _fixture.DbContext;
            _bookManager = _fixture.BookManager;

        }

        //Arrange

        //Act

        //Assert

        [Fact]
        public async Task GetAllAsync_WhenBooksExist_ShouldReturnAllBooks()
        {

            //Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            //Act
            var allBooks = await _bookManager.GetAllAsync();

            //Assert
            Assert.True(_dbContext.Books.Count() == 10);
            Assert.Equal(allBooks.Count(), 10);

            foreach (var book in allBooks)
            {
                Assert.Contains(_dbContext.Books, b => b.Id == book.Id);
                Assert.Contains(_dbContext.Books, b => b.ISBN == book.ISBN);
                Assert.Contains(_dbContext.Books, b => b.Title == book.Title);
                Assert.Contains(_dbContext.Books, b => b.Pages == book.Pages);
                Assert.Contains(_dbContext.Books, b => b.Price == book.Price);
                Assert.Contains(_dbContext.Books, b => b.Genre == book.Genre);
                Assert.Contains(_dbContext.Books, b => b.YearPublished == book.YearPublished);

            }

        }


        [Fact]
        public async Task GetAllAsync_WhenNoBooksExist_ShouldThrowKeyNotFoundException()
        {

            //Act&Assert        
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.GetAllAsync());
            Assert.Equal("No books found.", exception.Message);

        }
    }
}
