using CommonServices.Extensions;
using System;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;

namespace BookingBanquetDV.Utility
{
    public static class Utilities
    {
        private static string VersionFormat = "{0}.{1}.{2}.{3}";
        private static Random _generator;

        public static string AppVersion
        {
            get
            {
                PackageVersion version = Package.Current.Id.Version;

                return VersionFormat.FormatString(
                        version.Major,
                        version.Minor,
                        version.Build,
                        version.Revision);
            }
        }

        public static bool IsInternetConnected()
        {
            ConnectionProfile connectionProfile = 
                NetworkInformation.GetInternetConnectionProfile();

            if (connectionProfile == null)
                return false;
            return (connectionProfile != null &&
                    connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }

        public static string GenerateOTP()
        {
            _generator = new Random();
            //return _generator.Next(0, 1000000).ToString("D6");
            return _generator.Next(100000, 999999).ConvertToString();
        }
    }
}
