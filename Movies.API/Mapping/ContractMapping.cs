using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Movies.API.Mapping
{
    public static class ContractMapping
    {

        public static Movie ToMovie(this CreateMovieRequest createMovieRequest)
        {
            return new()
            {
                Genres = (List<string>)createMovieRequest.Genres,
                Title = createMovieRequest.Title,
                Id = Guid.NewGuid(),
                YearOfRelease = createMovieRequest.YearOfRelease
            };
        }

        public static MovieResponse ToMovieResponse (this Movie movie)
        {
            return new()
            {
                Genres = movie.Genres,
                YearOfRelease = movie.YearOfRelease,
                Title = movie.Title,
                Slug = movie.Slug,
                Id = movie.Id,
                Rating = movie.Rating,
                UserRating = movie.UserRating
            };
        }

        public static MoviesResponse ToMoviesResponse(this IEnumerable<Movie> movies, int page, int size, int count)
        {
            return new() { Items = movies.Select(ToMovieResponse), Page = page, Size = size, Count = count  };
        }

        public static IEnumerable<MovieRatingResponse> ToMovieRatingsResponse(this IEnumerable<MovieRating> ratings)
        {
            return ratings.Select(x => new MovieRatingResponse { MovieId = x.MovieId, Rating = x.Rating, Slug = x.Slug });
        }

        public static Movie ToMovie (this UpdateMovieRequest updateMovieRequest, Guid guid )
        {

            return new()
            {
                Genres = (List<string>)updateMovieRequest.Genres,
                Title = updateMovieRequest.Title,
                Id = guid,
                YearOfRelease = updateMovieRequest.YearOfRelease
            };

        } 

        public static MoviesOptions ToMoviesOptions(this GetMoviesRequest request) {
            var sortField = request.SortBy?.Trim('+', '-');
            Console.WriteLine($"Trimmed SortField: {sortField}, sort by is {request.SortBy}");
            return new()
            {
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                SortField = request.SortBy?.Trim('+', '-'),
                Page = request.Page,
                Size = request.Size,
                SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                request.SortBy.StartsWith('-') ? SortOrder.Descending :
                SortOrder.Ascending
                


            };

        
        }

        public static MoviesOptions WithUserId(this MoviesOptions options, Guid? userId)
        {
            options.UserId = userId;
            return options;


        }


    }
}
