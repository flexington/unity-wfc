using System;
using System.Collections.Generic;

namespace flexington.WFC
{
    /// <inheritdoc/>
    public class PatternBuilder<T> : IPatternBuilder<T> where T : IPattern, new()
    {
        private ITile[,] _tiles;
        private int _width;
        private int _height;

        /// <inheritdoc/>
        public IPatternBuilder<T> WithSize(int width, int height)
        {
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

            _width = width;
            _height = height;
            _tiles = new ITile[width, height];
            return this;
        }

        /// <inheritdoc/>
        public IPatternBuilder<T> WithTiles(ITile[,] tiles)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));

            _tiles = tiles;
            return this;
        }

        /// <inheritdoc/>
        public IPatternBuilder<T> SetTile(int x, int y, ITile tile)
        {
            if (_tiles == null) throw new InvalidOperationException("Cannot set tile before setting size.");
            if (x < 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0) throw new ArgumentOutOfRangeException(nameof(y));
            if(x >= _width) throw new ArgumentOutOfRangeException(nameof(x));
            if(y >= _height) throw new ArgumentOutOfRangeException(nameof(y));
            if (tile == null) throw new ArgumentNullException(nameof(tile));

            _tiles[x, y] = tile;
            return this;
        }



        /// <inheritdoc/>
        public IPattern Build()
        {
            if (_tiles == null) throw new InvalidOperationException("Cannot build pattern before setting size and tiles.");

            return new T
            {
                Tiles = _tiles
            };
        }
    }
}