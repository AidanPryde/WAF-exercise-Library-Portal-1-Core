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
                    return await response.Content.ReadAsAsync<IEnumerable<BookData>>();
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
        public async Task<Boolean> CreateBookAsync(BookData bookData)
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
        public async Task<Boolean> UpdateBookAsync(BookData bookData)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/Books/", bookData);
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }
        public async Task<Boolean> DeleteBookAsync(BookData bookData)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/Books/" + bookData.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }

        public async Task<IEnumerable<BookAuthorData>> ReadBookAuthorsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/BookAuthors");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<BookAuthorData>>();
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

        public Task<bool> AddBookAuthorAsync(BookAuthorData bookAuthorData)
        {
            throw new NotImplementedException();
        }
        public Task<bool> RemoveBookAuthorAsync(BookAuthorData bookAuthorData)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AuthorData>> ReadAuthorsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/Authors");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<AuthorData>>();
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
        public Task<Boolean> AddAuthorAsync(AuthorData authorData)
        {
            throw new NotImplementedException();
        }
        public Task<Boolean> UpdateAuthorAsync(AuthorData authorData)
        {
            throw new NotImplementedException();
        }
        public Task<Boolean> RemoveAuthorAsync(AuthorData authorData)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CoverData>> ReadCoversAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/Covers");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<CoverData>>();
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
        public Task<Boolean> AddCoverAsync(CoverData coverData)
        {
            throw new NotImplementedException();
        }
        public Task<Boolean> DeleteCoverAsync(CoverData coverData)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VolumeData>> ReadVolumesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/Volumes");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<VolumeData>>();
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
        public Task<Boolean> AddVolumeAsync(VolumeData volumeData)
        {
            throw new NotImplementedException();
        }
        public Task<Boolean> UpdateVolumeAsync(VolumeData volumeData)
        {
            throw new NotImplementedException();
        }
        public Task<Boolean> RemoveVolumeAsync(VolumeData volumeData)
        {
            throw new NotImplementedException();
        }
        public Task<Boolean> SortOutVolumeAsync(VolumeData volumeData)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LendingData>> ReadLendingsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/Lendings");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<LendingData>>();
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

        public Task<bool> AddLendingAsync(LendingData volumeData)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLendingAsync(LendingData volumeData)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveLendingAsync(LendingData volumeData)
        {
            throw new NotImplementedException();
        }
    }
}
