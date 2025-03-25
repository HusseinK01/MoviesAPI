using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using System.Reflection.Metadata.Ecma335;
namespace Movies.API.Controllers
{
    [ApiController]

    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieRepository)
        {
            _movieService = movieRepository;
        }


        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> CreateMovie([FromBody]CreateMovieRequest createMovieRequest, CancellationToken token)
        {
            Movie movie = createMovieRequest.ToMovie();

            await _movieService.CreateAsync(movie, token);

            MovieResponse movieResponse = movie.ToMovieResponse();

            return CreatedAtAction(nameof(GetMovie), new { idOrSlug = movie.Id }, movie);

        }

        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> GetMovie([FromRoute] string idOrSlug, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            Movie? movie = Guid.TryParse(idOrSlug, out var movieId) 
                ?  await _movieService.GetByIdAsync(movieId, userId, token) 
                : await _movieService.GetBySlugAsync(idOrSlug, userId, token);
            if (movie is null)
            {
                return NotFound();
            }

            return Ok(movie.ToMovieResponse());
        }

        [HttpGet(ApiEndpoints.Movies.GetAll)]

        public async Task<IActionResult> GetMovies([FromQuery] GetMoviesRequest request,CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var options = request.ToMoviesOptions().WithUserId(userId); 
            IEnumerable<Movie> movies = await _movieService.GetAllAsync(options, token);
            int resultCount = await _movieService.GetCountAsync(options.Title,options.YearOfRelease, token);
            MoviesResponse reponse = movies.ToMoviesResponse(request.Page, request.Size, resultCount);
            return Ok(reponse);

        }

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> UpdateMovie ([FromRoute]Guid id, [FromBody] UpdateMovieRequest updateMovieRequest, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            Movie movie = updateMovieRequest.ToMovie(id);
            var updated = await _movieService.UpdateAsync(movie, userId, token);

            if (updated is null) { return NotFound(); }

            return Ok(updated.ToMovieResponse());
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id, CancellationToken token)
        {
            bool deleted = await _movieService.DeleteByIdAsync(id, token);
            if (!deleted) { return NotFound(); }
            return Ok();
        }
    }
}
