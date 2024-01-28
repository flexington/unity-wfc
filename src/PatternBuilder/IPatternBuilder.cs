namespace flexington.WFC
{
    /// <summary>
    /// Represents a pattern builder that is used to construct instances of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of pattern to build.</typeparam>
    public interface IPatternBuilder<T> where T : IPattern, new()
    {
        /// <summary>
        /// Sets the size of the pattern.
        /// </summary>
        /// <param name="width">The width of the pattern.</param>
        /// <param name="height">The height of the pattern.</param>
        /// <returns>The pattern builder instance.</returns>
        IPatternBuilder<T> WithSize(int width, int height);

        /// <summary>
        /// Sets the tiles for the pattern builder.
        /// </summary>
        /// <param name="tiles">The tiles to use for building the pattern.</param>
        /// <returns>The pattern builder instance.</returns>
        IPatternBuilder<T> WithTiles(ITile[,] tiles);

        /// <summary>
        /// Sets the tile at the specified position.
        /// </summary>
        /// <param name="x">The x-coordinate of the position.</param>
        /// <param name="y">The y-coordinate of the position.</param>
        /// <param name="tile">The tile to set.</param>
        /// <returns>The pattern builder instance.</returns>
        IPatternBuilder<T> SetTile(int x, int y, ITile tile);

        /// <summary>
        /// Builds the pattern using the specified tiles.
        /// </summary>
        /// <returns>The constructed pattern.</returns>
        IPattern Build();
    }
}