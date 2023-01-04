using Microsoft.EntityFrameworkCore;
using TheScientistAPI.Data;

var myAllowScecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ScientistContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TheScientistCS"));
});

//Enable Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowScecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
}); 
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowScecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
