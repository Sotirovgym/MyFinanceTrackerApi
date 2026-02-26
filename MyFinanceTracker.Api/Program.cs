using MyFinanceTracker.Api.Common.Extensions;
using MyFinanceTracker.Api.Common.Middlewares;
using MyFinanceTracker.Infrastructure;
using MyFinanceTracker.Infrastructure.Common.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MyFinanceTracker.Application.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add validators from the Application assembly
builder.Services.AddValidatorsFromAssemblyContaining<IIdentityService>(includeInternalTypes: true);
// Enables automatic validation during model binding
builder.Services.AddFluentValidationAutoValidation();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

// Add Swagger services
builder.Services.AddSwagger();

var app = builder.Build();

// Seed roles
if (app.Environment.IsDevelopment())
{
    await app.Services.SeedRolesAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
