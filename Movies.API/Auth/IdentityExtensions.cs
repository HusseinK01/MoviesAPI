namespace Movies.API.Auth
{
    public static class IdentityExtensions
    {

        public static Guid? GetUserId(this HttpContext context)
        {

            var userId = context.User?.Claims.SingleOrDefault(x => x.Type == "userclaim");

            if (Guid.TryParse(userId?.Value, out var userGuid))
            {
                return userGuid;
            }
            else
            {
                return null;
            }
        }
    }
}
