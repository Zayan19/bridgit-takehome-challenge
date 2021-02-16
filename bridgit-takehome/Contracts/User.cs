using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tasklify.Contracts
{
    public class TasklifyUser
    {
        [JsonProperty("id", NullValueHandling=NullValueHandling.Ignore)]
        public int Id {get; private set;}
        [JsonProperty("email")]
        public string Email {get; set;}
        [JsonProperty("name")]
        public string Name {get; set;}

        public TasklifyUser(int id, string email, string name)
        {
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(name))
            {
                Id = id;
                Email = email;
                Name = name;
            }
        }
    }
}