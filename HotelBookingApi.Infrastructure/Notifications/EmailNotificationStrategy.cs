using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.Interfaces.Notifications;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HotelBookingApi.Infrastructure.Notifications;

public class EmailNotificationStrategy : INotificationStrategy
{
    private readonly ILogger<EmailNotificationStrategy> _logger;
    private readonly SmtpSettings _smtpSettings;

    public EmailNotificationStrategy(ILogger<EmailNotificationStrategy> logger, IOptions<SmtpSettings> smtpSettings)
    {
        _logger = logger;
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendBookingConfirmedAsync(Booking booking, Guest guest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[Email] Sending confirmation to {Email} for booking {BookingId}...", guest.Email, booking.Id);
        await SendEmailAsync(guest.Email, "Booking Confirmed", $"Your booking {booking.Id} is confirmed.");
        _logger.LogInformation("[Email] Confirmation sent to {Email}.", guest.Email);
    }

    public async Task SendBookingCancelledAsync(Booking booking, Guest guest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[Email] Sending cancellation to {Email} for booking {BookingId}...", guest.Email, booking.Id);
        await SendEmailAsync(guest.Email, "Booking Cancelled", $"Your booking {booking.Id} has been cancelled.");
        _logger.LogInformation("[Email] Cancellation sent to {Email}.", guest.Email);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpSettings.FromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };
        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }
}
