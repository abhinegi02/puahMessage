using Messages.Entity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messages.Repo
{
    public class MessageDB : IMessageDB
    {
        private readonly IConfiguration _configuration;
        private readonly Container _container;
        public static CosmosClient Client { get; private set; }

        public MessageDB(IConfiguration configuration)
        {
            _configuration = configuration;
            var x = _configuration["endpoint"];//not working // now it is
            var y = _configuration["masterKey"];
            Client = new CosmosClient("https://azureserv.documents.azure.com:443/", "gqkxQFxrvasnM8knOcJPynxSv7c3VCSik6KzhIE15rh2DZNowWmodNUiWJwVXiyYyHIokRq8VTcb56fHj8BJrg==");

             _container = Client.GetContainer("messages", "messages");
        }
        public async Task<string> Add(Message message)
        {
            await _container.CreateItemAsync<Message>(message, new PartitionKey(message.flag));
            return message.id;
        }

        public async Task Delete(string id,string flag)
        {
            await _container.DeleteItemAsync<Message>(id,new PartitionKey(flag));
        }

        public async Task<Message> Get(string id,string flag)
        {
            try
            {
                ItemResponse<Message> message = await _container.ReadItemAsync<Message>(id, new PartitionKey(flag));
                return message.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<Message>> GetEntities(string queryString)
        {
            var query = _container.GetItemQueryIterator<Message>(new QueryDefinition(queryString));
            List<Message> results = new List<Message>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task update(Message message)
        {
           // await _container.UpsertItemAsync<Message>(message, new PartitionKey(message.flag));
            await _container.ReplaceItemAsync<Message>(message,message.id, new PartitionKey(message.flag));
        }
    }
}
