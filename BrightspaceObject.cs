using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UvA.Connectors.Brightspace
{
    public abstract class BrightspaceObject
    {
        public BrightspaceConnector Connector { get; set; }

        Dictionary<Type, BrightspaceObject[]> Related;

        protected T[] GetRelated<T>() where T : BrightspaceObject
            => Related.GetValueOrDefault(typeof(T)) as T[];

        HashSet<string> UrlCache;

        protected async Task<T[]> RetrieveRelated<T>(string path, Action<T> init = null, bool paged = false) where T : BrightspaceObject
        {
            if (Related == null)
            {
                Related = new Dictionary<Type, BrightspaceObject[]>();
                UrlCache = new HashSet<string>();
            }
            if (!UrlCache.Contains(path))
            {
                T[] res;
                try
                {
                    res = paged ? (await Connector.GetPagedCollection<T>(path)).ToArray() : await Connector.Get<T[]>(path);
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    res = new T[0]; // Brightspace gives a 404 for empty collections
                }
                lock (Related)
                {
                    if (Related.ContainsKey(typeof(T)))
                        Related[typeof(T)] = res.Union(Related[typeof(T)]).Cast<T>().ToArray();
                    else
                        Related[typeof(T)] = res;
                    UrlCache.Add(path);
                }
                foreach (var obj in res)
                {
                    obj.Connector = Connector;
                    init?.Invoke(obj);
                }
            }
            return Related[typeof(T)] as T[];
        }
    }
}
