using EmailOTP.Common;
using EmailOTP.Models;
using EmailOTP.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmailOTP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Email_OTP_ModuleController : ControllerBase
    {
        private readonly IOTPService _otpService;
        private readonly IEmailService _emailService;

        public Email_OTP_ModuleController(IOTPService otpService, IEmailService emailService)
        {
            _otpService = otpService;
            _emailService = emailService;
        }

        /// <summary>
        /// Generate OTP and send it to the given email address
        /// </summary>
        /// <param name="user_email"></param>
        /// <returns></returns>
        [Route("GenerateOTP/{user_email}")]
        [HttpPost]
        public IActionResult generate_OTP_email(string user_email)
        {
            //Check whether the given email address is valid or not
            if (!_emailService.IsEmailValid(user_email))
            {
                return StatusCode(StatusCodes.Status400BadRequest, Constants.STATUS_EMAIL_INVALID);
            }
            //Check whether the given email address is in valid domain or not
            else if (!user_email.EndsWith(Constants.DOMAIN))
            {
                return StatusCode(StatusCodes.Status400BadRequest, Constants.STATUS_EMAIL_FAIL);
            }
            else
            {
                if (HttpContext != null && HttpContext.Session != null)
                {
                    try
                    {
                        //Generate OTP
                        string otpCode = _otpService.GenerateOTP();

                        //Set data to OTPDetails model
                        OTPDetails otpDetails = new OTPDetails()
                        {
                            Email = user_email,
                            OTP = otpCode,
                            GeneratedTime = DateTime.UtcNow,
                            Attempts = 0
                        };

                        //Set session data
                        HttpContext.Session.SetString(user_email, JsonSerializer.Serialize(otpDetails));

                        //Set the mail body
                        string email_body = "You OTP Code is " + otpCode + ". The code is valid for 1 minute";

                        //Send OTP code to the given email address
                        bool isEmailSent = _emailService.send_email(user_email, email_body);

                        //Send the response to the client according to the status of the sending email 
                        if (isEmailSent)
                        {
                            return StatusCode(StatusCodes.Status200OK, Constants.STATUS_EMAIL_OK);
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, Constants.STATUS_EMAIL_FAIL);
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, Constants.INTERNAL_SERVER_ERROR + ":" + ex);
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, Constants.INTERNAL_SERVER_ERROR);
                }
            }
        }

        /// <summary>
        /// Validate the OTP
        /// </summary>
        /// <param name="otp"></param>
        /// <returns></returns>
        [Route("CheckOTP/{email}/{otp}")]
        [HttpPost]
        public IActionResult check_OTP(string email, string otp)
        {
            //Check whether the session is expired or not. The session idle time has been set to 1 min.
            if (HttpContext != null && HttpContext.Session != null && HttpContext.Session.Keys.Count() > 0)
            {
                //Get the session data and set it to the OTPDetails model
                var value = HttpContext.Session.Get(email);
                OTPDetails? otpDetails = value != null ? JsonSerializer.Deserialize<OTPDetails>(value) : null;

                //Check whether the OTP details are available for the given email address or not
                if (otpDetails != null)
                {
                    //Check whether the OTP has been expired or not
                    if ((DateTime.UtcNow - otpDetails.GeneratedTime).TotalSeconds < Constants.OTP_VALIDITY)
                    {
                        //Check whether the number of attempts have been exceeded or not
                        if (otpDetails.Attempts < Constants.MAX_ATTEMPTS)
                        {
                            //Check whether the user given OTP is tally with the sent OTP
                            if (otpDetails.OTP == otp)
                            {
                                return StatusCode(StatusCodes.Status200OK, Constants.STATUS_OTP_OK);
                            }
                            else
                            {
                                //Increment the number of attempts
                                otpDetails.Attempts++;
                                //Set the new attempt in the session
                                HttpContext.Session.SetString(email, JsonSerializer.Serialize(otpDetails));
                                return StatusCode(StatusCodes.Status406NotAcceptable, Constants.STATUS_OTP_INVALID);
                            }
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status504GatewayTimeout, Constants.STATUS_OTP_FAIL);
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status504GatewayTimeout, Constants.STATUS_OTP_TIMEOUT);
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, Constants.STATUS_OTP_INVALID_EMAIL);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.STATUS_OTP_SESSION_EXPIRED);
            }
        }
    }
}
