using CommonServices.Model;
using DbModel = CommonServices.Model.DbModel;

namespace CommonServices.Extensions
{
    public static class UsersExtension
    {
        public static DbModel.UsersCred ToDbUsers(this UsersCred usersCred)
        {
            DbModel.UsersCred dbUsersCred = new DbModel.UsersCred
            {
                Id = usersCred.Id,
                EmailId = usersCred.EmailId,
                Mobile = usersCred.Mobile,
                CreationDate = usersCred.CreationDate,
                IsAdmin = usersCred.IsAdmin,
                Password = usersCred.Password,
                PasswordChanged = usersCred.PasswordChanged,
                UserId = usersCred.UserId
            };

            return dbUsersCred;
        }

        public static UsersCred ToUsersCred(this DbModel.UsersCred dbUsersCred)
        {
            UsersCred UsersCred = new UsersCred
            {
                Id = dbUsersCred.Id,
                EmailId = dbUsersCred.EmailId,
                Mobile = dbUsersCred.Mobile,
                CreationDate = dbUsersCred.CreationDate,
                IsAdmin = dbUsersCred.IsAdmin,
                Password = dbUsersCred.Password,
                PasswordChanged = dbUsersCred.PasswordChanged,
                UserId = dbUsersCred.UserId
            };

            return UsersCred;
        }

        public static DbModel.UsersPersonalDetail ToDbUsersPersonalDetail(this UsersPersonalDetail usersPersonalDetail)
        {
            DbModel.UsersPersonalDetail dbUsersPersonalDetail = new DbModel.UsersPersonalDetail
            {
                Id = usersPersonalDetail.Id,
                Address = usersPersonalDetail.Address,
                Country = usersPersonalDetail.Country,
                FullName = usersPersonalDetail.FullName,
                City = usersPersonalDetail.City,
                EmailId = usersPersonalDetail.EmailId,
                Mobile = usersPersonalDetail.Mobile,
                State = usersPersonalDetail.State
            };

            return dbUsersPersonalDetail;
        }

        public static UsersPersonalDetail ToUsersPersonalDetail(this DbModel.UsersPersonalDetail dbUsersPersonalDetail)
        {
            UsersPersonalDetail usersPersonalDetail = new UsersPersonalDetail
            {
                Id = dbUsersPersonalDetail.Id,
                Address = dbUsersPersonalDetail.Address,
                Country = dbUsersPersonalDetail.Country,
                FullName = dbUsersPersonalDetail.FullName,
                City = dbUsersPersonalDetail.City,
                EmailId = dbUsersPersonalDetail.EmailId,
                Mobile = dbUsersPersonalDetail.Mobile,
                State = dbUsersPersonalDetail.State
            };

            return usersPersonalDetail;
        }
    }
}
