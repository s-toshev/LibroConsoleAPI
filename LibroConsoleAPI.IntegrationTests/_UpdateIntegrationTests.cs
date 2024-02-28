using LibroConsoleAPI.Business.Contracts;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }


        [Fact]
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
        {
            throw new NotImplementedException();
        }

    }
}
