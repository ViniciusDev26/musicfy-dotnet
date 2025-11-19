using Infrastructure;
using Application;
using Api.GraphQL.Mutations;
using Api.GraphQL.Queries;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SoapCore;
using Api.Soap.Contracts;
using Api.Soap.Services;
using Api.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure (DbContext + Repositories)
builder.Services.AddInfrastructure("Data Source=musicfy.db");

// Add Application (Use Cases)
builder.Services.AddApplication();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add gRPC
builder.Services.AddGrpc();

// Add SOAP services
builder.Services.AddScoped<IUserSoapService, UserSoapService>();
builder.Services.AddScoped<IMusicSoapService, MusicSoapService>();
builder.Services.AddScoped<IPlaylistSoapService, PlaylistSoapService>();
builder.Services.AddSoapCore();

// Add GraphQL
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

// Map REST API endpoints
app.MapControllers();

// Map GraphQL endpoint
app.MapGraphQL();

// Map gRPC endpoints
app.MapGrpcService<UserGrpcService>();
app.MapGrpcService<MusicGrpcService>();
app.MapGrpcService<PlaylistGrpcService>();

// Map SOAP endpoints
app.UseSoapEndpoint<IUserSoapService>("/soap/UserService.asmx", new SoapEncoderOptions());
app.UseSoapEndpoint<IMusicSoapService>("/soap/MusicService.asmx", new SoapEncoderOptions());
app.UseSoapEndpoint<IPlaylistSoapService>("/soap/PlaylistService.asmx", new SoapEncoderOptions());

app.Run();

