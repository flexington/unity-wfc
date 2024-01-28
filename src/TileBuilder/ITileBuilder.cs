using UnityEngine;

namespace flexington.WFC
{
    /// <summary>
    /// Represents a builder for creating tiles.
    /// </summary>
    public interface ITileBuilder<T> where T : ITile, new()
    {
        /// <summary>
        /// Sets the sprite for the tile being built.
        /// </summary>
        /// <param name="sprite">The sprite to set.</param>
        /// <returns>The tile builder instance.</returns>
        TileBuilder<T> WithSprite(Sprite sprite);

        /// <summary>
        /// Builds the tile using the specified parameters.
        /// </summary>
        /// <returns>The built tile.</returns>
        ITile Build();
    }
}