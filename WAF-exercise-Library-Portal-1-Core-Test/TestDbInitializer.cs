using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using WAF_exercise_Library_Portal_1_Core_Db;
using WAF_exercise_Library_Portal_1_Core_Db.Models;

namespace WAF_exercise_Library_Portal_1_Core_Test
{
    public static class TestDbInitializer
    {
        public static void Initialize(LibraryDbContext context)
        {
            String coverImageDirectory = "..\\Data\\Images\\Covers";

            context.Author.Add(new Author() { /*Id = 1,*/ Name = "J. K. Rowling" });
            context.Author.Add(new Author() { /*Id = 2,*/ Name = "Madeleine L'Engle" });
            context.Author.Add(new Author() { /*Id = 3,*/ Name = "Melinda Leigh" });
            context.Author.Add(new Author() { /*Id = 4,*/ Name = "Lee Goldberg " });
            context.Author.Add(new Author() { /*Id = 5,*/ Name = "Bella Forrest" });
            context.Author.Add(new Author() { /*Id = 6,*/ Name = "James Patterson" });
            context.Author.Add(new Author() { /*Id = 7,*/ Name = "Brandi Reeds " });
            context.Author.Add(new Author() { /*Id = 8,*/ Name = "Harold Schechter" });
            context.Author.Add(new Author() { /*Id = 9,*/ Name = "Loretta Nyhan" });
            context.Author.Add(new Author() { /*Id = 10,*/ Name = "Kristin Hannah" });
            context.Author.Add(new Author() { /*Id = 11,*/ Name = "Kevin J Anderson" });
            context.Author.Add(new Author() { /*Id = 12,*/ Name = "Brian Herbert" });


            if (Directory.Exists(coverImageDirectory))
            {
                //int index = 1;
                foreach (String filePath in Directory.EnumerateFiles(coverImageDirectory))
                {
                    if (File.Exists(filePath))
                    {
                        context.Cover.Add(new Cover() { /*Id = index,*/ Image = File.ReadAllBytes(filePath) });
                        //index += 1;
                    }
                }
            }

            context.Book.Add(new Book() { /*Id = 1,*/Title = "Harry Potter and the Chamber of Secrets: The Illustrated Edition", PublishedYear = 2016, Isbn = 9780545791328, CoverId = 7 });
            context.Book.Add(new Book() { /*Id = 2,*/Title = "Harry Potter and the Prisoner of Azkaban: The Illustrated Edition", PublishedYear = 2017, Isbn = 9780545791342, CoverId = 8 });
            context.Book.Add(new Book() { /*Id = 3,*/Title = "A Wrinkle in Time", PublishedYear = 2007, Isbn = 9780312367541, CoverId = 1 });
            context.Book.Add(new Book() { /*Id = 4,*/Title = "Say You're Sorry", PublishedYear = 2017, Isbn = 9781503948709, CoverId = 15 });
            context.Book.Add(new Book() { /*Id = 5,*/Title = "True Fiction", PublishedYear = 2018, Isbn = 9781503949188, CoverId = 23 });
            context.Book.Add(new Book() { /*Id = 6,*/Title = "The Gender Game", PublishedYear = 2016, Isbn = 9781535197724, CoverId = 19 });
            context.Book.Add(new Book() { /*Id = 7,*/Title = "Fifty Fifty", PublishedYear = 2018, Isbn = 9780316513227, CoverId = 6 });
            context.Book.Add(new Book() { /*Id = 8,*/Title = "Trespassing: A Novel", PublishedYear = 2018, Isbn = 9781503950108, CoverId = 22 });
            context.Book.Add(new Book() { /*Id = 9,*/Title = "Hell's Princess: The Mystery of Belle Gunness, Butcher of Men", PublishedYear = 2018, Isbn = 9781477808955, CoverId = 11 });
            context.Book.Add(new Book() { /*Id = 10,*/Title = "Digging In: A Novel", PublishedYear = 2018, Isbn = 9781503951709, CoverId = 3 });
            context.Book.Add(new Book() { /*Id = 11,*/Title = "The Great Alone: A Novel", PublishedYear = 2018, Isbn = 9780312577230, CoverId = 20 });
            context.Book.Add(new Book() { /*Id = 12,*/Title = "Harry Potter and the Sorcerer's Stone", PublishedYear = 1999, Isbn = 9780439708180, CoverId = 9 });
            context.Book.Add(new Book() { /*Id = 13,*/Title = "Harry Potter and the Sorcerer's Stone: The Illustrated Edition", PublishedYear = 2015, Isbn = 9780545790352, CoverId = 10 });
            context.Book.Add(new Book() { /*Id = 14,*/Title = "Fantastic Beasts and Where to Find Them", PublishedYear = 2017, Isbn = 9781338216790, CoverId = 5 });
            context.Book.Add(new Book() { /*Id = 15,*/Title = "All-American Murder: The Rise and Fall of Aaron Hernandez, the Superstar Whose Life Ended on Murderers' Row", PublishedYear = 2018, Isbn = 9780316412650, CoverId = 2 });
            context.Book.Add(new Book() { /*Id = 16,*/Title = "The 17th Suspect", PublishedYear = 2018, Isbn = 9780316274043, CoverId = 17 });
            context.Book.Add(new Book() { /*Id = 17,*/Title = "Murder Beyond the Grave", PublishedYear = 2018, Isbn = 9781538744826, CoverId = 13 });
            context.Book.Add(new Book() { /*Id = 18,*/Title = "The People vs. Alex Cross", PublishedYear = 2017, Isbn = 9780316273909, CoverId = 21 });
            context.Book.Add(new Book() { /*Id = 19,*/Title = "Seconds to Live", PublishedYear = 2016, Isbn = 9781503935020, CoverId = 16 });
            context.Book.Add(new Book() { /*Id = 20,*/Title = "What I've Done", PublishedYear = 2018, Isbn = 9781503903050, CoverId = 24 });
            context.Book.Add(new Book() { /*Id = 21,*/Title = "Empire Girls", PublishedYear = 2014, Isbn = 9780778316299, CoverId = 4 });
            context.Book.Add(new Book() { /*Id = 22,*/Title = "I'll Be Seeing You", PublishedYear = 2013, Isbn = 9780778314950/*, CoverId = NULL*/ });
            context.Book.Add(new Book() { /*Id = 23,*/Title = "Hunters of Dune", PublishedYear = 2005, Isbn = 9780765312921, CoverId = 12 });
            context.Book.Add(new Book() { /*Id = 24,*/Title = "Sandworms of Dune", PublishedYear = 2012, Isbn = 9780340837528, CoverId = 14 });
            context.Book.Add(new Book() { /*Id = 25,*/Title = "The Forgotten Heroes: The Heroic Story of the United States Merchant Marine", PublishedYear = 2005, Isbn = 9780765307064, CoverId = 18 });

            context.Volume.Add(new Volume() { /*Id = 1,*/ BookId = 1 });
            context.Volume.Add(new Volume() { /*Id = 2,*/ BookId = 2 });
            context.Volume.Add(new Volume() { /*Id = 3,*/ BookId = 3 });
            context.Volume.Add(new Volume() { /*Id = 4,*/ BookId = 4 });
            context.Volume.Add(new Volume() { /*Id = 5,*/ BookId = 5 });
            context.Volume.Add(new Volume() { /*Id = 6,*/ BookId = 6 });
            context.Volume.Add(new Volume() { /*Id = 7,*/ BookId = 7 });
            context.Volume.Add(new Volume() { /*Id = 8,*/ BookId = 8 });
            context.Volume.Add(new Volume() { /*Id = 9,*/ BookId = 9 });
            context.Volume.Add(new Volume() { /*Id = 10,*/ BookId = 10 });
            context.Volume.Add(new Volume() { /*Id = 11,*/ BookId = 11 });
            context.Volume.Add(new Volume() { /*Id = 12,*/ BookId = 12 });
            context.Volume.Add(new Volume() { /*Id = 13,*/ BookId = 13 });
            context.Volume.Add(new Volume() { /*Id = 14,*/ BookId = 14 });
            context.Volume.Add(new Volume() { /*Id = 15,*/ BookId = 15 });
            context.Volume.Add(new Volume() { /*Id = 16,*/ BookId = 16 });
            context.Volume.Add(new Volume() { /*Id = 17,*/ BookId = 17 });
            context.Volume.Add(new Volume() { /*Id = 18,*/ BookId = 18 });
            context.Volume.Add(new Volume() { /*Id = 19,*/ BookId = 20 });
            context.Volume.Add(new Volume() { /*Id = 20,*/ BookId = 21 });
            context.Volume.Add(new Volume() { /*Id = 21,*/ BookId = 22 });
            context.Volume.Add(new Volume() { /*Id = 22,*/ BookId = 23 });
            context.Volume.Add(new Volume() { /*Id = 23,*/ BookId = 24 });
            context.Volume.Add(new Volume() { /*Id = 24,*/ BookId = 25 });
            context.Volume.Add(new Volume() { /*Id = 25,*/ BookId = 1 });
            context.Volume.Add(new Volume() { /*Id = 26,*/ BookId = 1 });
            context.Volume.Add(new Volume() { /*Id = 27,*/ BookId = 2 });
            context.Volume.Add(new Volume() { /*Id = 28,*/ BookId = 3 });
            context.Volume.Add(new Volume() { /*Id = 29,*/ BookId = 3 });
            context.Volume.Add(new Volume() { /*Id = 30,*/ BookId = 3 });
            context.Volume.Add(new Volume() { /*Id = 31,*/ BookId = 3 });
            context.Volume.Add(new Volume() { /*Id = 32,*/ BookId = 3 });
            context.Volume.Add(new Volume() { /*Id = 33,*/ BookId = 3 });
            context.Volume.Add(new Volume() { /*Id = 34,*/ BookId = 3 });

            context.BookAuthor.Add(new BookAuthor() { /*Id = 1,*/ BookId = 1, AuthorId = 1 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 2,*/ BookId = 2, AuthorId = 1 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 3,*/ BookId = 3, AuthorId = 2 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 4,*/ BookId = 4, AuthorId = 3 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 5,*/ BookId = 5, AuthorId = 4 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 6,*/ BookId = 6, AuthorId = 5 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 7,*/ BookId = 7, AuthorId = 6 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 8,*/ BookId = 8, AuthorId = 7 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 9,*/ BookId = 9, AuthorId = 8 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 10,*/ BookId = 10, AuthorId = 9 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 11,*/ BookId = 11, AuthorId = 10 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 12,*/ BookId = 12, AuthorId = 1 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 13,*/ BookId = 13, AuthorId = 1 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 14,*/ BookId = 14, AuthorId = 1 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 15,*/ BookId = 15, AuthorId = 6 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 16,*/ BookId = 16, AuthorId = 6 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 17,*/ BookId = 17, AuthorId = 6 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 18,*/ BookId = 18, AuthorId = 6 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 19,*/ BookId = 19, AuthorId = 3 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 20,*/ BookId = 20, AuthorId = 3 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 21,*/ BookId = 21, AuthorId = 9 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 22,*/ BookId = 22, AuthorId = 9 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 23,*/ BookId = 23, AuthorId = 11 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 24,*/ BookId = 23, AuthorId = 12 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 25,*/ BookId = 24, AuthorId = 11 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 26,*/ BookId = 24, AuthorId = 12 });
            context.BookAuthor.Add(new BookAuthor() { /*Id = 27,*/ BookId = 25, AuthorId = 12 });

            context.SaveChanges();

            List<Volume> volumes = context.Volume.Where(v => v.IsSordtedOut == false).ToList();

            if (volumes.Count < 7)
            {
                throw new Exception("Not enougth not sorted out volume seed ...");
            }

            context.Lending.Add(new Lending() {/*Id = 1,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(0).Id });
            context.Lending.Add(new Lending() {/*Id = 2,*/ Active = 1, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(0).Id });
            context.Lending.Add(new Lending() {/*Id = 3,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(0).Id });
            context.Lending.Add(new Lending() {/*Id = 4,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(0).Id });
            context.Lending.Add(new Lending() {/*Id = 5,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(0).Id });
            context.Lending.Add(new Lending() {/*Id = 6,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(0).Id });
            context.Lending.Add(new Lending() {/*Id = 7,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(0).Id });

            context.Lending.Add(new Lending() {/*Id = 8,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(1).Id });
            context.Lending.Add(new Lending() {/*Id = 9,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(1).Id });
            context.Lending.Add(new Lending() {/*Id = 10,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(1).Id });
            context.Lending.Add(new Lending() {/*Id = 11,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(1).Id });
            context.Lending.Add(new Lending() {/*Id = 12,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(1).Id });
            context.Lending.Add(new Lending() {/*Id = 13,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(1).Id });
            context.Lending.Add(new Lending() {/*Id = 14,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(1).Id });

            context.Lending.Add(new Lending() {/*Id = 15,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(2).Id });
            context.Lending.Add(new Lending() {/*Id = 16,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(2).Id });
            context.Lending.Add(new Lending() {/*Id = 17,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(2).Id });
            context.Lending.Add(new Lending() {/*Id = 18,*/ Active = 1, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(2).Id });
            context.Lending.Add(new Lending() {/*Id = 19,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(2).Id });
            context.Lending.Add(new Lending() {/*Id = 20,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(2).Id });
            context.Lending.Add(new Lending() {/*Id = 21,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(2).Id });

            context.Lending.Add(new Lending() {/*Id = 22,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(3).Id });
            context.Lending.Add(new Lending() {/*Id = 23,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(3).Id });
            context.Lending.Add(new Lending() {/*Id = 24,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(3).Id });
            context.Lending.Add(new Lending() {/*Id = 25,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(3).Id });
            context.Lending.Add(new Lending() {/*Id = 26,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(3).Id });
            context.Lending.Add(new Lending() {/*Id = 27,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(3).Id });
            context.Lending.Add(new Lending() {/*Id = 28,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(3).Id });

            context.Lending.Add(new Lending() {/*Id = 29,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(4).Id });
            context.Lending.Add(new Lending() {/*Id = 30,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(4).Id });
            context.Lending.Add(new Lending() {/*Id = 31,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(4).Id });
            context.Lending.Add(new Lending() {/*Id = 32,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(4).Id });
            context.Lending.Add(new Lending() {/*Id = 33,*/ Active = 1, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(4).Id });
            context.Lending.Add(new Lending() {/*Id = 34,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(4).Id });
            context.Lending.Add(new Lending() {/*Id = 35,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(4).Id });

            context.Lending.Add(new Lending() {/*Id = 36,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(5).Id });
            context.Lending.Add(new Lending() {/*Id = 37,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(5).Id });
            context.Lending.Add(new Lending() {/*Id = 38,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(5).Id });
            context.Lending.Add(new Lending() {/*Id = 39,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(5).Id });
            context.Lending.Add(new Lending() {/*Id = 40,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(5).Id });
            context.Lending.Add(new Lending() {/*Id = 41,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(5).Id });
            context.Lending.Add(new Lending() {/*Id = 42,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(5).Id });

            context.Lending.Add(new Lending() {/*Id = 43,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-7), EndDate = DateTime.UtcNow.AddDays(-6), VolumeId = volumes.ElementAt(6).Id });
            context.Lending.Add(new Lending() {/*Id = 44,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-4), VolumeId = volumes.ElementAt(6).Id });
            context.Lending.Add(new Lending() {/*Id = 45,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-3), EndDate = DateTime.UtcNow.AddDays(-2), VolumeId = volumes.ElementAt(6).Id });
            context.Lending.Add(new Lending() {/*Id = 46,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), VolumeId = volumes.ElementAt(6).Id });
            context.Lending.Add(new Lending() {/*Id = 47,*/ Active = 2, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), VolumeId = volumes.ElementAt(6).Id });
            context.Lending.Add(new Lending() {/*Id = 48,*/ Active = 1, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(4), EndDate = DateTime.UtcNow.AddDays(5), VolumeId = volumes.ElementAt(6).Id });
            context.Lending.Add(new Lending() {/*Id = 49,*/ Active = 0, ApplicationUserId = 0, StartDate = DateTime.UtcNow.AddDays(6), EndDate = DateTime.UtcNow.AddDays(7), VolumeId = volumes.ElementAt(6).Id });

            context.SaveChanges();
        }
    }
}
