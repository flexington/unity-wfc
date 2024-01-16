using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace flexington.WFC
{
    public class InputManager
    {
        /// <summary>
        /// The tilemap used by the input manager.
        /// </summary>
        private Tilemap _tilemap;

        /// <summary>
        /// Temporary storage of the tiles read from the tilemap.
        /// </summary>
        private Queue<TileBase> _tiles;

        /// <summary>
        /// The 2D array of TileBase objects representing the grid used by the WFC algorithm.
        /// </summary>
        private Tile[,] _grid;

        /// <summary>
        /// A dictionary that maps tile indices to TileBase objects.
        /// </summary>
        private Dictionary<TileBase, int> _indices;

        /// <summary>
        /// The bottom left corner of the input image.
        /// </summary>
        private Nullable<Vector2Int> _bottomLeft;

        /// <summary>
        /// The top right corner of the input image.
        /// </summary>
        private Nullable<Vector2Int> _topRight;

        /// <summary>
        /// The 2D array of TileBase objects representing the grid used by the WFC algorithm.
        /// </summary>
        public Tile[,] Grid => _grid;

        /// <summary>
        /// Initializes a new instance of the InputManager class with the specified tilemap.
        /// </summary>
        /// <param name="tilemap">The tilemap to use for input.</param>
        public InputManager(Tilemap tilemap)
        {
            _tilemap = tilemap;
        }

        /// <summary>
        /// Reads the tilemap and returns a 2D array of TileBase objects.
        /// </summary>
        /// <returns>A 2D array of TileBase objects representing the tilemap.</returns>
        public Tile[,] ReadTilemap()
        {
            ReadInput();
            VerifyInput();
            MakeIndices();
            MakeGrid();

            return _grid;
        }

        /// <summary>
        /// Reads the input from the given tilemap and returns a queue of tiles.
        /// </summary>
        /// <param name="tilemap">The tilemap to read input from.</param>
        private void ReadInput()
        {
            var tiles = _tilemap.GetTilesBlock(_tilemap.cellBounds);
            _tiles = new Queue<TileBase>();
            for (int y = 0; y < _tilemap.cellBounds.size.y; y++)
            {
                for (int x = 0; x < _tilemap.cellBounds.size.x; x++)
                {
                    var index = x + (y * _tilemap.cellBounds.size.x);
                    TileBase tile = tiles[index];
                    if (tile == null) continue;

                    if (_bottomLeft == null) _bottomLeft = new Vector2Int(x, y);

                    _topRight = new Vector2Int(x, y);
                    _tiles.Enqueue(tile);
                }
            }
        }

        /// <summary>
        /// Verifies the input tiles and throws an exception if they do not meet the expected criteria.
        /// </summary>
        /// <param name="tiles">The queue of tiles to verify.</param>
        private void VerifyInput()
        {
            if (_tiles.Count == 0) throw new Exception("No tiles found in input image");
            var first = _tiles.First();
            var last = _tiles.Last();
            var width = _topRight.Value.x - _bottomLeft.Value.x + 1;
            var height = _topRight.Value.y - _bottomLeft.Value.y + 1;
            var expectedCount = width * height;
            if (_tiles.Count != expectedCount) throw new Exception($"The input image must be a rectangle without holes or outliers.\nExpected {expectedCount} tiles, but found {_tiles.Count}");
        }

        /// <summary>
        /// Creates a 2D grid of TileBase objects from a queue of tiles and a given size.
        /// </summary>
        /// <param name="tiles">The queue of tiles to use for the grid.</param>
        /// <param name="size">The size of the grid to create.</param>
        /// <returns>The 2D grid of TileBase objects.</returns>
        private void MakeGrid()
        {
            var size = _topRight.Value - _bottomLeft.Value + Vector2Int.one;
            _grid = new Tile[size.x, size.y];
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    var tile = _tiles.Dequeue();

                    _grid[x, y] = new Tile(tile, _indices[tile]);
                }
            }
        }

        /// <summary>
        /// Creates a dictionary of indices for each unique tile in the input set.
        /// </summary>
        private void MakeIndices()
        {
            _indices = new Dictionary<TileBase, int>();
            int index = 0;
            foreach (var tile in _tiles)
            {
                if (!_indices.ContainsKey(tile)) _indices.Add(tile, index++);
            }
        }
    }
}