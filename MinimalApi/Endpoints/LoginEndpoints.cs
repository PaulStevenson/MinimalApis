namespace MinimalApiDemo.Endpoints
{
    public static class LoginEndpoints
    {
        public static void MapLoginEndpoints(WebApplication app)
        {
            app.MapPost("/login", (
                Login user, 
                ILoginInService loginInService) 
                => loginInService.Login(user));
        }
    }
}
