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
         [JsonProperty("assignee_id", NullValueHandling=NullValueHandling.Ignore)]
        public int? Assignee_Id {get; set;}

        public TasklifyTask(int id, string summary, string description, int? assignee_id = null)
        {
            Id = id;
            Summary = summary;
            Description = description;
            Assignee_Id = assignee_id;
        }

        public TasklifyTask Clone()
        {
            return new TasklifyTask(Id, Summary, Description, Assignee_Id);
        }
    }
}