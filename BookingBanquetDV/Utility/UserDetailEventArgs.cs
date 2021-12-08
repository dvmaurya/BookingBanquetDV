using CommonServices.Enums;
using CommonServices.Model;

namespace BookingBanquetDV.Utility
{
    public class UserDetailEventArgs
    {
        private readonly UsersPersonalDetail _usersPersonalDetail;
        private readonly PreviewDialogAction _previewDialogAction;

        public UserDetailEventArgs(
            UsersPersonalDetail  usersPersonalDetail,
            PreviewDialogAction previewDialogAction)
        {
            _usersPersonalDetail = usersPersonalDetail;
            _previewDialogAction = previewDialogAction;
        }

        public UsersPersonalDetail UsersPersonalDetail => _usersPersonalDetail;

        public PreviewDialogAction PreviewDialogAction => _previewDialogAction;
    }
}

