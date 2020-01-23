using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNA.Lib.Catalog.Entity
{
    public class CatalogHierarchyEntity
    {
        public string NodeID { get; set; }
        public string NodeParentID { get; set; }
        public string NodeName { get; set; }
        public string NodeType { get; set; }
        public int NodeLevel { get; set; }
        public string HierarchyCode { get; set; }
        public string HierarchyType { get; set; }

        public string CountData { get; set; }
    }
}
