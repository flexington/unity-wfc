namespace flexington.WFC
{
    /// <summary>
    /// Represents a pattern in a WFC model.
    /// </summary>
    public interface IPattern
    {
        /// <summary>
        /// The <see cref="Hash"/> of the pattern.
        /// </summary>
        IHash Hash { get; }

        /// <summary>
        /// The <see cref="Tiles"/> of the pattern.
        /// </summary>
        ITile[,] Tiles { get; }
    }
}