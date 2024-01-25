using UnityEngine;

namespace flexington.WFC
{
    /// <summary>
    /// Represents the hash of a WFC object.
    /// </summary>
    public interface IHash
    {
        /// <summary>
        /// The hash of the top edge of the tile.
        /// </summary>
        public int Top { get; }

        /// <summary>
        /// The hash of the right edge of the tile.
        /// </summary>
        public int Right { get; }

        /// <summary>
        /// The hash of the bottom edge of the tile.
        /// </summary>
        public int Bottom { get; }

        /// <summary>
        /// The hash of the left edge of the tile.
        /// </summary>
        public int Left { get; }

        /// <summary>
        /// Gets the hash of the edge at the specified index.
        /// </summary>
        /// <param name="index">The index of the edge.</param>
        /// <returns>The hash of the edge.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is less than 0 or greater than 3.</exception>
        /// <remarks>
        /// The indices correspond to the following edges:
        /// 0: Top
        /// 1: Right
        /// 2: Bottom
        /// 3: Left
        /// </remarks>
        public int this[int index] { get; }
    }
}