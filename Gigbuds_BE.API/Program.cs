using Gigbuds_BE.API.Extensions;
using Gigbuds_BE.API.Middlewares;
using Gigbuds_BE.Application.Extensions;
using Gigbuds_BE.Infrastructure.Extensions;
using Gigbuds_BE.Infrastructure.Persistence;
using Gigbuds_BE.Infrastructure.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using System.Configuration;

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
// app.UseCors("AllowGemini");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GigBuds API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapGroup("api/identities")
    .WithTags("Identities");
    
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
var applicationDbContext = scope.ServiceProvider.GetRequiredService<GigbudsDbContext>();

var ShouldReseedData = app.Configuration.GetValue<bool>("ClearAndReseedData");

app.Run();
