using Auth;
using Auth.BL.Extensions;
using Auth.DAL.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthDependencyIdentity();
builder.Services.AddAccountService();
builder.Services.AddAutoMapperExt();

var app = builder.Build();

/*using var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetService<AuthDbContext>();*/

await app.ConfigureIdentityAsync();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
