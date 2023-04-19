using TestBogus.Models;

var builder = WebApplication.CreateBuilder(args);

var playerGuid = Guid.NewGuid();

//for (int i = 0; i < 10; i++)
//{
//    Console.WriteLine(UserGenerator.GetPlayedGameProcedural(playerGuid).Dump());
//}

//UserGenerator.GetGameUser(true).Generate(10);

// Add services to the container.

var MyAllowSpecificOrigins = "CorsRules";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyOrigin();
                      });
});

builder.Services.AddControllers();
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

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
