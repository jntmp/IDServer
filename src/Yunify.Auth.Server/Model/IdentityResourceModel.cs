using IdentityServer4.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yunify.Auth.Server.Model
{
    public class IdentityResourceModel
    {
        public string IdentityResourceData { get; set; }

        [Key]
        public string IdentityResourceName { get; set; }

        [NotMapped]
        public IdentityResource IdentityResource { get; set; }

        public IdentityResourceModel()
        {
        }

        public IdentityResourceModel(IdentityResource identityResource)
        {
            IdentityResource = identityResource;
            Add();
        }
        
        public void Add()
        {
            IdentityResourceData = JsonConvert.SerializeObject(IdentityResource);
            IdentityResourceName = IdentityResource.Name;
        }

        public void Map()
        {
            IdentityResource = JsonConvert.DeserializeObject<IdentityResource>(IdentityResourceData);
            IdentityResourceName = IdentityResource.Name;
        }
    }
}
