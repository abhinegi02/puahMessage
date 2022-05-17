using Messages.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Repo
{
    public interface IMessageDB
    {
        public Task<Message> Get(string id , string flag);

        public Task<List<Message>> GetEntities(string queryString);

        public Task<string> Add(Message message);

        public Task update(Message message);

        public Task Delete(string id , string flag);
    }
}
