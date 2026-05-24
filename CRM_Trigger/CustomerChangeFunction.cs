using CRM_Trigger.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class CustomerChangeFunction
{
    private readonly ILogger _logger;

    public CustomerChangeFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CustomerChangeFunction>();
    }

    [Function("CustomerChangeFunction")]
    public async Task Run(
        [CosmosDBTrigger(
            databaseName: "CRM_DB",
            containerName: "customers",
            Connection = "CosmosConnection",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]
        IReadOnlyList<Customer> input)
    {
        if (input == null || input.Count == 0)
            return;

        foreach (var customer in input)
        {
            _logger.LogInformation($"Customer changed: {customer.name}");

            // Send email to sales rep
            await EmailService.SendEmail(customer);
        }
    }
}