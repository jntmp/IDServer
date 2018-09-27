using IdentityServer4.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yunify.Auth.Server.Model
{
    public class ClientModel
    {
        public string ClientData { get; set; }

        [Key]
        public string ClientId { get; set; }

        [NotMapped]
        public Client Client { get; set; }
        
        public ClientModel()
        {
        }

        public ClientModel(Client client)
        {
            Client = client;
            Add();
        }
        
        public void Add()
        {
            ClientData = JsonConvert.SerializeObject(Client);
            ClientId = Client.ClientId;
        }

        public void Map()
        {
            Client = JsonConvert.DeserializeObject<Client>(ClientData);
            ClientId = Client.ClientId;
        }
    }
}
