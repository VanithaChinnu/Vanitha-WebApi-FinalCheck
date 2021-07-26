using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllMovies();
        Task<Movie> GetMovieByTitle(string title);
        Task InsertMoive(Movie movie);
        Task DeleteMovie(string title);
        Task EditMovie(string title, Movie movie);
    }
}
