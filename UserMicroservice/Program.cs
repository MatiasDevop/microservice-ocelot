using UserMicroservice.Services.Interface;
using UserMicroservice.Services;
using UserMicroservice.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<DbUserContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
//}

app.Logger.LogInformation("testing for docker User Microservice");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
