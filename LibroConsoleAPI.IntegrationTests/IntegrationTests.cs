using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibroConsoleAPI.IntegrationTests
{
    public class IntegrationTests : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public IntegrationTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
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
            Assert.Equal("Test Book", bookInDb.Title);
            Assert.Equal("John Doe", bookInDb.Author);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var invalidBook = new Book
            {
                Title = new string('A', 256),
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 116,
                Price = 29.99
            };
            // Act
            var exeption = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(invalidBook));

            // Assert
            Assert.Equal("Book is invalid.", exeption.Result.Message);
        }

        [Fact]
        public async Task DeleteBookAsync_WithValidISBN_ShouldRemoveBookFromDb()
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

            var secondBook = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "6786786786713",
                YearPublished = 2023,
                Genre = "No genre",
                Pages = 154,
                Price = 33.99
            };
            await _bookManager.AddAsync(newBook);
            await _bookManager.AddAsync(secondBook);


            // Act
            await _bookManager.DeleteAsync(newBook.ISBN);

            // Assert
            var bookInDb = _dbContext.Books.ToList();
            Assert.Single(bookInDb);
            Assert.Equal("Test Book", bookInDb[0].Title);
            Assert.Equal("6786786786713", bookInDb[0].ISBN);

        }


        [Fact]
        public async Task DeleteBookAsync_TryToDeleteWithNullOrWhiteSpaceISBN_ShouldThrowException()
        {
            // Arrange
            var nullBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            var whiteSpaceBook = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "  ",
                YearPublished = 2023,
                Genre = "No genre",
                Pages = 154,
                Price = 33.99
            };

            //Act
            var nullExeption = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(nullBook));

            //Assert
            Assert.Equal("Book is invalid.", nullExeption.Result.Message);

            //Act
            var whiteSpaceExeption = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(whiteSpaceBook));


            //Assert
            Assert.Equal("Book is invalid.", whiteSpaceExeption.Result.Message);


        }

        [Fact]
        public async Task GetAllAsync_WhenBooksExist_ShouldReturnAllBooks()
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

            var secondBook = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "6786786786713",
                YearPublished = 2023,
                Genre = "No genre",
                Pages = 154,
                Price = 33.99
            };
            await _bookManager.AddAsync(newBook);
            await _bookManager.AddAsync(secondBook);


            //Act
            var result = await _bookManager.GetAllAsync();

            //Assert
            Assert.Equal(2, result.Count());

        }

        [Fact]
        public async Task GetAllAsync_WhenNoBooksExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange


            //Act
            var exeption = Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.GetAllAsync());


            //Assert
            Assert.Equal("No books found.", exeption.Result.Message);

        }

        [Fact]
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            var secondBook = new Book
            {
                Title = "Test",
                Author = "Test Author",
                ISBN = "6786786786713",
                YearPublished = 2023,
                Genre = "No genre",
                Pages = 154,
                Price = 33.99
            };
            await _bookManager.AddAsync(newBook);
            await _bookManager.AddAsync(secondBook);

            // Act
            var result = await _bookManager.SearchByTitleAsync("Book");
            // Assert
            Assert.Equal(newBook.Title, result.FirstOrDefault().Title);
        }

        [Fact]
        public async Task SearchByTitleAsync_WithInvalidTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            var secondBook = new Book
            {
                Title = "Test",
                Author = "Test Author",
                ISBN = "6786786786713",
                YearPublished = 2023,
                Genre = "No genre",
                Pages = 154,
                Price = 33.99
            };
            await _bookManager.AddAsync(newBook);
            await _bookManager.AddAsync(secondBook);

            // Act

            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.SearchByTitleAsync("Wrong Title"));

            // Assert
            Assert.Equal("No books found with the given title fragment.", exception.Result.Message);
        }

        [Fact]
        public async Task GetSpecificAsync_WithValidIsbn_ShouldReturnBook()
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
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal("Test Book", bookInDb.Title);
            Assert.Equal("John Doe", bookInDb.Author);
            Assert.Equal("1234567890123", bookInDb.ISBN);
        }

        [Fact]
        public async Task GetSpecificAsync_WithInvalidIsbn_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var invalidBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "12321",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 116,
                Price = 29.99
            };
            // Act
            var exeption = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(invalidBook));

            // Assert
            Assert.Equal("Book is invalid.", exeption.Result.Message);
            ////good way to additionally test if everything is correct
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == invalidBook.ISBN);
            Assert.Null(bookInDb);
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
            await _bookManager.AddAsync(newBook);

            newBook.Title = "Updated Title";
            newBook.Author = "upd author";
            newBook.ISBN = "1122334455667";

            // Act
            await _bookManager.UpdateAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.NotNull(bookInDb);
            Assert.Equal("Updated Title", bookInDb.Title);
            Assert.Equal("upd author", bookInDb.Author);
            Assert.Equal("1122334455667", bookInDb.ISBN);

        }

        [Fact]
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
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
            await _bookManager.AddAsync(newBook);

            newBook.Title = new string('a', 260);
            newBook.Author = "";
            newBook.ISBN = "133";

            // Act

            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.UpdateAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
        }

    }
}
