using CommonServices.Extensions;
using System;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace CommonServices.Utilities
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
            return connectionProfile != null &&
                    connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }

        public static string GenerateOTP()
        {
            _generator = new Random();
            return _generator.Next(100000, 999999).ConvertToString();
        }

        public static string GetSha1Hashed(string input)
        {
            IBuffer data = CryptographicBuffer.ConvertStringToBinary(
                input, BinaryStringEncoding.Utf16LE);
            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("SHA1");
            IBuffer hashed = hasher.HashData(data);
            return CryptographicBuffer.EncodeToBase64String(hashed);
        }
    }
}
