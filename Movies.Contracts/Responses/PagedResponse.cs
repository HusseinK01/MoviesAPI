using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contracts.Responses
{
    public class PagedResponse<Tresponse>
    {
        public required IEnumerable<Tresponse> Items { get; init; } = Enumerable.Empty<Tresponse>();

        public required int Page { get; init; }

        public required int Size { get; init; }

        public required int Count { get; init; }

        public bool HasNextPage => Count > (Page * Size);
    }
}
