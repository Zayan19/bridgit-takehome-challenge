using Newtonsoft.Json;

namespace Tasklify.Contracts
{
    public class TasklifyTask
    {
        [JsonProperty("id", NullValueHandling=NullValueHandling.Ignore)]
        public int Id {get; private set;}
        [JsonProperty("summary")]
        public string Summary {get; set;}
        [JsonProperty("description", NullValueHandling=NullValueHandling.Ignore)]
        public string Description {get; set;}

        public TasklifyTask(int id, string summary, string description)
        {
            if (!string.IsNullOrWhiteSpace(summary))
            {
                Id = id;
                Summary = summary;
                Description = description;
            }
        }
    }
}