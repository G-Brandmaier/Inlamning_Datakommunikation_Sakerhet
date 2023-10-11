using Api_Hub.Hubs;
using Api_Hub.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// AllowLocalHost policy added to Cors
/// <summary>
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
    builder =>
    {
        builder.WithOrigins("https://localhost:7107").WithMethods("POST", "GET").AllowAnyHeader();
    });
});

/// <summary>
/// Authentication added with JwtBearer token
/// <summary>
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.Events = new JwtBearerEvents
    {
        //OnTokenValidated = context =>
        //{
        //    if (string.IsNullOrEmpty(context?.Principal?.FindFirst("id")?.Value) || string.IsNullOrEmpty(context?.Principal?.Identity?.Name))
        //        context?.Fail("Unauthorized");

        //    return Task.CompletedTask;
        //},
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };

    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt")["Issuer"],
        ValidAudience = builder.Configuration.GetSection("Jwt")["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt")["Key"]!))
    };
});

/// <summary>
/// Authorization policy added for a required role
/// <summary>
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("admin"));
});

builder.Services.AddSignalR();
builder.Services.AddScoped<AccountService>();

var app = builder.Build();

/// <summary>
/// HttpsResponseHeaders added, allowing from localhost with https and wss on specified port number
/// <summary>
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self' wss://localhost:44320/Api_Hub/; " +
        "script-src 'self' 'unsafe-inline'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "font-src 'self'; " +
        "img-src 'self' data:; " +
        "frame-src 'self';" +
        "connect-src 'self' https://localhost:7107 wss://localhost:44320/Api_Hub/;");

    await next();
});
app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<WeatherHub>("/weatherhub");

app.Run();
