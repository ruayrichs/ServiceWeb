using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNA.Lib.Catalog.Entity
{
    public class CatalogEntity
    {
        public string SID { get; set; }
        public string ItmNumber { get; set; }
        public string ItmDescription { get; set; }
        public string DescForeign { get; set; }
        public string PictureFile { get; set; }
        public string ItmGroup { get; set; }
        public string MatHierarchy { get; set; }
        public string HierarchyType { get; set; }
        public int amount { get; set; }
        public string NameCatalog { get; set; }
        public string SubNameCatalog { get; set; }
        public string Language { get; set; }
        public decimal MarketPrice { get; set; }
        public string Currency { get; set; }
        public string UOM { get; set; }
    }
}
