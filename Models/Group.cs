using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.Connectors.Brightspace.Models
{
    public class Group : BrightspaceObject
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int[] Enrollments { get; set; }
    }
}
