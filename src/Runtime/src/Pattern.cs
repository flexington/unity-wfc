using System.Collections.Generic;
using UnityEngine;

namespace flexington.WFC
{
    public class Pattern
    {
        public Hash Hash { get; set; }

        public int Id { get; set; }

        public Tile[,] Tiles { get; set; }

        public Pattern(Tile[,] pattern)
        {
            Tiles = pattern;

            Hash = GetHash();
        }

        private Hash GetHash()
        {
            string top = "";
            string right = "";
            string bottom = "";
            string left = "";

            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                top += Tiles[x, Tiles.GetLength(1) - 1].Hash.Top.ToString();
                bottom += Tiles[x, 0].Hash.Bottom.ToString();
            }

            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                left += Tiles[0, y].Hash.Left.ToString();
                right += Tiles[Tiles.GetLength(0) - 1, y].Hash.Right.ToString();
            }

            var hash = new Hash(top, right, bottom, left);

            return hash;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}