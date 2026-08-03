using System.Text;
using System.Text.Json.Serialization;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
IConfiguration configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddDbContext<VendingDbContext>(op=>
{
    op.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddMvc().AddJsonOptions(op =>
{
    op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op =>
{
    op.RequireHttpsMetadata = false;
    op.TokenValidationParameters = new()
    {
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
    };
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(op=>
{
    op.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
});

app.MapControllers();
app.Run();