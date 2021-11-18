namespace CommonServices.Analytcis
{
    public interface IAnalyticsManager
    {
        void TraceScreenView(string screenName);
        void TraceEvents(string analyticsEvent, string action);
        void TraceEvents(string analyticsEvent, string action, string label);
        void TraceEventsDevice(string analyticsEvent, string action, string label);
        void TraceExceptions(string description);
    }
}
