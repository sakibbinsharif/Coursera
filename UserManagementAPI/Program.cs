
namespace UserManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpLogging();

            builder.Services.AddScoped<IUserService, UserService>();
            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception exception)
                {
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An unhandled exception occurred while processing the request.");

                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(
                        new { status = StatusCodes.Status500InternalServerError, title = "An unexpected error occurred." },
                        context.RequestAborted);
                }
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.Use(async (context,next) =>
            {
                // Log the request
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Incoming request: {method} {url}", context.Request.Method, context.Request.Path);
                await next.Invoke();
                // Log the response
                logger.LogInformation("Outgoing response: {statusCode}", context.Response.StatusCode);
            });
            app.MapControllers();

            app.MapGet("/", () => "User Management API is running!");

            app.Run();
        }
    }
}
