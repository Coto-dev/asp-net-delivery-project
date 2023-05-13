using System.Reflection;
using Auth;
using Auth.BL;
using Auth.DAL.Data;
using Auth.BL.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Common.Enums;
using System.Reflection;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using Common.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(opts => {
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});
builder.Services.AddControllers();

builder.Services.AddAuthBlDependency();
builder.Services.AddAuthBlIdentityDependency();
builder.Services.AddAccountService();
builder.Services.AddAutoMapperExt();
builder.Services.AddTokenService();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => {
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth", Version = "v2" });
    option.SchemaFilter<EnumSchemaFilter>();  
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});




builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = JwtConfiguration.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtConfiguration.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = JwtConfiguration.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();


/*using var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetService<AuthDbContext>();*/

await app.ConfigureIdentityAsync();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
