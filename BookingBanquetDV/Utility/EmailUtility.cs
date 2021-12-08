using BookingBanquetDV.Source.Camera;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model;
using CommonServices.Utilities;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace BookingBanquetDV.Utility
{
    public static class EmailUtility
    {
        public static async Task<bool> SendEmailsAsync(EmailParameter emailParameter)
        {
            bool isEmailSent = false;
            if (Utilities.IsInternetConnected())
            { 
                try
                {
                    MimeMessage message = new MimeMessage();
                    message.From.Add(new MailboxAddress(EmailConstants.Admin, EmailConstants.AdminEmail));
                    message.To.Add(new MailboxAddress(emailParameter.UserName,
                        emailParameter.EmailAddress));
                    message = await ConstructBodyAsync(emailParameter, message);

                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Connect(EmailConstants.GmailServer, 465, true);
                        client.Authenticate(EmailConstants.AdminEmail, EmailConstants.AdminPassword);
                        client.Send(message);
                        client.Disconnect(true);
                        isEmailSent = true;
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.Logger.Info("Email Exception: ".ConcatString(ex.Message));
                }
            }

            return isEmailSent;
        }

        private static async Task<MimeMessage> ConstructBodyAsync(EmailParameter emailParameter, MimeMessage message)
        {
            if (emailParameter.IsSignup)
            {
                message.Subject = EmailConstants.AccountCreationOtp;

                message.Body = new TextPart("plain")
                {
                    Text = "Hi ".ConcatString(emailParameter.UserName, ", ",
                    "\r\r\tYour One Time Password for your account creation is: ", emailParameter.Otp,
                    "\r\rThanks and regards\nAdmin")
                };
            }
            else if (emailParameter.IsRecovery)
            {
                message.Subject = EmailConstants.PasswordRecoverOtp;

                message.Body = new TextPart("plain")
                {
                    Text = "Hi ".ConcatString(emailParameter.UserName, ", ",
                    "\r\r\tYour One Time Password for recover your account password is: ", emailParameter.Otp,
                    "\r\rThanks and regards\nAdmin")
                };
            }
            else if (emailParameter.BookingDetails != null)
            {
                message.Subject = EmailConstants.BookingSuccessfull;

                BodyBuilder builder = new BodyBuilder
                {
                    TextBody = "Hi ".ConcatString(emailParameter.UserName, ", ",
                    "\r\r\tCongrats!!. Your booking has done successfully. \r\rYour booking details are following:",
                    "\r\r\tYour mobile number: ", emailParameter.BookingDetails.MobileNo,
                    "\r\tYour email id: ", emailParameter.BookingDetails.EmailId,
                    "\r\tBooking date: ", emailParameter.BookingDetails.BookingDate,
                    "\r\tBooked by: ", emailParameter.BookingDetails.BookedBy, 
                    "\r\tTotal room: ", emailParameter.BookingDetails.TotalRooms,
                    "\r\tTotal flour: ", emailParameter.BookingDetails.TotalFlours,
                    "\r\tTotal amount: ", emailParameter.BookingDetails.TotalAmount,
                    "\r\tAdvance given by you: ", emailParameter.BookingDetails.Advanced,
                    "\r\tRemaining balance: ", emailParameter.BookingDetails.Balance,
                    "\r\tConfirmation: ", emailParameter.BookingDetails.Confirmation,
                    "\r\rThanks and regards\nAdmin")
                };

                builder = await AddAttachment(emailParameter.BookingDetails.PhotoName, builder);
                message.Body = builder.ToMessageBody();
            }
            else if (emailParameter.UsersCred != null && emailParameter.UsersPersonalDetail != null)
            {
                message.Subject = EmailConstants.AccountCreation;

                BodyBuilder builder = new BodyBuilder
                {
                    TextBody = "Hi ".ConcatString(emailParameter.UserName, ", ",
                    "\r\r\tCongrats!!. Your account has been created successfully. \r\rYour account details are following:",
                    "\r\r\tYour mobile number: ", emailParameter.UsersCred.Mobile,
                    "\r\tYour email id: ", emailParameter.UsersPersonalDetail.EmailId,
                    "\r\tYour address: ", emailParameter.UsersPersonalDetail.Address,
                    ", country: ", emailParameter.UsersPersonalDetail.Country ?? string.Empty,
                    ", state: ", emailParameter.UsersPersonalDetail.State ?? string.Empty,
                    ", city: ", emailParameter.UsersPersonalDetail.City ?? string.Empty,
                    "\r\tYour account is active and created on ", emailParameter.UsersCred.CreationDate,
                    "\r\rThanks and regards\nAdmin")
                };

                builder = await AddAttachment(emailParameter.PhotoDetails.LocalPhotoName, builder);
                message.Body = builder.ToMessageBody();
            }

            return message;
        }

        private static async Task<BodyBuilder> AddAttachment(string photoName, BodyBuilder builder)
        {
            try
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                    GetFolderAsync(ApplicationConstants.Document);

                if (await storageFolder.TryGetItemAsync(photoName) is StorageFile file)
                {
                    ContentType contentType = new ContentType(EmailConstants.ContentType, string.Empty);
                    byte[] data = await PhotoUtility.GetByteArrayFromEncryptedPhoto(file);
                    MimeEntity mimeEntity = PhotoUtility.AddMimeEntity(file.Name, data, contentType);

                    builder.Attachments.Add(mimeEntity);
                }
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Email Attachment Exception: ".ConcatString(ex.Message));
            }

            return builder;
        }
    }
}
