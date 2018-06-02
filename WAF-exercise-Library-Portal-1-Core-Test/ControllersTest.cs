using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using Xunit;

using WAF_exercise_Library_Portal_1_Core_Db.Models;
using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_Test
{
    public class ControllersTest : IClassFixture<ServerClientFixture>
    {
        private readonly ServerClientFixture _fixture;

        public ControllersTest(ServerClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Test_GetBooks_ReturnsAllBooks()
        {
            // Act
            var response = await _fixture.Client.GetAsync("api/Books");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Book>>(responseString);

            Assert.NotNull(responseObject);
            Assert.True(responseObject.Count() == _fixture.Context.Book.Count());
        }

        [Fact]
        public async void Test_PostBook_ShouldAddBook()
        {
            // Arrange
            BookData bookData = new BookData()
            {
                Title = "TestBookTitle",
                Cover = null,
                Isbn = 123456789,
                PublishedYear = 2000
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(bookData), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PostAsync("api/Books", content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.NotNull(_fixture.Context.Book.FirstOrDefault(
                b => b.Title == bookData.Title
                && b.CoverId == null
                && b.Isbn == bookData.Isbn
                && b.PublishedYear == bookData.PublishedYear));
        }

        [Fact]
        public async void Test_PutBook_ShouldUpdateBook()
        {
            // Arrange
            BookData bookData = new BookData()
            {
                Id = 1,
                Title = "TestBookTitle",
                Cover = null,
                Isbn = 123456789,
                PublishedYear = 2000
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(bookData), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PutAsync("api/Books", content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.NotNull(_fixture.Context.Book.FirstOrDefault(
                b => b.Id == bookData.Id
                  && b.Title == bookData.Title
                  && b.CoverId == null
                  && b.Isbn == bookData.Isbn
                  && b.PublishedYear == bookData.PublishedYear));
        }

        [Fact]
        public async void Test_DeleteBook_ShouldFailToDeleteBook()
        {
            // Act
            var response = await _fixture.Client.DeleteAsync("api/Books/1");

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed);

            Assert.NotNull(_fixture.Context.Book.FirstOrDefault(b => b.Id == 1));
        }

        [Fact]
        public async void Test_PutLending_ShouldUpdatendeLending()
        {
            // Arrange
            LendingData lendingData = new LendingData()
            {
                Id = 48,
                Active = 2,
                StartDate = DateTime.UtcNow
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(lendingData), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PutAsync("api/Lendings", content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.NotNull(_fixture.Context.Lending.FirstOrDefault(
                l => l.Id == lendingData.Id
                  && l.Active == lendingData.Active
                  && l.StartDate == lendingData.StartDate));
        }

        [Fact]
        public async void Test_DeleteLending_ShouldDeleteLending()
        {
            // Act
            var response = await _fixture.Client.DeleteAsync("api/Lendings/8");

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Null(_fixture.Context.Lending.FirstOrDefault(
                l => l.Id == 8));
        }
    }
}
