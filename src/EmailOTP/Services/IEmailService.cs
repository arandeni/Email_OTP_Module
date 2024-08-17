namespace EmailOTP.Services
{
    public interface IEmailService
    {
        bool send_email(string email_address, string email_body);
        bool IsEmailValid(string email);
    }
}
