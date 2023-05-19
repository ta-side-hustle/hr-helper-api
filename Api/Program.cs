using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Api.Middleware;
using Infrastructure.DI;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Options injection
if (builder.Environment.IsProduction()) builder.Configuration.AddKeyPerFile("/run/secrets/", false, true);
if (builder.Environment.IsDevelopment()) builder.Configuration.AddUserSecrets<Program>(true, true);


builder.Services.Configure<ConnectionStringsOptions>(
	builder.Configuration.GetSection(nameof(ConnectionStringsOptions)));

var jwtOptionsConfig = builder.Configuration.GetSection(nameof(JwtOptions));
builder.Services.Configure<JwtOptions>(jwtOptionsConfig);


// Service configuration
const string corsPolicy = "_allowAllOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(corsPolicy, policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod());
});

builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		var jwtOptions = jwtOptionsConfig.Get<JwtOptions>();

		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwtOptions.Issuer,

			ValidateAudience = true,
			ValidAudience = jwtOptions.Audience,

			ValidateLifetime = true,

			ValidateIssuerSigningKey = true,
			IssuerSigningKey = jwtOptions.SymmetricSecurityKey,

			ClockSkew = TimeSpan.FromMinutes(5)
		};
	});

builder.Services.AddSwaggerGen(options =>
{
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath, true);

	options.SwaggerDoc("v1", new OpenApiInfo { Title = "hr-helper-api", Version = "v1" });

	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Scheme = "Bearer",
		Type = SecuritySchemeType.Http,
		In = ParameterLocation.Header,
		Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter your token in the text input below.
                      Example: `12345abcdef`",
		BearerFormat = "'Bearer [token]'"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Id = "Bearer",
					Type = ReferenceType.SecurityScheme
				},
				Scheme = "oauth2",
				Name = "Bearer",
				Type = SecuritySchemeType.Http,
				In = ParameterLocation.Header,
				BearerFormat = "Bearer [jwt-token]"
			},
			new List<string>()
		}
	});
});

// Service injection
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(uiOptions => { uiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "hr_helper_api v1"); });
}

app.ConfigureExceptionHandler();

app.UseCors(corsPolicy);
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();