using System;
using System.Collections.Generic;
using RevStack.Plunk.Query;
using System.Linq.Expressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plunk.Api;
using PlunkClient = Plunk.Client;

namespace RevStack.Plunk
{
    public class PlunkQueryProvider : QueryProvider
    {
        private readonly DefaultApi _client;
        private readonly string _appId = null;

        public PlunkQueryProvider(PlunkDataContext context)
        {
            _client = context.Client();
            _appId = context.AppId;
        }

        public override object Execute(Expression expression)
        {
            string limit = "-1";
            string page = "-1";
            string top = "-1";
            string fetch = "*:-1";

            string query = this.Translate(expression);
            Type elementType = TypeSystem.GetElementType(expression.Type);
            JObject obj = (JObject)_client.V1AppidDatastoreQuerySqlGet(query, _appId, limit: limit, page: page, top: top, fetch: fetch);
            var array = obj.Value<JArray>("data");
            object results = JsonConvert.DeserializeObject(array.ToString(), typeof(IEnumerable<>).MakeGenericType(elementType));
            return results;
        }
    }

}
