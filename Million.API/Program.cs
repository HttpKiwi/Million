using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Million.Application.Interfaces.Services;
using Million.Application.Mappings;
using Million.Application.Services;
using Million.Infrastructure.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Inject Services
builder.Services.AddScoped<OwnerService>();
builder.Services.AddScoped<PropertyService>();
builder.Services.AddScoped<PropertyImageService>();
builder.Services.AddScoped<PropertyTraceService>();
builder.Services.AddScoped<IOwnerService, OwnerService>();  
builder.Services.AddScoped<IPropertyService, PropertyService>();  
builder.Services.AddScoped<IPropertyTraceService, PropertyTraceService>();  
builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<AutoMapperProfile>();
}, typeof(Program).Assembly);

var app = builder.Build();

if (!app.Environment.EnvironmentName.Equals("Test", StringComparison.OrdinalIgnoreCase))
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();
            context.Database.Migrate();  
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during migration: {ex.Message}");
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
