using BookingServices.Interfaces;
using BookingServices.Repository.Interface;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBModel = CommonServices.Model.DbModel;

namespace BookingServices.DataProvider
{
    public class UsersPersonalDetailDataProvider : IUsersPersonalDetailDataProvider
    {
        private IUsersPersonalDetailRepository _usersPersonalDetailRepository;

        public UsersPersonalDetailDataProvider(
            IUsersPersonalDetailRepository usersPersonalDetailRepository)
        {
            _usersPersonalDetailRepository = usersPersonalDetailRepository ?? throw new ArgumentNullException(nameof(usersPersonalDetailRepository)); 
        }

        public bool AddOrUpdateUsersPersonalDetail(UsersPersonalDetail usersPersonalDetail)
        {
            DBModel.UsersPersonalDetail dbUsersPersonalDetail = usersPersonalDetail.ToDbUsersPersonalDetail();

            if(dbUsersPersonalDetail != null)
                return _usersPersonalDetailRepository.AddOrUpdateUsersPersonalDetail(dbUsersPersonalDetail);

            return false;
        }

        public UsersPersonalDetail GetUsersPersonalDetail(string emailId)
        {
            DBModel.UsersPersonalDetail userPersonalDetail = _usersPersonalDetailRepository.GetUsersPersonalDetail(emailId);

            if (userPersonalDetail != null)
                return userPersonalDetail.ToUsersPersonalDetail();

            return null;
        }
    }
}
