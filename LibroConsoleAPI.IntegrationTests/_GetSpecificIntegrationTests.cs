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
    public class _GetSpecificIntegrationTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManagerFixture _fixture;
        private readonly TestLibroDbContext _dbContext;
        private readonly BookManager _bookManager;

        public _GetSpecificIntegrationTests()
        {
            _fixture = new BookManagerFixture();
            _dbContext = _fixture.DbContext;
            _bookManager = _fixture.BookManager;
        }
        //Arrange

        //Act

        //Assert

        [Fact]
        public async Task GetSpecificAsync_WithValidIsbn_ShouldReturnBook()
        {
            //Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            //Act
            string specificBookISBN = "9780330258644";
            string specificBookTitle = "The Catcher in the Rye";

            var specificBook = await _bookManager.GetSpecificAsync(specificBookISBN);

            //Assert
            Assert.Equal(10, _dbContext.Books.Count());

            Assert.Equal(specificBook.Title, specificBookTitle);
            Assert.Equal(specificBook.ISBN, specificBookISBN);
        }


        [Fact]
        public async Task GetSpecificAsync_WithInvalidIsbn_ShouldThrowKeyNotFoundException()
        {
            //Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            //Act
            string invalidSBN = "MIX9780330258644MIX";


            //Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(()=> _bookManager.GetSpecificAsync(invalidSBN));
            Assert.Equal($"No book found with ISBN: {invalidSBN}", exception.Message);
        }


    }
}
