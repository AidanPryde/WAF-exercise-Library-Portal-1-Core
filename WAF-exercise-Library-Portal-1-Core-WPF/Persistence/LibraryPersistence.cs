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

        public async Task<Boolean> LoginAsAdminAsync(LoginData user)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/LoginAsAdmin", user);
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
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Books", new BookData(bookData.Id, bookData.Title, bookData.PublishedYear, bookData.Isbn, new CoverData(bookData.Cover.Id, bookData.Cover?.Image)));
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
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/Books/", new BookData(bookData.Id, bookData.Title, bookData.PublishedYear, bookData.Isbn, new CoverData(bookData.Cover.Id, bookData.Cover?.Image)));
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
        public async Task<Boolean> CreateBookAuthorAsync(BookAuthorData bookAuthorData)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/BookAuthors", new BookAuthorData(bookAuthorData.Id, new BookData(bookAuthorData.BookData.Id), new AuthorData(bookAuthorData.AuthorData.Id)));
                bookAuthorData.Id = (await response.Content.ReadAsAsync<Int32>());
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }
        public async Task<Boolean> DeleteBookAuthorAsync(BookAuthorData bookAuthorData)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/BookAuthors/" + bookAuthorData.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
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
        public async Task<Boolean> CreateAuthorAsync(AuthorData authorData)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Authors", new AuthorData(authorData.Id, authorData.Name));
                authorData.Id = (await response.Content.ReadAsAsync<Int32>());
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }
        public async Task<Boolean> UpdateAuthorAsync(AuthorData authorData)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/Authors", new AuthorData(authorData.Id, authorData.Name));
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
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
        public async Task<Boolean> CreateCoverAsync(CoverData coverData)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Covers", new CoverData(coverData.Id, coverData.Image));
                coverData.Id = (await response.Content.ReadAsAsync<Int32>());
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }
        public async Task<Boolean> DeleteCoverAsync(CoverData coverData)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/Covers/" + coverData.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
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
        public async Task<Boolean> CreateVolumeAsync(VolumeData volumeData)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Volumes", new VolumeData(volumeData.Id, volumeData.IsSortedOut, new BookData(volumeData.BookData.Id)));
                volumeData.Id = (await response.Content.ReadAsAsync<String>());
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }
        public async Task<Boolean> UpdateVolumeAsync(VolumeData volumeData)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/Volumes", new VolumeData(volumeData.Id, volumeData.IsSortedOut, new BookData(volumeData.BookData.Id)));
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }
        public async Task<Boolean> DeleteVolumeAsync(VolumeData volumeData)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/Volumes/" + volumeData.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
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
        public async Task<Boolean> UpdateLendingAsync(LendingData lendingData)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/Lendings", new LendingData(lendingData.Id, lendingData.UserName, lendingData.StartDate, lendingData.EndDate, lendingData.Active, new VolumeData(lendingData.VolumeData.Id)));
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }
        public async Task<Boolean> DeleteLendingAsync(LendingData lendingData)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/Lendings/" + lendingData.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                throw new PersistenceUnavailableException(exception);
            }
        }
    }
}
