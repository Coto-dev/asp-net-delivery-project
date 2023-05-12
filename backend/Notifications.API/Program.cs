using Auth.BL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Notifications.API;
using Notifications.API.Hubs;

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
//builder.Services.AddBackGroundService();
//builder.Services.AddHostedService<RabbitMqBackGroundListener>(); TODO

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
			ValidIssuer = JwtConfiguration.Issuer,
			ValidateAudience = true,
			ValidAudience = JwtConfiguration.Audience,
			ValidateLifetime = true,
			IssuerSigningKey = JwtConfiguration.GetSymmetricSecurityKey(),
			ValidateIssuerSigningKey = true,
		};
		options.Events = new JwtBearerEvents {
			OnMessageReceived = context => {
				var accessToken = context.Request.Query["access_token"];

				// If the request is for our hub...
				var path = context.HttpContext.Request.Path;
				if (!string.IsNullOrEmpty(accessToken) &&
					(path.StartsWithSegments("/notifications"))) {
					// Read the token out of the query string
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
