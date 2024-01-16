using System.Collections.Generic;
using UnityEngine;

namespace flexington.WFC
{
    public class Cell
    {
        public bool IsCollapsed { get; set; }

        public List<Pattern> Options { get; set; }

        public Pattern Pattern { get; set; }

        public Vector2Int Position { get; set; }

        public override string ToString()
        {
            return Options.Count.ToString();
        }
    }
}