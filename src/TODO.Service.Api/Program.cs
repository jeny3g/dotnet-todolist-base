using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using System.Globalization;
using Todo.Service.Api.Helpers;
using Todo.Service.CrossCutting;
using Todo.Service.Domain.Settings;
using Todo.Service.Persistence;


var logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

logger.Debug("init main");


try
{
    EnvConstants.ValidateRequiredEnvs();

    var builder = WebApplication.CreateBuilder(args);

    //builder.Services.AddControllers(options =>
    //{
    //    options.AddCustomFilters();
    //});

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    // Add services to the container.
    builder.Services
        .InjectDependencies()
        .InjectLocalization()
        .InjectDatabases(); 


    builder.Services.AddCors();



    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new[] { new CultureInfo("pt-br"), };

        options.DefaultRequestCulture = new RequestCulture("pt-br");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    });

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = false });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));
    }

    app.UseHttpsRedirection();

    app.UseCors(e =>
        e.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

    app.UseLoggerScope();

    var requestlocalizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();

    if (requestlocalizationOptions != null)
        app.UseRequestLocalization(requestlocalizationOptions.Value);

    app.MapControllers();

    SeedDatabase(app);

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}


void SeedDatabase(WebApplication host)
{
    using var scope = host.Services.CreateScope();
    try
    {
        logger.Debug("Seeding db...");

        var context = scope.ServiceProvider.GetService<TodoContext>();

        TodoInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        logger.Error(ex, "Error on seed db...");
    }
}











//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();




//void SeedDatabase(WebApplication host)
//{
//    using var scope = host.Services.CreateScope();
//    try
//    {
//        logger.Debug("Seeding db...");

//        var context = scope.ServiceProvider.GetService<TodoContext>();

//        TodoInitializer.Initialize(context);
//    }
//    catch (Exception ex)
//    {
//        logger.Error(ex, "Error on seed db...");
//    }
//}
