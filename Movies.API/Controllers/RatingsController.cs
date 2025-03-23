using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Auth;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.API.Controllers
{
    [ApiController]
    public class RatingsController : ControllerBase
    {


        private readonly IRatingsService _ratingsService;

        public RatingsController(IRatingsService ratingsService)
        {
            _ratingsService = ratingsService;
        }

        [HttpPut(ApiEndpoints.Movies.Rate)]
        public async Task<IActionResult> RateMovie([FromRoute] Guid id,[FromBody] RateMovieRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var result = await _ratingsService.RateMovieAsync(id,userId!.Value, request.Rating, token);

            return result ? Ok() : NotFound();
        }


        [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var result = await _ratingsService.DeleteRatingAsync(id, userId!.Value, token);
            return result ? Ok() : NotFound();
        }





    }
}
