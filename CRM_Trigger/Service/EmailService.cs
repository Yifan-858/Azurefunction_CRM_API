using CRM_Trigger.Models;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

public static class EmailService
{
    public static async Task SendEmail(Customer customer, ILogger logger)
    {
        var client = new SendGridClient(Environment.GetEnvironmentVariable("SendGridKey"));

        var from = new EmailAddress("yifan.wang@iths.se", "Labb3_AzureFunction");
        var to = new EmailAddress(customer.salesRep.email);

        var subject = "You have a new/updated customer";

        var body = $@"
Customer updated/added:

Name: {customer.name}
Title: {customer.title}
Phone: {customer.phone}
Email: {customer.email}
Address: {customer.address}

You are responsible for this customer.

";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

        var response = await client.SendEmailAsync(msg);
        var responseBody = await response.Body.ReadAsStringAsync();

        logger.LogInformation($"SendGrid status: {response.StatusCode}");
        logger.LogInformation($"SendGrid body: {body}");
    }
}