using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Graph;
using MovieLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Providers.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class MovieController : ControllerBase
    {
        IMovieRepository mRep;
        public MovieController(IMovieRepository imr)
        {
            mRep = imr;
        }
        [Authorize(Roles = "Customer,Admin,AnonymousUser")]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Movie>>> GetAll()
        {
            return Ok(await mRep.GetAllMovies());
        }
        [Authorize(Roles ="Customer,Admin")]
        [HttpGet("{title}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Movie>> GetOne(string title)
        {
            return Ok(await mRep.GetMovieByTitle(title));
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult> Insert(Movie movie)
        {
            await mRep.InsertMoive(movie);
            return Created($"api/Movie/{movie.Title}", movie);
        }
       [Authorize(Roles ="Admin")]
        [HttpPut("{title}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Edit(string title, Movie movie)
        {
            await mRep.EditMovie(title, movie);
            return Ok(movie);
        }
        [HttpDelete("{title}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Delete(string title)
        {
            await mRep.DeleteMovie(title);
            return Ok();
        }
    }
}
