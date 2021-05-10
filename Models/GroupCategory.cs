using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UvA.Connectors.Brightspace.Models
{
    public class GroupCategory : BrightspaceObject
    {
        public Course Course { get; set; }

        public int GroupCategoryId { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("Groups")]
        public int[] GroupIds { get; set; }

        public Task<Group[]> GetGroups() =>
            RetrieveRelated<Group>($"/{Course.Identifier}/groupcategories/{GroupCategoryId}/groups/");
        [JsonIgnore]
        public Group[] Groups => GetRelated<Group>();
    }
}
