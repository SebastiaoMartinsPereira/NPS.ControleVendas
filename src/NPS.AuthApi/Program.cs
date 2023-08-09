using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using NPS.AuthApi.Data;
using NPS.AuthApi.Domain;
using NPS.AuthApi.Model;
using NPS.Core;
using NPS.Core.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.NPSConfigAuthentication(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.NPSConfigSwaggerGen();

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("pt-BR")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


builder.Services.AddCors(p => p.AddDefaultPolicy(policy => policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IAppSettingsProvider>(a => new AppSettingsProvider(builder.Configuration));
builder.Services.AddScoped<IMongoDbContextBase, MongoDbContextBase>();
builder.Services.AddScoped<IMongoDbSettings, MongoDbSettings>();
builder.Services.AddScoped<IMongoRepository<UserInfo>, MongoRepository<UserInfo>>();

builder.Services.AddAuthorization(options =>
{
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
        JwtBearerDefaults.AuthenticationScheme);

    defaultAuthorizationPolicyBuilder =
        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseStatusCodePages();

IOptions<RequestLocalizationOptions>? locationzationOption = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if (locationzationOption != null)
    app.UseRequestLocalization(locationzationOption.Value);

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

//Add middleware to change de language Culture to user Resource file
app.UseRequestCulture();

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());

app.Run();
