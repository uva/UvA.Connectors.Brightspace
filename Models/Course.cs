using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.Connectors.Brightspace.Models
{
    public class Course : BrightspaceObject
    {
        public int Identifier { get; set; }

        public Task<Section[]> GetSections() 
            => RetrieveRelated<Section>($"/{Identifier}/sections/");
        public Section[] Sections => GetRelated<Section>();

        public Task<GroupCategory[]> GetGroupCategories()
            => RetrieveRelated<GroupCategory>($"/{Identifier}/groupCategories/", g => g.Course = this);
        public GroupCategory[] GroupCategories => GetRelated<GroupCategory>();

        public async Task<IEnumerable<Enrollment>> GetEnrollments(int? roleId = null)
            => (await RetrieveRelated<Enrollment>($"/enrollments/orgUnits/{Identifier}/users/" + (roleId == null ? "" : $"?roleId={roleId}"), paged: true))
                .Where(e => roleId == null || e.Role.Id == roleId);
        public Enrollment[] Enrollments => GetRelated<Enrollment>();
    }
}
