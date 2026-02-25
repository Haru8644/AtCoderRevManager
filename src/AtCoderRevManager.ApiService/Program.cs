using AtCoderRevManager.ApiService.Services;
using AtCoderRevManager.Domain.Interfaces;
using AtCoderRevManager.Infrastructure.Data;
using AtCoderRevManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Infrastructure
builder.AddSqlServerDbContext<AppDbContext>("AtCoderRevDb");

// API & Application Services
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IReviewRepository, SqlReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();