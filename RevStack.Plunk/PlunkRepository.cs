using System;
using System.Collections.Generic;
using System.Linq;
using RevStack.Pattern;
using System.Linq.Expressions;
using Plunk.Api;
using Plunk.Client;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RevStack.Plunk.Utils;

namespace RevStack.Plunk
{
    public class PlunkRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        private readonly DefaultApi _client;
        private readonly string _appId = null;
        private readonly PlunkQueryProvider _queryProvider;

        public PlunkRepository(PlunkDataContext context)
        {
            _client = context.Client();
            _appId = context.AppId;
            _queryProvider = new PlunkQueryProvider(context);
        }

        public IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = new Query.Query<TEntity>(_queryProvider);
            return query.ToList().AsEnumerable<TEntity>();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return new Query.Query<TEntity>(_queryProvider).Where(predicate);
        }

        public TEntity Add(TEntity entity)
        {
            string entityStr = CamelCaseJsonSerializer.SerializeObject(entity);
            JObject json = JObject.Parse(entityStr);
            string name = entity.GetType().Name;
            json["@class"] = name;

            JObject obj = (JObject)_client.V1AppidDatastorePost(_appId, json);

            if (obj["error"] != null)
                throw new ApplicationException(obj["message"].ToString());

            return JsonConvert.DeserializeObject<TEntity>(obj.ToString());
        }

        public TEntity Update(TEntity entity)
        {
            Type type = entity.GetType();
            var info = type.GetProperty("Id");
            if (info == null)
                throw new ApplicationException("Id is required.");

            string entityStr = CamelCaseJsonSerializer.SerializeObject(entity);
            JObject json = JObject.Parse(entityStr);
            string name = entity.GetType().Name;
            json["@class"] = name;

            JObject obj = (JObject)_client.V1AppidDatastorePut(_appId, json);

            if (obj["error"] != null)
                throw new ApplicationException(obj["message"].ToString());

            return JsonConvert.DeserializeObject<TEntity>(obj.ToString());
        }

        public void Delete(TEntity entity)
        {
            Type type = typeof(TEntity);
            string query = PlunkUtils.GetEntityIdQueryFormat<TEntity>(entity);
            query = "DELETE FROM " + type.Name + " WHERE " + query;
            _client.V1AppidDatastoreCommandSqlGet(_appId, query);
        }
        
    }
}
