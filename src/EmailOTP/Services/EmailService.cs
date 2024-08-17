using System.Text.RegularExpressions;

namespace EmailOTP.Services
{
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Validate the email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsEmailValid(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Assume this function is implemented to send an email
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="emailBody"></param>
        /// <returns></returns>
        public bool send_email(string email_address, string email_body)
        {
            return true;
        }
    }
}
