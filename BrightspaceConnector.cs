using D2L.Extensibility.AuthSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using UvA.Connectors.Brightspace.Models;

namespace UvA.Connectors.Brightspace
{
    public class BrightspaceConnector
    {
        ID2LUserContext UserContext;
        HttpClient Client;

        static string LePart => "le/1.52";
        static string LpPart => "lp/1.30";
        static string BasePath = "/d2l/api/";

        /// <summary>
        /// Creates the Brightspace connector
        /// </summary>
        /// <param name="url">Full url to the Brightspace instance, including scheme</param>
        /// <param name="appId">Application Id</param>
        /// <param name="appKey">Application key</param>
        /// <param name="userId">User Id</param>
        /// <param name="userKey">User key</param>
        public BrightspaceConnector(string url, string appId, string appKey, string userId, string userKey)
        {
            var uri = new Uri(url);
            var appContext = new D2LAppContextFactory().Create(appId, appKey);
            UserContext = appContext.CreateUserContext(userId, userKey, new HostSpec("https", uri.Host, 443));

            Client = new HttpClient();
            Client.BaseAddress = uri;
        }

        internal async Task<T> Get<T>(string path)
        {
            if (!path.StartsWith("lp") && !path.StartsWith("le"))
                path = LpPart + path;
            var url = BasePath + path;
            var uri = UserContext.CreateAuthenticatedUri(new Uri(Client.BaseAddress, url), "GET");
            return await Client.GetFromJsonAsync<T>(uri);
        }

        internal async Task<IEnumerable<T>> GetPagedCollection<T>(string path)
        {
            var current = path;
            var marker = path.Contains("?") ? "&" : "?";
            var results = new List<T>();
            CollectionPage<T> res;
            do
            {
                res = await Get<CollectionPage<T>>(current);
                current = $"{path}{marker}bookmark={res.PagingInfo.Bookmark}";
                results.AddRange(res.Items);
            }
            while (res.PagingInfo.HasMoreItems);
            return results;
        }

        /// <summary>
        /// Get course info
        /// </summary>
        /// <param name="orgUnitId">Target orgUnit Id</param>
        public Task<Course> GetCourseOffering(int orgUnitId)
            => Get<Course>($"{LpPart}/courses/{orgUnitId}");
        
        /// <summary>
        /// Get the sections for a course
        /// </summary>
        /// <param name="orgUnitId">Target orgUnit Id</param>
        public Task<Section[]> GetSections(int orgUnitId)
            => Get<Section[]>($"{LpPart}/{orgUnitId}/sections/");

        /// <summary>
        /// Get the class list for a course
        /// </summary>
        /// <param name="orgUnitId">Target orgUnit Id</param>
        public Task<ClasslistUser[]> GetClasslist(int orgUnitId)
            => Get<ClasslistUser[]>($"{LePart}/{orgUnitId}/classlist/");

        /// <summary>
        /// Gets all enrollments in a particular orgUnit
        /// </summary>
        /// <param name="orgUnitId">Target orgUnit Id</param>
        /// <param name="roleId">Optional. Get only enrollments with this role</param>
        public Task<IEnumerable<Enrollment>> GetEnrollments(int orgUnitId, int? roleId = null)
            => GetPagedCollection<Enrollment>($"{LpPart}/enrollments/orgUnits/{orgUnitId}/users/" + (roleId == null ? "" : $"?roleId={roleId}"));

        /// <summary>
        /// Get the group categories for a course
        /// </summary>
        /// <param name="orgUnitId">Target orgUnit Id</param>
        public Task<GroupCategory[]> GetGroupCategories(int orgUnitId)
            => Get<GroupCategory[]>($"{LpPart}/{orgUnitId}/groupcategories/");

        /// <summary>
        /// Get the groups for a group category
        /// </summary>
        /// <param name="orgUnitId">Target orgUnit Id</param>
        /// <param name="groupCategoryId">Target group category Id</param>
        public Task<Group[]> GetGroups(int orgUnitId, int groupCategoryId)
            => Get<Group[]>($"{LpPart}/{orgUnitId}/groupcategories/{groupCategoryId}/groups/");
    }
}
