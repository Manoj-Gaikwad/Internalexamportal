using AutoMapper;
using Internalexamportal.Common.Extensions;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Configurations;
using Internalexamportal.Core.FileSystem;
using Internalexamportal.Core.Services;
using Internalexamportal.DocumentManager.Core;
using Internalexamportal.Web.StartupExtensions;
using InternalExamportal.DataAccessLayer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ===== Service registrations =====

// Configure Application Configurations for Dependency Injection.
builder.Services.AddCustomConfigurations(builder.Configuration);

// Register application services
builder.Services.AddApplicationServices();

// Register EF Core
builder.Services.AddEntityFramework(builder.Configuration);

// CORS policy
builder.Services.AddCorsPolicyCustom();

// Identity
builder.Services.AddIdentityCustom();

// MVC with custom settings
builder.Services.AddMvcCustom();

// Allow Angular dist folder
builder.Services.AddSpaStaticFilesCustom();

// Swagger
builder.Services.AddSwagger();

// Antiforgery
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

// HttpContext accessor
builder.Services.AddHttpContextAccessor();

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// ===== Middleware pipeline =====
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseCors("FusionstakCorsPolicy");

app.UseHttpsRedirection();
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
    app.UseSpaStaticFiles();

app.UseSwaggerUICustom();
app.UseSwagger();

app.UseRouting();

app.UseAuthentication();
app.UseHandleExceptionErrorMiddleware();
app.UseAuthorization();

app.UseDefaultFiles();

// Initialize Aspose license
//var fileConfig = app.Services.GetRequiredService<IOptions<FileConfiguration>>().Value;
//FluentAspose.InitializeLicense(fileConfig.LicensePath);

// Map endpoints
app.MapDefaultControllerRoute().RequireAuthorization();
if (!app.Environment.IsDevelopment())
{
    app.UseSpaCustom();
}


app.Run();
