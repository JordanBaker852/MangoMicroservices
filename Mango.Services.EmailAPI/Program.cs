using Mango.Services.EmailAPI.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("Config/seri-log.config.json", optional: false, reloadOnChange: true);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSerilog(new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger());

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

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
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetAppliedMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}