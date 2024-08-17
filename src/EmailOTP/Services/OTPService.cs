using OtpNet;

namespace EmailOTP.Services
{
    public class OTPService
    {
        /// <summary>
        /// Generate OTP code
        /// </summary>
        /// <returns></returns>
        public string GenerateOTP()
        {
            var secret = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(secret);
            var secretKey = Base32Encoding.ToBytes(base32Secret);
            var totp = new Totp(secretKey);
            var otpCode = totp.ComputeTotp(DateTime.UtcNow);
            return otpCode;
        }
    }
}
