using Microsoft.EntityFrameworkCore;
using Movies_API.Data;
using Movies_API.Models;
using Movies_API.Repository.IRepository;

namespace Movies_API.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _db;

        public MovieRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateMovie(Movie movie)
        {
            movie.CreatedAt = DateTime.UtcNow;

            _db.Movie.Add(movie);

            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _db.Movie.Remove(movie);

            return Save();
        }

        public IEnumerable<Movie> FindMovie(string name)
        {
            IQueryable<Movie> query = _db.Movie;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name) || e.Description.Contains(name));
            }

            return query.ToList();
        }

        public Movie GetMovie(int id)
        {
            return _db.Movie.FirstOrDefault(category => category.Id == id);
        }

        public ICollection<Movie> GetMovies()
        {
            return _db.Movie.OrderBy(category => category.Name).ToList();
        }

        public ICollection<Movie> GetMoviesByCategory(int categoryId)
        {
            return _db.Movie.Include(c => c.Category).Where(c => c.CategoryId == categoryId).ToList();
        }

        public bool MovieExists(int id)
        {
            return _db.Movie.Any(category => category.Id == id);
        }

        public bool MovieExists(string name)
        {
            bool value = _db.Movie.Any(category => category.Name.ToLower().Trim() == name.ToLower().Trim());

            return value;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateMovie(Movie movie)
        {
            movie.CreatedAt = DateTime.UtcNow;

            var movieExists = _db.Movie.Find(movie.Id);

            if (movieExists != null)
            {
                _db.Entry(movieExists).CurrentValues.SetValues(movie);
            }
            else
            {
                _db.Movie.Update(movie);
            }

            return Save();
        }
    }
}
