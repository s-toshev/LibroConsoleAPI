using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
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
        private readonly IBookManager _bookManager;

        public _SearchByTitleIntegrationTests()
        {
            _fixture = new BookManagerFixture();
            _dbContext = _fixture.DbContext;
            _bookManager = _fixture.BookManager;
        }


        [Fact]
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public async Task SearchByTitleAsync_WithInvalidTitleFragment_ShouldThrowKeyNotFoundException()
        {
            throw new NotImplementedException();
        }


    }
}
