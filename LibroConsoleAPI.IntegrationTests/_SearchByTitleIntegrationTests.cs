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
    public class _SearchByTitleIntegrationTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManagerFixture _fixture;
        private readonly TestLibroDbContext _dbContext;
        private readonly BookManager _bookManager;

        public _SearchByTitleIntegrationTests()
        {
            _fixture = new BookManagerFixture();
            _dbContext = _fixture.DbContext;
            _bookManager = _fixture.BookManager;
        }


        [Fact]
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
        {
            //Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            //Act
            string validTitleFragment = "The God";

            List<string> expectedTitles = new List<string>()
            {
                "The God of Cows",
                "The God of Small Things"
             };


            var result = await _bookManager.SearchByTitleAsync(validTitleFragment);

            //Assert
            Assert.Equal(2, result.Count());


            foreach (var item in result)
            {
                Assert.Contains(validTitleFragment, item.Title);
                Assert.Contains(item.Title, expectedTitles);
                
            }
        }

        [Fact]
        public async Task SearchByTitleAsync_WithInvalidTitleFragment_ShouldThrowKeyNotFoundException()
        {
            //Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);
            string invalidTitleFragment = "MixMixMix";

            //Act&Assert        
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(()=> _bookManager.SearchByTitleAsync(invalidTitleFragment));
            //"No books found with the given title fragment."
            Assert.Equal("No books found with the given title fragment.", exception.Message);
        }
    }
}
