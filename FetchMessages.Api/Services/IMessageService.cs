using Messages.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messages.Api.Services
{
    public interface IMessageService
    {

        public Task<Message> GetEntity( string id, string flag);

        public Task<List<Message>> GetEntities();

        public Task<string> Add(string message , string flag);

        public void update( string id, string message, string flag);

        public void Delete(string id, string flag);

    }
}
