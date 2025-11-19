using Infrastructure;
using Application;
using Api.GraphQL.Mutations;
using Api.GraphQL.Queries;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map GraphQL endpoint
app.MapGraphQL();

app.Run();

