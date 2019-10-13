using System.Data.SqlClient;

using ComedyKing.Model;

using Microsoft.EntityFrameworkCore;

namespace ComedyKing.DataAccess
{
    // Table per Hierarchy(TPH)
    public class CelebrityInCelebrityJokeContext : DbContext
    {
        public DbSet<Person> Pepole { get; set; }
        public DbSet<Celebrity> Celebrities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<CelebrityJoke> CelebrityJokes { get; set; }
        public DbSet<CelebrityInCelebrityJoke> CelebrityInCelebrityJokes { get; set; }

  
        public CelebrityInCelebrityJokeContext()
        {

        }

        public CelebrityInCelebrityJokeContext(DbContextOptions<CelebrityInCelebrityJokeContext> options)
            : base(options)
        {

        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = "",
                UserID = "",
                InitialCatalog = "",
                Password = ""  
                 
            };
            optionsBuilder.UseSqlServer(builder.ConnectionString.ToString());
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CelebrityInCelebrityJoke>().HasKey(sc => new { sc.CelebrityID, sc.CelebrityJokeID });
        }        
    }
}

