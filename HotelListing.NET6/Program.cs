using System;
using HotelListing.NET6.Data;
using Serilog;
using Microsoft.EntityFrameworkCore;
using HotelListing.NET6.Configurations;
using HotelListing.NET6.Contracts;
using HotelListing.NET6.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HotelListing.NET6.Middleware;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnection");

builder.Services.AddDbContext<HotelListingDbContext>(options =>
    options.UseNpgsql(connectionString));

///Identity Role
builder.Services.AddIdentityCore<ApiUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListing")
    .AddEntityFrameworkStores<HotelListingDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AllowAll
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        b => b.AllowAnyHeader()
              .AllowAnyOrigin()
              .AllowAnyMethod());
});

///APi Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver")
    );
});
///APi Versioning Explorer
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
///DI
builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

//Jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],       // appsettings.json se issuer
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))  // Secret key jo token sign karti hai
    };
});
//Api Caching
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024;
    options.UseCaseSensitivePaths = true;
});

//Add Odata for Filtering Searching
builder.Services.AddControllers()
    .AddOData(options =>
    {
        options.Select().Filter().OrderBy();
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

///For Exception
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

//Caching 
app.UseResponseCaching();
app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(10)
        };

    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] { "Accept-Encoding" };

    await next();
});

//For Login
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();