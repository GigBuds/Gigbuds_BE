using Gigbuds_BE.API.Extensions;
using Gigbuds_BE.API.Middlewares;
using Gigbuds_BE.Application.Extensions;
using Gigbuds_BE.Infrastructure.Extensions;
using Gigbuds_BE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.AddPresentation(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplications(builder.Configuration);

var app = builder.Build();

// ====================================
// === Use Middlewares
// ====================================

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure CORS
app.UseCors("AllowFrontend");
app.UseCors("AllowGemini");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
var applicationDbContext = scope.ServiceProvider.GetRequiredService<GigbudsDbContext>();

try
{
    await applicationDbContext.Database.MigrateAsync();
}
catch (Exception ex)
{
    logger.LogError(ex, "Error happens during migrations!");
}
app.Run();
