using UnityEngine;

namespace flexington.WFC
{
    /// <summary>
    /// Represents a tile in a WFC model.
    /// </summary>
    public interface ITile
    {
        /// <summary>
        /// Gets the sprite associated with the tile.
        /// </summary>
        public Sprite Sprite { get; set; }
    }
}