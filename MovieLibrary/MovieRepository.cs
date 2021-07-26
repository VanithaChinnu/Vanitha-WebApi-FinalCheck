using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary
{
    public class MovieRepository : IMovieRepository
    {
        MovieDataContext dc = new MovieDataContext();
        public async Task DeleteMovie(string title)
        {
            Movie movie2del = await GetMovieByTitle(title);
            dc.Movies.Remove(movie2del);
            await dc.SaveChangesAsync();
        }

        public async Task EditMovie(string title, Movie movie)
        {
            Movie movie2edit = await GetMovieByTitle(title);
            movie2edit.BoxOffice = movie.BoxOffice;
            movie2edit.Active = movie.Active;
            movie2edit.Genre = movie.Genre;
            movie2edit.DateOfLaunch = movie.DateOfLaunch;
            movie2edit.HasTeaser = movie.HasTeaser;
            movie2edit.Favorite = movie.Favorite;
            await dc.SaveChangesAsync();
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            return await dc.Movies.ToListAsync();
        }       
        public async Task<Movie> GetMovieByTitle(string title)
        {
            try
            {
                Movie movie = await (from m in dc.Movies where m.Title == title select m).FirstAsync();
                return movie;
            }
            catch (Exception)
            {

                throw new MovieException("Movie with the given title not found");
            }
        }

        public async Task InsertMoive(Movie movie)
        {
            await dc.Movies.AddAsync(movie);
            await dc.SaveChangesAsync();
        }
    }
}
