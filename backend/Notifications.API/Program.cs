using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Notifications.API;
using Notifications.API.Hubs;
using Notifications.BL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
	options.AddDefaultPolicy(
		policy => {
			policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5500")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials()
				.SetIsOriginAllowed(hostName => true);
		});
});

// Add services to the container.
builder.Services.AddSingleton<IUserIdProvider, SignalRUserIdProvider>();
builder.Services.AddHostedService<RabbitMqBackGroundListener>();
builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options => {
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options => {
		options.TokenValidationParameters = new TokenValidationParameters {
			ValidateIssuer = true,
			ValidIssuer = Config.Issuer,
			ValidateAudience = true,
			ValidAudience = Config.Audience,
			ValidateLifetime = true,
			IssuerSigningKey = Config.GetSymmetricSecurityKey(),
			ValidateIssuerSigningKey = true,
		};
		options.Events = new JwtBearerEvents {
			OnMessageReceived = context => {
				var accessToken = context.Request.Query["access_token"];

				var path = context.HttpContext.Request.Path;
				if (!string.IsNullOrEmpty(accessToken) &&
					(path.StartsWithSegments("/notifications"))) {
					context.Token = accessToken;
				}

				return Task.CompletedTask;
			}
		};
	});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors();


// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationsHub>("/notifications");


app.Run();
