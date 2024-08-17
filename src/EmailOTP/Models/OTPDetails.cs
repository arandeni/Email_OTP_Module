namespace EmailOTP.Models
{
    public class OTPDetails
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime GeneratedTime { get; set; }
        public int Attempts { get; set; }
    }
}
