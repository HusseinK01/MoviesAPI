﻿using Microsoft.OpenApi.Services;

namespace Movies.API
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Movies
        {
            private const string Base = $"{ApiBase}/Movies";

            public const string Create = Base;
            public const string Get = $"{Base}/{{idOrSlug}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";

            public const string Rate = $"{Base}/{{id:guid}}/rate";
            public const string DeleteRating = $"{Base}/{{id:guid}}/rate";




        }

        public static class Ratings
        {
            private const string Base = $"{ApiBase}/ratings";


            public const string GetUserRating = $"{Base}/me";


        }

    }
}
