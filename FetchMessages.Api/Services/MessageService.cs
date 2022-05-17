using Messages.Entity;
using Messages.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messages.Api.Services
{
    public class MessageService : IMessageService
    {
        public readonly IMessageDB _messageDB;

        public MessageService(IMessageDB messageDB)
        {
            _messageDB = messageDB;
        }

        public void validate(string text,string flag)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("message is empty");
            }
        }

        public Task<string> Add(string text, string flag)
        {
           
             validate(text,flag);
             var message = new Message()
                   {
                        id = Guid.NewGuid().ToString(),
                        value = text,
                        flag = flag
                   };
            return _messageDB.Add(message);
        }

        public void Delete(string id, string flag)
        {
            GetEntity(id,flag);
            _messageDB.Delete(id,flag);
        }

        public Task<Message> GetEntity(string id , string flag)
        {
            var message = _messageDB.Get(id,flag);
            if (message.Result == null)
            {
                throw new Exception($"message not found for given id: {id}");
            }
            return message;
        }

        public Task<List<Message>> GetEntities()
        {
            string queryString = "SELECT * FROM c";
            return _messageDB.GetEntities(queryString);
        }

        public void update(string id,string text , string flag)
        {
            validate(text,flag); 
            GetEntity(id,flag);
            var message = new Message()
            {
                id = id ,
                value = text ,
                flag = flag
            };
            _messageDB.update(message);
        }
    }
}
