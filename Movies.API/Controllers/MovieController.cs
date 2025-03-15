using Microsoft.AspNetCore.Mvc;
using Movies.API.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using System.Reflection.Metadata.Ecma335;
namespace Movies.API.Controllers
{
    [ApiController]

    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> CreateMovie([FromBody]CreateMovieRequest createMovieRequest)
        {
            Movie movie = createMovieRequest.ToMovie();

            await _movieRepository.CreateAsync(movie);

            MovieResponse movieResponse = movie.ToMovieResponse();

            return Created($"api/movies/{movieResponse.Id}", movieResponse);

        }
        [HttpPost(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> GetMovie([FromRoute] Guid id)
        {
            Movie? movie = await _movieRepository.GetByIdAsync(id);
            if (movie is null)
            {
                return NotFound();
            }

            return Ok(movie.ToMovieResponse());
        }

        [HttpPost(ApiEndpoints.Movies.GetAll)]

        public async Task<IActionResult> GetMovies()
        {
            IEnumerable<Movie> movies = await _movieRepository.GetAllAsync();
            MoviesResponse reponse = movies.ToMoviesResponse();
            return Ok();

        }
        
    }
}
