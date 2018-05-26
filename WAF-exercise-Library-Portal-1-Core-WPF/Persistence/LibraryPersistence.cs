using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_WPF.Persistence
{
    public class LibraryPersistence : ILibraryPersistence
    {
        private HttpClient _client;

        public LibraryPersistence(String baseAddress)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<Boolean> LoginAsync(LoginData user)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Login", user);
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }

        public async Task<Boolean> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/Account/Logout");
                return !response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                
                throw new PersistenceUnavailableException(exception);
            }
        }

        public async Task<IEnumerable<BookData>> ReadBooksAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/Books");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<BookData> books = await response.Content.ReadAsAsync<IEnumerable<BookData>>();

                    foreach (BookData book in books)
                    {
                        response = await _client.GetAsync("api/Authors/BookId/" + book.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            book.Authors = (await response.Content.ReadAsAsync<IEnumerable<AuthorData>>()).ToList();
                        }

                        response = await _client.GetAsync("api/Volumes/BookId/" + book.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            book.Volumes = (await response.Content.ReadAsAsync<IEnumerable<VolumeData>>()).ToList();
                        }
                    }

                    return books;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }

        public async Task<Boolean> CreateBuildingAsync(BookData bookData)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Books", bookData);
                bookData.Id = (await response.Content.ReadAsAsync<Int32>());
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }

        public Task<Boolean> UpdateBuildingAsync(BookData bookData)
        {
            throw new NotImplementedException();
        }

        public Task<Boolean> DeleteBuildingAsync(BookData bookData)
        {
            throw new NotImplementedException();
        }
    }
}
