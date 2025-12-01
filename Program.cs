using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers(); // controllers approach
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure sqlite (for dev)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=movietracker.db"));

// optional: add JWT auth here (omitted for brevity), but you can add later.

var app = builder.Build();

// apply migrations automatically in dev (optional)
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
