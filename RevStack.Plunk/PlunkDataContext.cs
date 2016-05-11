using System;
using Plunk.Api;
using PlunkClient = Plunk.Client;

namespace RevStack.Plunk
{
    public class PlunkDataContext
    {
        private DefaultApi _api = null;
        private readonly string _appId = null;
        private readonly string _accessToken = null;

        public PlunkDataContext(string appId, string accessToken)
        {
            _appId = appId;
            if (!accessToken.Contains("Bearer"))
                _accessToken = "Bearer " + accessToken.Trim();
        }

        public DefaultApi Client()
        {
            _api = new DefaultApi(new PlunkClient.Configuration(new PlunkClient.ApiClient()));
            _api.Configuration.AddDefaultHeader("Authorization", _accessToken);
            return _api;
        }

        public string AppId
        {
            get
            {
                return _appId;
            }
        }
    }
}
