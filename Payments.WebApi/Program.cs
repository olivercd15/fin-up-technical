using Payments.Application.Payments.Commands;
using FluentValidation;
using Payments.Infrastructure.DI;
using MediatR;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection")
);

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddMediatR(typeof(CreatePaymentCommand).Assembly);


builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentCommandValidator>();


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
