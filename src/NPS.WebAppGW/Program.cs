using Ocelot.Middleware;
using Ocelot.DependencyInjection; //For Dependency Injection s
//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var app = new WebHostBuilder()
//        .UseKestrel()
//        .UseContentRoot(Directory.GetCurrentDirectory())
//        .ConfigureAppConfiguration((hostingContext, config) =>
//        {
//            config
//                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
//                .AddJsonFile("appsettings.json", true, true)
//                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
//                .AddJsonFile("ocelot.json")
//                .AddEnvironmentVariables();
//        })
//        .ConfigureServices(s =>
//        {
//            s.AddControllers();
//            s.AddOcelot();
//            s.AddEndpointsApiExplorer();
//            s.AddSwaggerGen();

//        })
//        .ConfigureLogging((hostingContext, logging) =>
//        {
//            //add your logging
//        })
//        .UseIISIntegration()
//        .Configure(app =>
//        {
//            app.UseOcelot().Wait();
//            app.UseSwagger();
//            app.UseSwaggerUI();
//            app.UseHttpsRedirection();
//            app.UseAuthorization();
//        })
//        .Build(); 

//        app.Run();
//    }
//}

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel();
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
builder.Configuration.AddJsonFile("ocelot.json");
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddOcelot();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();
