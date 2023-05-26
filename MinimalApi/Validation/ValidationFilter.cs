namespace MinimalApiDemo.Validation
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

            if (validator != null)
            {
                var entity = context.Arguments
                    .OfType<T>()
                    .FirstOrDefault(x => x?.GetType() == typeof(T));

                var validation = await validator.ValidateAsync(entity);
                if (validation.IsValid)
                {
                    return await next(context);
                }

                return Results.ValidationProblem(validation.ToDictionary());
            }
            else
            {
                return Results.Problem("Could not find type to validate");
            }
        }
    }
}
