using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contracts.Requests
{
    public class GetMoviesRequest : PagedRequest
    {

        public required string? Title { get; init; }
        public required int? YearOfRelease { get; init; }

        public required string? SortBy { get; init; }

    }
}
