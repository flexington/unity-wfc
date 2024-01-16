using UnityEngine;
using UnityEngine.Tilemaps;

namespace flexington.WFC
{
    /// <summary>
    /// Represents a tile for the WFC algorithm.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// The TileBase associated with this tile.
        /// </summary>
        public TileBase TileBase { get; private set; }

        /// <summary>
        /// The Unity Tile object associated with this tile.
        /// </summary>
        private UnityEngine.Tilemaps.Tile _tile;

        /// <summary>
        /// Gets the index of the tile.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The hashes of the edges of the tile.
        /// </summary>
        public Hash Hash { get; private set; }

        /// <summary>
        /// Instantiates a new instance of the <see cref="Tile"/> class.
        /// </summary>
        public Tile(TileBase tileBase, int index)
        {
            TileBase = tileBase;
            Index = index;

            _tile = tileBase as UnityEngine.Tilemaps.Tile;
            Hash = GetHash();
        }

        /// <summary>
        /// Creating the hashes for the edges of the tile.
        /// </summary>
        private Hash GetHash()
        {
            RectInt rect = new RectInt
            {
                x = (int)_tile.sprite.textureRect.x,
                y = (int)_tile.sprite.textureRect.y,
                width = (int)_tile.sprite.textureRect.width,
                height = (int)_tile.sprite.textureRect.height
            };
            Color[] pixels = _tile.sprite.texture.GetPixels(rect.x, rect.y, rect.width, rect.height);

            Texture2D hashTexture = new Texture2D(rect.width, rect.height);
            hashTexture.SetPixels(pixels);
            hashTexture.Apply();

            return new Hash(hashTexture);
        }

        /// <summary>
        /// Returns a string representation of the tile object.
        /// </summary>
        /// <returns>A string representation of the tile object.</returns>
        public override string ToString()
        {
            return Index.ToString();
        }
    }
}