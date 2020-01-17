using System;
using System.Collections.Generic;

namespace DemoTreeView.Model
{
    [Serializable]
    public class XamlItemGroup
    {
        public List<XamlItemGroup> Children { get; } = new List<XamlItemGroup>();
        public List<XamlItem> XamlItems { get; } = new List<XamlItem>();

        public string Name { get; set; }
        public int GroupId { get; set; }
    }
}
