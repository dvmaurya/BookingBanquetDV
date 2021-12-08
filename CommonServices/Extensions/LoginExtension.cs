using CommonServices.Model;
using DbModel = CommonServices.Model.DbModel;

namespace CommonServices.Extensions
{
    public static class LoginExtension
    {
        public static DbModel.LoginDetail ToDbLoginDetail(this LoginDetail loginDetail)
        {
            DbModel.LoginDetail dbLoginDetail = new DbModel.LoginDetail
            {
                Id = loginDetail.Id,
                NumberOfLogin = loginDetail.NumberOfLogin,
                IsRememberChecked = loginDetail.IsRememberChecked,
                LastLogin = loginDetail.LastLogin,
                UserId = loginDetail.UserId
            };

            return dbLoginDetail;
        }

        public static LoginDetail ToLoginDetail(this DbModel.LoginDetail dbLoginDetail)
        {
            return new LoginDetail
            {
                Id = dbLoginDetail.Id,
                NumberOfLogin = dbLoginDetail.NumberOfLogin,
                IsRememberChecked = dbLoginDetail.IsRememberChecked,
                LastLogin = dbLoginDetail.LastLogin,
                UserId = dbLoginDetail.UserId
            };
        }
    }
}
