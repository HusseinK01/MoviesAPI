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

            return CreatedAtAction(nameof(GetMovie), new { idOrSlug = movie.Id }, movie);

        }
        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> GetMovie([FromRoute] string idOrSlug)
        {
            Movie? movie = Guid.TryParse(idOrSlug, out var guid) 
                ?  await _movieRepository.GetByIdAsync(guid) 
                : await _movieRepository.GetBySlugAsync(idOrSlug);
            if (movie is null)
            {
                return NotFound();
            }

            return Ok(movie.ToMovieResponse());
        }

        [HttpGet(ApiEndpoints.Movies.GetAll)]

        public async Task<IActionResult> GetMovies()
        {
            IEnumerable<Movie> movies = await _movieRepository.GetAllAsync();
            MoviesResponse reponse = movies.ToMoviesResponse();
            return Ok(reponse);

        }

        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> UpdateMovie ([FromRoute]Guid id, [FromBody] UpdateMovieRequest updateMovieRequest)
        {
            Movie movie = updateMovieRequest.ToMovie(id);
            bool updated = await _movieRepository.UpdateAsync(movie);

            if (!updated) { return NotFound(); }

            return Ok(movie.ToMovieResponse());
        }

        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
        {
            bool deleted = await _movieRepository.DeleteByIdAsync(id);
            if (!deleted) { return NotFound(); }
            return Ok();
        }
    }
}
