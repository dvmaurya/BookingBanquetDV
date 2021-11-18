using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Utilities;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using Windows.ApplicationModel.Contacts;

namespace BookingBanquetDV.Utility
{
    public static class EmailUtility
    {
        public static bool SendEmails(ContactEmail contactEmail, string otp, string userName)
        {
            bool isEmailSent = false;
            if (Utilities.IsInternetConnected())
            { 
                try
                {
                    MimeMessage message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin", "dvmaurya6@gmail.com"));
                    message.To.Add(new MailboxAddress(userName, contactEmail.Address));
                    message.Subject = "Password recover OTP";

                    message.Body = new TextPart("plain")
                    {
                        Text = "Hi " + userName + ", " +
                            "\rYour One Time Password for recover your account password is: " + otp +
                            "\r\rThanks and regards\nAdmin"
                    };

                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 465, true);

                        client.Authenticate("dvmaurya6@gmail.com", "rqhgpveikjndhxgw");

                        client.Send(message);
                        client.Disconnect(true);
                        isEmailSent = true;
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));
                }
            }

            return isEmailSent;
        }
    }
}
