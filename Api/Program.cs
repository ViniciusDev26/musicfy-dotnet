using Infrastructure;
using Application;
using Api.GraphQL.Mutations;
using Api.GraphQL.Queries;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure (DbContext + Repositories)
builder.Services.AddInfrastructure("Data Source=musicfy.db");

// Add Application (Use Cases)
builder.Services.AddApplication();

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddTypeExtension<UserQueries>()
    .AddTypeExtension<MusicQueries>()
    .AddTypeExtension<PlaylistQueries>()
    .AddMutationType()
    .AddTypeExtension<UserMutations>()
    .AddTypeExtension<MusicMutations>()
    .AddTypeExtension<PlaylistMutations>();

var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map GraphQL endpoint
app.MapGraphQL();

app.Run();

