using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Logger;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace CommonServices.Utilities
{
    public class HttpUtility
    {
        public static async Task<HttpResponseMessage> GetAsync(string linkText)
        {
            using (HttpClient httpClient = GetHttpClient())
            {
                using (CancellationTokenSource cancellationTokenSource = GetCancellationTokenSource())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(new Uri(linkText));
                    ValidateAccessToken(response);
                    return response;
                }
            }
        }

        private static void ValidateAccessToken(HttpResponseMessage response)
        {
            if (response != null && response.ReasonPhrase == ApplicationConstants.InvalidAccessToken)
                FileLogger.Logger.Info("HttpException: ".ConcatString(response.Content));
        }

        private static HttpClient GetHttpClient()
        {
            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;

            HttpClient httpClient = new HttpClient(filter);

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed in the called method. Added this to seperate method to elimate the duplicate code.")]
        public static CancellationTokenSource GetCancellationTokenSource()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(ApplicationConstants.DefaultApiTimeout);
            return cancellationTokenSource;
        }
    }
}
