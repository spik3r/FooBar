using FooBar;
using Microsoft.EntityFrameworkCore;
using FooBar.Data;
using FooBar.Repositories;
using FooBar.Services;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddHttpClient<JsonPlaceholderService>(client =>
// {
//     client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
// });

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the DbContext to the service container
builder.Services.AddDbContext<AppDbContext>(options =>
    // options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection")));
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the repository and service
builder.Services.AddScoped<ISampleEntityRepository, SampleEntityRepository>();
builder.Services.AddScoped<ISampleEntityService, SampleEntityService>();


// Register User repository and service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IJsonPlaceholderService, JsonPlaceholderService>();
// https://devblogs.microsoft.com/dotnet/dotnet-8-networking-improvements/
builder.Services.AddHttpClient("JsonPlaceholder", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});


// Extra debugging...
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// Log the current environment
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation($"\nCurrent environment: {app.Environment.EnvironmentName}\n");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseMiddleware<ApiKeyMiddleware>();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FooBar API V1"));
}

// Redirect to Swagger when accessing the root URL
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html", permanent: true);
        return;
    }

    await next();
});

// Serve Swagger UI only if the API key is valid
// app.UseWhen(context => context.Request.Path.StartsWithSegments("/swagger"), appBuilder =>
// {
//     appBuilder.UseMiddleware<ApiKeyMiddleware>();
//     appBuilder.UseSwagger();
//     appBuilder.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//         c.RoutePrefix = string.Empty;
//     });
// });

app.UseHttpsRedirection();

// Register the ApiKeyMiddleware
// app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

// app.Run();
app.Run("http://*:5056");

