using DataImport.API.Extensions;
using DataImport.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterApplicationLayers();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCorsAllowAny();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
