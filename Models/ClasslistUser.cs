using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.Connectors.Brightspace.Models
{
    public class ClasslistUser
    {
        public string Identifier { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string OrgDefinedId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
    }

    public class Enrollment : BrightspaceObject
    {
        public User User { get; set; }
        public Role Role { get; set; }

        public override string ToString()
            => $"{Role}: {User}";
    }

    public class User
    {
        public string Identifier { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string OrgDefinedId { get; set; }

        public override string ToString() => DisplayName;
    }

    public class Role
    {
        public string Code { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
