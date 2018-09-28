using IdentityServer4.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace idsrv.server.Model
{
    public class ApiResourceModel
    {
        public string ApiResourceData { get; set; }

        [Key]
        public string ApiResourceName { get; set; }

        [NotMapped]
        public ApiResource ApiResource { get; set; }

        public ApiResourceModel()
        {
        }

        public ApiResourceModel(ApiResource apiResource)
        {
            ApiResource = apiResource;
            Add();
        }

        public void Add()
        {
            ApiResourceData = JsonConvert.SerializeObject(ApiResource);
            ApiResourceName = ApiResource.Name;
        }

        public void Map()
        {
            ApiResource = JsonConvert.DeserializeObject<ApiResource>(ApiResourceData);
            ApiResourceName = ApiResource.Name;
        }
    }
}
