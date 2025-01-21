using Movies_API.Models;

namespace Movies_API.Repository.IRepository
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();

        ICollection<Movie> GetMoviesByCategory(int categoryId);

        IEnumerable<Movie> FindMovie(string name);

        Movie GetMovie(int id);

        bool MovieExists(int id);

        bool MovieExists(string name);

        bool CreateMovie(Movie movie);

        bool UpdateMovie(Movie movie);

        bool DeleteMovie(Movie movie);

        bool Save();
    }
}
