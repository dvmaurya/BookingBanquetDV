using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Utilities;
using GoogleAnalytics;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

namespace CommonServices.Analytcis
{
    public class AnalyticsManager : IAnalyticsManager
    {
        private const string _analyticsId = "UA-216092568-1";
        private const string _systemName = "SystemName";
        private const string _systemDevice = "Device";
        private const string _systemArchitecture = "SystemArchitecture";
        private const string _systemType = "Systemtype";
        private const string _systemOSBuildVersion = "OSBuildVersion";
        private const string _systemDeviceModel = "DeviceModel";
        private readonly Tracker _tracker;
        private static readonly IAnalyticsManager _analyticsManager = new AnalyticsManager();
        private string _mobileNo;
        private string _emailId;

        private AnalyticsManager()
        {
            _tracker = GoogleAnalytics.AnalyticsManager.Current.CreateTracker(_analyticsId);
            GoogleAnalytics.AnalyticsManager.Current.AutoAppLifetimeMonitoring = true;
            GoogleAnalytics.AnalyticsManager.Current.AutoTrackNetworkConnectivity = true;
            ResetEmailAndMobile();

            Dictionary<string, string> deviceInfo = GetDeviceInfo();

            TraceEventsDevice(
                analyticsEvent: AnalyticConstants.DeviceInfo,
                action: AnalyticConstants.OSBuild,
                label: deviceInfo[_systemOSBuildVersion]);

            TraceEventsDevice(
                analyticsEvent: AnalyticConstants.DeviceInfo,
                action: AnalyticConstants.DeviceModel,
                label: deviceInfo[_systemDeviceModel]);

            TraceEvents(
                analyticsEvent: AnalyticConstants.Application,
                action: $"{AnalyticConstants.App} " +
                        $"{AnalyticConstants.Launched}");
        }

        public static IAnalyticsManager Instance => _analyticsManager;

        public void TraceEventsDevice(string analyticsEvent, string action, string label)
        {
            _tracker.Send(
                HitBuilder.CreateCustomEvent(
                    analyticsEvent,
                    action,
                    label).Build());
        }

        public void TraceEvents(string analyticsEvent, string action)
        {
            _tracker.Send(
                HitBuilder.CreateCustomEvent(
                    analyticsEvent,
                    action,
                    Labels(action)).
                    SetCustomDimension(AnalyticsCustomDimension.EmailId.ToInt32(), _emailId).
                    SetCustomDimension(AnalyticsCustomDimension.MobileNo.ToInt32(), _mobileNo).
                    Build());

        }

        private string Labels(string message)
        {
            GetLocalSettingValues();

            string networkStatus = Utilities.Utilities.IsInternetConnected() ? "Online" : "Offline";

            return (!string.IsNullOrEmpty(_mobileNo) ? $"{AnalyticConstants.MobileNo}: {_mobileNo} | " : string.Empty) +
                    $"{AnalyticConstants.NetworkStatus}: {networkStatus} | " +
                    $"{message}";
        }

        public void TraceScreenView(string screenName)
        {
            if (AnalyticConstants.LoginScreen.FormatStringEquals(screenName))
                ResetEmailAndMobile();

            GetLocalSettingValues();

            if (!string.IsNullOrEmpty(screenName))
            {
                _tracker.Send(HitBuilder.CreateScreenView(screenName).
                    SetCustomDimension(AnalyticsCustomDimension.EmailId.ToInt32(), _emailId).
                    SetCustomDimension(AnalyticsCustomDimension.MobileNo.ToInt32(), _mobileNo).
                    Build());
            }
        }

        private static Dictionary<string, string> GetDeviceInfo()
        {
            string SystemArchitecture = string.Empty;
            string systemVersion = string.Empty;

            if (AnalyticsInfo.VersionInfo != null &&
                AnalyticsInfo.VersionInfo.DeviceFamilyVersion != null)
            {
                ulong version = AnalyticsInfo.VersionInfo.DeviceFamilyVersion.UlongParse();
                ulong version1 = (version & 0xFFFF000000000000L) >> 48;
                ulong version2 = (version & 0x0000FFFF00000000L) >> 32;
                ulong version3 = (version & 0x00000000FFFF0000L) >> 16;
                ulong version4 = version & 0x000000000000FFFFL;
                systemVersion = $"{version1}.{version2}.{version3}.{version4}";
            }

            Package package = Package.Current;
            if (package != null && package.Id != null)
                SystemArchitecture = package.Id.Architecture.ToString();

            EasClientDeviceInformation easClientDeviceInformation = new EasClientDeviceInformation();

            Dictionary<string, string> DeviceInfo = new Dictionary<string, string>
            {
                { _systemName, easClientDeviceInformation.FriendlyName },
                { _systemDevice, easClientDeviceInformation.SystemManufacturer },
                { _systemArchitecture, SystemArchitecture },
                { _systemType, AnalyticsInfo.VersionInfo.DeviceFamily },
                { _systemOSBuildVersion, systemVersion },
                { _systemDeviceModel, easClientDeviceInformation.SystemProductName }
            };

            return DeviceInfo;
        }

        public void TraceEvents(string analyticsEvent, string action, string label)
        {
            _tracker.Send(
                HitBuilder.CreateCustomEvent(
                    analyticsEvent,
                    action,
                    Labels(label)).
                    SetCustomDimension(AnalyticsCustomDimension.EmailId.ToInt32(), _emailId).
                    SetCustomDimension(AnalyticsCustomDimension.MobileNo.ToInt32(), _mobileNo).
                    Build());
        }

        public void TraceExceptions(string description)
        {
            GetLocalSettingValues();

            Dictionary<string, string> deviceInfo = GetDeviceInfo();
            _tracker.Send(
                HitBuilder.CreateException(
                    (Utilities.Utilities.IsInternetConnected() ? "Online | " : "Offline | ") +
                    $"{DateTime.Now.ConvertToString("dd-MM-yyyy HH:mm")} | " +
                    $"{deviceInfo[_systemDevice]} " +
                    $"{deviceInfo[_systemOSBuildVersion]} + " +
                    $"{deviceInfo[_systemDeviceModel]} | " +
                    $"{description}",
                true).
                SetCustomDimension(AnalyticsCustomDimension.EmailId.ToInt32(), _emailId).
                SetCustomDimension(AnalyticsCustomDimension.MobileNo.ToInt32(), _mobileNo).
                Build());
        }

        public void GetLocalSettingValues()
        {
            string mobileNo = Helper.GetLocalSettingsValue<string>(ApplicationConstants.CurrentUserMobileNo);
            _mobileNo = !string.IsNullOrEmpty(mobileNo) ? mobileNo : string.Empty;

            string emailId = Helper.GetLocalSettingsValue<string>(ApplicationConstants.CurrentUserEmailId);
            _emailId = !string.IsNullOrEmpty(mobileNo) ? emailId : string.Empty;
        }

        private void ResetEmailAndMobile()
        {
            Helper.SetLocalSetting(ApplicationConstants.CurrentUserEmailId, string.Empty);
            Helper.SetLocalSetting(ApplicationConstants.CurrentUserMobileNo, string.Empty);
        }
    }
}
