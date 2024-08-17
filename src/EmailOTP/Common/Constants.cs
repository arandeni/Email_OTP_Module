namespace EmailOTP.Common
{
    public class Constants
    {
        public const string STATUS_EMAIL_OK = "email containing OTP has been sent successfully";
        public const string STATUS_EMAIL_FAIL = "email address does not exist or sending to the email has failed";
        public const string STATUS_EMAIL_INVALID = "email address is invalid";
        public const string STATUS_EMAIL_OTP_SENT = "OTP has been already sent to this email address";
        public const string STATUS_OTP_OK = "OTP is valid and checked";
        public const string STATUS_OTP_INVALID = "OTP is invalid";
        public const string STATUS_OTP_INVALID_EMAIL = "email is invalid";
        public const string STATUS_OTP_FAIL = "OTP is wrong after 10 tries";
        public const string STATUS_OTP_TIMEOUT = "timeout after 1 min";
        public const string STATUS_OTP_SESSION_EXPIRED = "session has been expired";
        public const string INTERNAL_SERVER_ERROR = "Error occurred while processing your request";
        public const int MAX_ATTEMPTS = 10;
        public const int OTP_VALIDITY = 60;
        public const string DOMAIN = "dso.org.sg";
    }
}
