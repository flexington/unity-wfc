using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace flexington.WFC
{
    /// <summary>
    /// Represents a hash of the top, right, bottom, and left edges of a texture.
    /// </summary>
    public readonly struct Hash
    {
        /// <summary>
        /// Returns the hash of the top edge of the texture.
        /// </summary>
        public readonly int Top { get; }

        /// <summary>
        /// Returns the hash of the right edge of the texture.
        /// </summary>
        public readonly int Right { get; }

        /// <summary>
        /// Returns the hash of the bottom edge of the texture.
        /// </summary>
        public readonly int Bottom { get; }

        /// <summary>
        /// Returns the hash of the left edge of the texture.
        /// </summary>
        public readonly int Left { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> struct with the specified texture.
        /// </summary>
        /// <param name="texture">The texture to hash.</param>
        /// <param name="samples">The number of samples to take from each edge.</param>
        public Hash(Texture2D texture, int samples = 3)
        {
            StringBuilder top = new StringBuilder();
            StringBuilder right = new StringBuilder();
            StringBuilder bottom = new StringBuilder();
            StringBuilder left = new StringBuilder();

            var xStep = Mathf.FloorToInt((texture.width - 1) / (samples - 1));
            var yStep = Mathf.FloorToInt((texture.height - 1) / (samples - 1));

            for (int i = 0; i < samples; i++)
            {
                top.Append(texture.GetPixel(i * xStep, texture.height - 1));
                right.Append(texture.GetPixel(texture.width - 1, i * yStep));
                bottom.Append(texture.GetPixel(i * xStep, 0));
                left.Append(texture.GetPixel(0, i * yStep));
            }

            Top = top.ToString().GetHashCode();
            Right = right.ToString().GetHashCode();
            Bottom = bottom.ToString().GetHashCode();
            Left = left.ToString().GetHashCode();
        }

        public Hash(string top, string right, string bottom, string left)
        {
            Top = top.GetHashCode();
            Right = right.GetHashCode();
            Bottom = bottom.GetHashCode();
            Left = left.GetHashCode();
        }
    }
}