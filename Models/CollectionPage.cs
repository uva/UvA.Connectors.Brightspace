using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.Connectors.Brightspace.Models
{
    class CollectionPage<T>
    {
        public T[] Items { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    class PagingInfo
    {
        public string Bookmark { get; set; }
        public bool HasMoreItems { get; set; }
    }
}
