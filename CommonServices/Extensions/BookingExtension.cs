using CommonServices.Model;
using DbModel = CommonServices.Model.DbModel;

namespace CommonServices.Extensions
{
    public static class BookingExtension
    {
        public static DbModel.BookingDetails ToDbBookingDetails(this BookingDetails bookingDetails)
        {
            DbModel.BookingDetails dbBookingDetails = new DbModel.BookingDetails
            {
                Id = bookingDetails.Id,
                Advanced = bookingDetails.Advanced,
                Balance = bookingDetails.Balance,
                BookedBy = bookingDetails.BookedBy,
                BookingDate = bookingDetails.BookingDate,
                Confirmation = bookingDetails.Confirmation,
                CreationDate = bookingDetails.CreationDate,
                CustomerName = bookingDetails.CustomerName,
                EmailId = bookingDetails.EmailId,
                IsEmailSent = bookingDetails.IsEmailSent,
                MobileNo = bookingDetails.MobileNo,
                TotalAmount = bookingDetails.TotalAmount,
                TotalFlours = bookingDetails.TotalFlours,
                TotalRooms = bookingDetails.TotalRooms,
                UserId = bookingDetails.UserId,
                Address = bookingDetails.Address,
                Country = bookingDetails.Country,
                State = bookingDetails.State,
                City = bookingDetails.City
            };

            return dbBookingDetails;
        }

        public static BookingDetails ToBookingDetails(this DbModel.BookingDetails DbbookingDetails)
        {
            BookingDetails bookingDetails = new BookingDetails
            {
                Id = DbbookingDetails.Id,
                Advanced = DbbookingDetails.Advanced,
                Balance = DbbookingDetails.Balance,
                BookedBy = DbbookingDetails.BookedBy,
                BookingDate = DbbookingDetails.BookingDate,
                Confirmation = DbbookingDetails.Confirmation,
                CreationDate = DbbookingDetails.CreationDate,
                CustomerName = DbbookingDetails.CustomerName,
                EmailId = DbbookingDetails.EmailId,
                IsEmailSent = DbbookingDetails.IsEmailSent,
                MobileNo = DbbookingDetails.MobileNo,
                TotalAmount = DbbookingDetails.TotalAmount,
                TotalFlours = DbbookingDetails.TotalFlours,
                TotalRooms = DbbookingDetails.TotalRooms,
                UserId = DbbookingDetails.UserId,
                Address = DbbookingDetails.Address,
                Country = DbbookingDetails.Country,
                State = DbbookingDetails.State,
                City = DbbookingDetails.City
            };

            return bookingDetails;
        }
    }
}
