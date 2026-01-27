using MailKit.Net.Smtp;
using MimeKit;

namespace MyPortolioUdemy.Services
{
  /// <summary>
  /// Email service implementation using MailKit
  /// </summary>
  public class EmailService : IEmailService
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Send email asynchronously
    /// </summary>
    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, string? senderName = null)
    {
      try
      {
        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        var smtpPortStr = _configuration["EmailSettings:SmtpPort"];
        var senderEmail = _configuration["EmailSettings:SenderEmail"];
        var senderPassword = _configuration["EmailSettings:SenderPassword"];
        var senderDisplayName = senderName ?? _configuration["EmailSettings:SenderName"];

        if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpPortStr) ||
            string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
        {
          _logger.LogError("Email configuration is missing in appsettings.json");
          return false;
        }

        int.TryParse(smtpPortStr, out int smtpPort);

        // Create email message
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderDisplayName, senderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        // Send email using SMTP
        using (var client = new SmtpClient())
        {
          await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
          await client.AuthenticateAsync(senderEmail, senderPassword);
          await client.SendAsync(message);
          await client.DisconnectAsync(true);
        }

        _logger.LogInformation($"Email sent successfully to {toEmail}");
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error sending email to {toEmail}: {ex.Message}");
        return false;
      }
    }

    /// <summary>
    /// Send email to admin about new message
    /// </summary>
    public async Task<bool> SendAdminNotificationAsync(string? visitorName, string? visitorEmail, string? subject, string? message)
    {
      if (string.IsNullOrEmpty(visitorName) || string.IsNullOrEmpty(visitorEmail) ||
          string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
      {
        _logger.LogWarning("Invalid parameters for admin notification");
        return false;
      }

      var adminEmail = _configuration["EmailSettings:AdminEmail"];

      if (string.IsNullOrEmpty(adminEmail))
      {
        _logger.LogWarning("Admin email not configured in appsettings.json");
        return false;
      }

      var emailBody = $@"
                <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h2>Yeni İletişim Formu Gönderimi</h2>
                        <p><strong>Gönderen Adı:</strong> {visitorName}</p>
                        <p><strong>Gönderen Email:</strong> {visitorEmail}</p>
                        <p><strong>Başlık:</strong> {subject}</p>
                        <p><strong>Mesaj:</strong></p>
                        <p>{message}</p>
                        <hr />
                        <p style='color: #999; font-size: 12px;'>Bu mesaj otomatik olarak gönderilmiştir.</p>
                    </body>
                </html>";

      return await SendEmailAsync(adminEmail, $"Yeni Mesaj: {subject}", emailBody, visitorName);
    }
  }
}
