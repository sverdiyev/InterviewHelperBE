using InterviewHelper.Core.Helper;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Repositories;
using InterviewHelper.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000"); // frontend origin
        });
}); // cors enabling

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IQuestionsServices, QuestionsServices>();
// inject userRepository in userService
builder.Services.AddScoped<IUserRepository, UserRepository>();
// inject userService in userController
builder.Services.AddScoped<UserService>();
builder.Services.Configure<DBConfiguration>(builder.Configuration.GetSection("Database"));

InitializationService.Init(builder.Configuration.GetValue<string>("Database:ConnectionString"));

var app = builder.Build();

app.UseCors(); // cors enabling

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();