using Microsoft.EntityFrameworkCore;
using MusicServiceGraphQL.Data;
using MusicServiceGraphQL.GraphQL;
using MusicServiceGraphQL.GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder
    .Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddType<SongInputType>()
    .AddType<SongType>();


var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MusicDbContext>();
    context.Database.EnsureCreated();
    DatabaseSeeder.SeedData(context);
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapGraphQL();
});

app.Run(); 