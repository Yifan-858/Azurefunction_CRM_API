using Microsoft.Azure.Cosmos;
using CRM_API.Models;

namespace CRM_API.Service
{
    public class CosmosService
    {
        private readonly Container _container;

        public CosmosService(IConfiguration config)
        {
            var client = new CosmosClient(
                config["Cosmos:AccountEndpoint"],
                config["Cosmos:AccountKey"]
            );

            var database = client.GetDatabase("CRM_DB");
            _container = database.GetContainer("customers");
        }

        public async Task<List<Customer>> GetAll()
        {
            var query = _container.GetItemQueryIterator<Customer>("SELECT * FROM c");
            var results = new List<Customer>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task Add(Customer customer)
        {
            await _container.CreateItemAsync(customer, new PartitionKey(customer.id));
        }

        public async Task Update(string id, Customer customer)
        {
            await _container.UpsertItemAsync(customer, new PartitionKey(id));
        }

        public async Task Delete(string id)
        {
            await _container.DeleteItemAsync<Customer>(id, new PartitionKey(id));
        }

        public async Task<Customer> GetById(string id)
        {
            return await _container.ReadItemAsync<Customer>(id, new PartitionKey(id));
        }

        public async Task<List<Customer>> SearchByName(string name)
        {
            var customers = await GetAll();

            return customers
                .Where(c => c.name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        public async Task<List<Customer>> SearchBySalesRep(string salesRepName)
        {
            var customers = await GetAll();

            return customers
                .Where(c => c.salesRep.name.ToLower().Contains(salesRepName.ToLower()))
                .ToList();
        }

    }
}
