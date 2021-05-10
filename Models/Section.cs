using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UvA.Connectors.Brightspace.Models
{
    public class Section : BrightspaceObject
    {
        public int SectionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int[] Enrollments { get; set; }   
    }
}
