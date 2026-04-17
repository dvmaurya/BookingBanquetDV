using CommonServices.Analytcis;
using System;

namespace BookingServices.Repository
{
    public abstract class BaseRepository
    {
        internal IAnalyticsManager _analyticsManager;

        protected BaseRepository(IAnalyticsManager analyticsManager)
        {
            _analyticsManager = analyticsManager ?? throw new ArgumentNullException(nameof(analyticsManager));
        }

        protected void TraceEvents(string analyticsEvent, string action)
        {
            _analyticsManager.TraceEvents(analyticsEvent, action);
        }

        protected void TraceEvents(string analyticsEvent, string action, string label)
        {
            _analyticsManager.TraceEvents(analyticsEvent, action, label);
        }

        protected void TraceExceptions(string label)
        {
            _analyticsManager.TraceExceptions(label);
        }

        protected static string FormatAdditionalInfo(string caption, string value)
        {
            return
                !string.IsNullOrEmpty(value)
                ?
                $"{caption} {value}"
                :
                null;
        }
    }
}
