namespace MyPortolioUdemy.Services
{
  /// <summary>
  /// Email service interface for sending emails
  /// </summary>
  public interface IEmailService
  {
    /// <summary>
    /// Send email asynchronously
    /// </summary>
    Task<bool> SendEmailAsync(string toEmail, string subject, string body, string? senderName = null);

    /// <summary>
    /// Send email to admin about new message
    /// </summary>
    Task<bool> SendAdminNotificationAsync(string? visitorName, string? visitorEmail, string? subject, string? message);
  }
}
