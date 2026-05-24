using CRM_Trigger.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

public static class EmailService
{
    public static async Task SendEmail(Customer customer)
    {
        var client = new SendGridClient(Environment.GetEnvironmentVariable("SendGridKey"));

        var from = new EmailAddress("test@example.com", "CRM System");
        var to = new EmailAddress(customer.salesRep.email);

        var subject = "You have a new/updated customer";

        var body = $@"
Customer updated/added:

Name: {customer.name}
Title: {customer.title}
Phone: {customer.phone}
Email: {customer.email}
Address: {customer.address}

";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

        await client.SendEmailAsync(msg);
    }
}