using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Identity;

using WAF_exercise_Library_Portal_1_Core_Db.Models;

namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public static class DbInitializer
    {
        private static LibraryDbContext _context;

        private static UserManager<ApplicationUser> _userManager;
        private static RoleManager<IdentityRole<int>> _roleManager;

        public static void Initialize(LibraryDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            string coverImageDirectory)
        {
            _context = context;
            _context.Database.EnsureCreated();

            _userManager = userManager;
            _roleManager = roleManager;

            if (_context.Book.Any() == false)
            {
                SeedAuthor();
                SeedCover(coverImageDirectory);
                SeedBook();
                SeedVolume();
                SeedBookAuthor();

                SeedApplicationUser();

                SeedLending();
            }
        }

        private static void SeedAuthor()
        {
            _context.Author.Add(new Author() { /*Id = 1,*/ Name = "J. K. Rowling" });
            _context.Author.Add(new Author() { /*Id = 2,*/ Name = "Madeleine L'Engle" });
            _context.Author.Add(new Author() { /*Id = 3,*/ Name = "Melinda Leigh" });
            _context.Author.Add(new Author() { /*Id = 4,*/ Name = "Lee Goldberg " });
            _context.Author.Add(new Author() { /*Id = 5,*/ Name = "Bella Forrest" });
            _context.Author.Add(new Author() { /*Id = 6,*/ Name = "James Patterson" });
            _context.Author.Add(new Author() { /*Id = 7,*/ Name = "Brandi Reeds " });
            _context.Author.Add(new Author() { /*Id = 8,*/ Name = "Harold Schechter" });
            _context.Author.Add(new Author() { /*Id = 9,*/ Name = "Loretta Nyhan" });
            _context.Author.Add(new Author() { /*Id = 10,*/ Name = "Kristin Hannah" });
            _context.Author.Add(new Author() { /*Id = 11,*/ Name = "Kevin J Anderson" });
            _context.Author.Add(new Author() { /*Id = 12,*/ Name = "Brian Herbert" });

            _context.SaveChanges();
        }

        private static void SeedCover(string coverImageDirectory)
        {
            if (Directory.Exists(coverImageDirectory))
            {
                //int index = 1;
                foreach (String filePath in Directory.EnumerateFiles(coverImageDirectory))
                {
                    if (File.Exists(filePath))
                    {
                        _context.Cover.Add(new Cover() { /*Id = index,*/ Image = File.ReadAllBytes(filePath) });
                        //index += 1;
                    }
                }
            }

            _context.SaveChanges();
        }

        private static void SeedBook()
        {
            _context.Book.Add(new Book() { /*Id = 1,*/Title = "Harry Potter and the Chamber of Secrets: The Illustrated Edition", PublishedYear = 2016, Isbn = 9780545791328, CoverId = 7 });
            _context.Book.Add(new Book() { /*Id = 2,*/Title = "Harry Potter and the Prisoner of Azkaban: The Illustrated Edition", PublishedYear = 2017, Isbn = 9780545791342, CoverId = 8 });
            _context.Book.Add(new Book() { /*Id = 3,*/Title = "A Wrinkle in Time", PublishedYear = 2007, Isbn = 9780312367541, CoverId = 1 });
            _context.Book.Add(new Book() { /*Id = 4,*/Title = "Say You're Sorry", PublishedYear = 2017, Isbn = 9781503948709, CoverId = 15 });
            _context.Book.Add(new Book() { /*Id = 5,*/Title = "True Fiction", PublishedYear = 2018, Isbn = 9781503949188, CoverId = 23 });
            _context.Book.Add(new Book() { /*Id = 6,*/Title = "The Gender Game", PublishedYear = 2016, Isbn = 9781535197724, CoverId = 19 });
            _context.Book.Add(new Book() { /*Id = 7,*/Title = "Fifty Fifty", PublishedYear = 2018, Isbn = 9780316513227, CoverId = 6 });
            _context.Book.Add(new Book() { /*Id = 8,*/Title = "Trespassing: A Novel", PublishedYear = 2018, Isbn = 9781503950108, CoverId = 22 });
            _context.Book.Add(new Book() { /*Id = 9,*/Title = "Hell's Princess: The Mystery of Belle Gunness, Butcher of Men", PublishedYear = 2018, Isbn = 9781477808955, CoverId = 11 });
            _context.Book.Add(new Book() { /*Id = 10,*/Title = "Digging In: A Novel", PublishedYear = 2018, Isbn = 9781503951709, CoverId = 3 });
            _context.Book.Add(new Book() { /*Id = 11,*/Title = "The Great Alone: A Novel", PublishedYear = 2018, Isbn = 9780312577230, CoverId = 20 });
            _context.Book.Add(new Book() { /*Id = 12,*/Title = "Harry Potter and the Sorcerer's Stone", PublishedYear = 1999, Isbn = 9780439708180, CoverId = 9 });
            _context.Book.Add(new Book() { /*Id = 13,*/Title = "Harry Potter and the Sorcerer's Stone: The Illustrated Edition", PublishedYear = 2015, Isbn = 9780545790352, CoverId = 10 });
            _context.Book.Add(new Book() { /*Id = 14,*/Title = "Fantastic Beasts and Where to Find Them", PublishedYear = 2017, Isbn = 9781338216790, CoverId = 5 });
            _context.Book.Add(new Book() { /*Id = 15,*/Title = "All-American Murder: The Rise and Fall of Aaron Hernandez, the Superstar Whose Life Ended on Murderers' Row", PublishedYear = 2018, Isbn = 9780316412650, CoverId = 2 });
            _context.Book.Add(new Book() { /*Id = 16,*/Title = "The 17th Suspect", PublishedYear = 2018, Isbn = 9780316274043, CoverId = 17 });
            _context.Book.Add(new Book() { /*Id = 17,*/Title = "Murder Beyond the Grave", PublishedYear = 2018, Isbn = 9781538744826, CoverId = 13 });
            _context.Book.Add(new Book() { /*Id = 18,*/Title = "The People vs. Alex Cross", PublishedYear = 2017, Isbn = 9780316273909, CoverId = 21 });
            _context.Book.Add(new Book() { /*Id = 19,*/Title = "Seconds to Live", PublishedYear = 2016, Isbn = 9781503935020, CoverId = 16 });
            _context.Book.Add(new Book() { /*Id = 20,*/Title = "What I've Done", PublishedYear = 2018, Isbn = 9781503903050, CoverId = 24 });
            _context.Book.Add(new Book() { /*Id = 21,*/Title = "Empire Girls", PublishedYear = 2014, Isbn = 9780778316299, CoverId = 4 });
            _context.Book.Add(new Book() { /*Id = 22,*/Title = "I'll Be Seeing You", PublishedYear = 2013, Isbn = 9780778314950/*, CoverId = NULL*/ });
            _context.Book.Add(new Book() { /*Id = 23,*/Title = "Hunters of Dune", PublishedYear = 2005, Isbn = 9780765312921, CoverId = 12 });
            _context.Book.Add(new Book() { /*Id = 24,*/Title = "Sandworms of Dune", PublishedYear = 2012, Isbn = 9780340837528, CoverId = 14 });
            _context.Book.Add(new Book() { /*Id = 25,*/Title = "The Forgotten Heroes: The Heroic Story of the United States Merchant Marine", PublishedYear = 2005, Isbn = 9780765307064, CoverId = 18 });

            _context.SaveChanges();
        }

        private static void SeedVolume()
        {
            _context.Volume.Add(new Volume() { /*Id = 1,*/ BookId = 1 });
            _context.Volume.Add(new Volume() { /*Id = 2,*/ BookId = 2 });
            _context.Volume.Add(new Volume() { /*Id = 3,*/ BookId = 3 });
            _context.Volume.Add(new Volume() { /*Id = 4,*/ BookId = 4 });
            _context.Volume.Add(new Volume() { /*Id = 5,*/ BookId = 5 });
            _context.Volume.Add(new Volume() { /*Id = 6,*/ BookId = 6 });
            _context.Volume.Add(new Volume() { /*Id = 7,*/ BookId = 7 });
            _context.Volume.Add(new Volume() { /*Id = 8,*/ BookId = 8 });
            _context.Volume.Add(new Volume() { /*Id = 9,*/ BookId = 9 });
            _context.Volume.Add(new Volume() { /*Id = 10,*/ BookId = 10 });
            _context.Volume.Add(new Volume() { /*Id = 11,*/ BookId = 11 });
            _context.Volume.Add(new Volume() { /*Id = 12,*/ BookId = 12 });
            _context.Volume.Add(new Volume() { /*Id = 13,*/ BookId = 13 });
            _context.Volume.Add(new Volume() { /*Id = 14,*/ BookId = 14 });
            _context.Volume.Add(new Volume() { /*Id = 15,*/ BookId = 15 });
            _context.Volume.Add(new Volume() { /*Id = 16,*/ BookId = 16 });
            _context.Volume.Add(new Volume() { /*Id = 17,*/ BookId = 17 });
            _context.Volume.Add(new Volume() { /*Id = 18,*/ BookId = 18 });
            _context.Volume.Add(new Volume() { /*Id = 19,*/ BookId = 20 });
            _context.Volume.Add(new Volume() { /*Id = 20,*/ BookId = 21 });
            _context.Volume.Add(new Volume() { /*Id = 21,*/ BookId = 22 });
            _context.Volume.Add(new Volume() { /*Id = 22,*/ BookId = 23 });
            _context.Volume.Add(new Volume() { /*Id = 23,*/ BookId = 24 });
            _context.Volume.Add(new Volume() { /*Id = 24,*/ BookId = 25 });
            _context.Volume.Add(new Volume() { /*Id = 25,*/ BookId = 1 });
            _context.Volume.Add(new Volume() { /*Id = 26,*/ BookId = 1 });
            _context.Volume.Add(new Volume() { /*Id = 27,*/ BookId = 2 });
            _context.Volume.Add(new Volume() { /*Id = 28,*/ BookId = 3 });
            _context.Volume.Add(new Volume() { /*Id = 29,*/ BookId = 3 });
            _context.Volume.Add(new Volume() { /*Id = 30,*/ BookId = 3 });
            _context.Volume.Add(new Volume() { /*Id = 31,*/ BookId = 3 });
            _context.Volume.Add(new Volume() { /*Id = 32,*/ BookId = 3 });
            _context.Volume.Add(new Volume() { /*Id = 33,*/ BookId = 3 });
            _context.Volume.Add(new Volume() { /*Id = 34,*/ BookId = 3 });

            _context.SaveChanges();
        }

        private static void SeedBookAuthor()
        {
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 1,*/ BookId = 1, AuthorId = 1 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 2,*/ BookId = 2, AuthorId = 1 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 3,*/ BookId = 3, AuthorId = 2 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 4,*/ BookId = 4, AuthorId = 3 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 5,*/ BookId = 5, AuthorId = 4 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 6,*/ BookId = 6, AuthorId = 5 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 7,*/ BookId = 7, AuthorId = 6 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 8,*/ BookId = 8, AuthorId = 7 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 9,*/ BookId = 9, AuthorId = 8 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 10,*/ BookId = 10, AuthorId = 9 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 11,*/ BookId = 11, AuthorId = 10 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 12,*/ BookId = 12, AuthorId = 1 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 13,*/ BookId = 13, AuthorId = 1 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 14,*/ BookId = 14, AuthorId = 1 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 15,*/ BookId = 15, AuthorId = 6 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 16,*/ BookId = 16, AuthorId = 6 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 17,*/ BookId = 17, AuthorId = 6 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 18,*/ BookId = 18, AuthorId = 6 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 19,*/ BookId = 19, AuthorId = 3 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 20,*/ BookId = 20, AuthorId = 3 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 21,*/ BookId = 21, AuthorId = 9 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 22,*/ BookId = 22, AuthorId = 9 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 23,*/ BookId = 23, AuthorId = 11 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 24,*/ BookId = 23, AuthorId = 12 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 25,*/ BookId = 24, AuthorId = 11 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 26,*/ BookId = 24, AuthorId = 12 });
            _context.BookAuthor.Add(new BookAuthor() { /*Id = 27,*/ BookId = 25, AuthorId = 12 });

            _context.SaveChanges();
        }

        private static void SeedApplicationUser()
        {
            var adminRole = new IdentityRole<int>("admin");
            CheckErrors(_roleManager.CreateAsync(adminRole).Result);

            var userRole = new IdentityRole<int>("user");
            CheckErrors(_roleManager.CreateAsync(userRole).Result);

            var adminUser = new ApplicationUser()
            {
                UserName = "admin",
                Name = "Admin",
                Email = "admin@example.com",
            };
            var adminUserPassword = "Almafa123";
            CheckErrors(_userManager.CreateAsync(adminUser, adminUserPassword).Result);
            CheckErrors(_userManager.AddToRoleAsync(adminUser, adminRole.Name).Result);

            var testUser1 = new ApplicationUser()
            {
                UserName = "test1",
                Name = "Test 1",
                Email = "test1@example.com",
            };
            var testUser1Password = "Almafa123";
            CheckErrors(_userManager.CreateAsync(testUser1, testUser1Password).Result);
            CheckErrors(_userManager.AddToRoleAsync(testUser1, userRole.Name).Result);

            var testUser2 = new ApplicationUser()
            {
                UserName = "test2",
                Name = "Test 2",
                Email = "test2@example.com",
            };
            var testUser2Password = "Almafa123";
            CheckErrors(_userManager.CreateAsync(testUser2, testUser2Password).Result);
            CheckErrors(_userManager.AddToRoleAsync(testUser2, userRole.Name).Result);
        }

        private static void SeedLending()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            Int32 min = 2;
            Int32 max = 3;

            List<Volume> volumes = _context.Volume.Where(v => v.IsSordtedOut == false).ToList();

            if (volumes.Count < 7)
            {
                throw new Exception("Not enougth not sorted out volume seed ...");
            }

            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(0).Id });
            _context.Lending.Add(new Lending() { Active = 1, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(0).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(0).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(0).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(0).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(0).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(0).Id });

            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(1).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(1).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(1).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(1).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(1).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(1).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(1).Id });

            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(2).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(2).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(2).Id });
            _context.Lending.Add(new Lending() { Active = 1, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(2).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(2).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(2).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(2).Id });

            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(3).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(3).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(3).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(3).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(3).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(3).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(3).Id });

            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(4).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(4).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(4).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(4).Id });
            _context.Lending.Add(new Lending() { Active = 1, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(4).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(4).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(4).Id });

            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(5).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(5).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(5).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(5).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(5).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(5).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(5).Id });

            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(6).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(6).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(6).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(6).Id });
            _context.Lending.Add(new Lending() { Active = 2, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(6).Id });
            _context.Lending.Add(new Lending() { Active = 1, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(6).Id });
            _context.Lending.Add(new Lending() { Active = 0, ApplicationUserId = random.Next(min, max), StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(6).Id });

            _context.SaveChanges();
        }

        private static void CheckErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                throw new Exception(String.Format("Error while creating users.{2}Code: {0}; Description: {1}.", error.Code, error.Description, Environment.NewLine));
            }
        }
    }
}
