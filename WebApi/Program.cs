using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrette;
using Business.Constants;
using Business.DependencyResolves.Autofac;
using Business.Mapping;
using Core.Commons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Abstract;
using Repository.Concrette;
using Repository.Concrette.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var databaseSetting = new DatabaseSettings();
databaseSetting.ConnectionString = builder.Configuration.GetConnectionString("Dapper");
builder.Configuration.Bind(nameof(DatabaseSettings), databaseSetting); 
builder.Services.AddSingleton(databaseSetting);

 
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
     .ConfigureContainer<ContainerBuilder>(builder =>
     {
         builder.RegisterModule(new AutofacBusinessModule());

     });


builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();


var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
              builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
         .AddJwtBearer(options =>
         {
             options.SaveToken = true;
             options.RequireHttpsMetadata = false;
             options.TokenValidationParameters = new TokenValidationParameters()
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidAudience = builder.Configuration["JWT:ValidAudience"],
                 ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
             };
             options.Events = new JwtBearerEvents
             {
                 OnMessageReceived = context =>
                 {
                     var accessToken = context.Request.Query["access_token"];
                     if (String.IsNullOrEmpty(accessToken))
                     {
                         accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                     }

                     var path = context.HttpContext.Request.Path;
                     if (!string.IsNullOrEmpty(accessToken)
                     )
                     {
                         context.Token = accessToken;
                     }
                     return Task.CompletedTask;
                 }
             };
         });

builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        { jwtSecurityScheme, Array.Empty<string>() }
                });

});





builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddMvc();
builder.Services.AddAutoMapper(typeof(MappingProfile));

ServiceTool.Create(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseCors();
app.MapControllers();

app.Run();
