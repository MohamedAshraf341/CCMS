namespace CCMS.FE.UI.Services
{
    public class ApiClientBase
    {
        protected ApiHttpClient ApiHttpClient { get; private set; }
        public ApiClientBase(ApiHttpClient _apiHttpClient)
        {
            ApiHttpClient = _apiHttpClient;
        }
    }
}
