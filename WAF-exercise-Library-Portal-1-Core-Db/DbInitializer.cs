using System;
using System.IO;
using System.Linq;


namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public static class DbInitializer
    {
        private static LibraryDbContext _context;

        public static void Initialize(LibraryDbContext context, string coverImageDirectory)
        {
            _context = context;
            _context.Database.EnsureCreated();

            if (context.Book.Any())
            {
                return; // Az adatbázis már inicializálva van.
            }

            SeedAuthor();
            SeedCover(coverImageDirectory);



            context.SaveChanges();
        }

        private static void SeedAuthor()
        {
            _context.Author.Add(new Author() { Id = 1, Name = "J. K. Rowling" });
            _context.Author.Add(new Author() { Id = 2, Name = "Madeleine L'Engle" });
            _context.Author.Add(new Author() { Id = 3, Name = "Melinda Leigh" });
            _context.Author.Add(new Author() { Id = 4, Name = "Lee Goldberg " });
            _context.Author.Add(new Author() { Id = 5, Name = "Bella Forrest" });
            _context.Author.Add(new Author() { Id = 6, Name = "James Patterson" });
            _context.Author.Add(new Author() { Id = 7, Name = "Brandi Reeds " });
            _context.Author.Add(new Author() { Id = 8, Name = "Harold Schechter" });
            _context.Author.Add(new Author() { Id = 9, Name = "Loretta Nyhan" });
            _context.Author.Add(new Author() { Id = 10, Name = "Kristin Hannah" });
            _context.Author.Add(new Author() { Id = 11, Name = "Kevin J Anderson" });
            _context.Author.Add(new Author() { Id = 12, Name = "Brian Herbert" });

            _context.SaveChanges();
        }

        private static async void SeedCover(string coverImageDirectory)
        {
            if (Directory.Exists(coverImageDirectory))
            {
                int index = 1;
                foreach (String fileName in Directory.EnumerateFiles(coverImageDirectory))
                {
                    String filePath = Path.Combine(coverImageDirectory, fileName);
                    if (File.Exists(filePath))
                    {
                        using (FileStream stream = File.Open(filePath, FileMode.Open))
                        {
                            Byte[] buffer = new Byte[stream.Length];
                            await stream.ReadAsync(buffer, 0, (int)stream.Length);
                            _context.Cover.Add(new Cover() { Id = index, Image = buffer });
                            index += 1;
                        }
                    }

                }

                _context.SaveChanges();
            }
        }
    }
}
