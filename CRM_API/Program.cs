
using CRM_API.Models;
using CRM_API.Service;
using Microsoft.EntityFrameworkCore;

namespace CRM_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddSingleton<CosmosService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Minimal API
            //
            // GET all customers
            app.MapGet("/customers", async (CosmosService service) =>
            {
                return await service.GetAll();
            });

            // GET by id
            app.MapGet("/customers/{id}", async (string id, CosmosService service) =>
            {
                return await service.GetById(id);
            });

            // GET by name
            app.MapGet("/customers/search/name/{name}", async (string name,CosmosService service) =>
            {
                return await service.SearchByName(name);
            });

            // GET by SalesRep
            app.MapGet("/customers/search/salesrep/{salesRepName}", async (string salesRepName,CosmosService service) =>
            {
                return await service.SearchBySalesRep(salesRepName);
            });

            // CREATE customer
            app.MapPost("/customers", async (Customer customer, CosmosService service) =>
            {
                await service.Add(customer);
                return Results.Ok(customer);
            });

            // UPDATE customer
            app.MapPut("/customers/{id}", async (string id, Customer customer, CosmosService service) =>
            {
                customer.id = id;
                await service.Update(id, customer);
                return Results.Ok(customer);
            });

            // DELETE customer
            app.MapDelete("/customers/{id}", async (string id, CosmosService service) =>
            {
                await service.Delete(id);
                return Results.Ok();
            });
            
            app.Run();
        }
    }
}
