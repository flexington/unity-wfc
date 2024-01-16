using System.Collections.Generic;
using System.Linq;
using flexington.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace flexington.WFC
{
    /// <summary>
    /// Provides functionality to generate patterns from the input data.
    /// </summary>
    public class PatternManager
    {
        /// <summary>
        /// The grid of tiles provided by the <see cref="InputManager"/>.
        /// </summary>
        private Tile[,] _grid;

        /// <summary>
        /// The size of the patterns to be generated.
        /// </summary>
        private Vector2Int _patternSize;

        /// <summary>
        /// The size of the grid of tiles provided by the <see cref="InputManager"/>.
        /// </summary>
        private Vector2Int _gridSize;

        /// <summary>
        /// Indicates whether the patterns overlap with each other or not.
        /// </summary>
        private readonly bool _isOverlapping;

        /// <summary>
        /// Indicates whether the patterns wrap around the edges of the grid or not.
        /// </summary>
        private readonly bool _isWrapping;

        /// <summary>
        /// A dictionary that maps pattern IDs to 2D arrays of TileBase objects.
        /// </summary>
        private Queue<Tile[,]> _patterns;

        public Vector2Int PatternGridSize { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="PatternManager"/> class.
        /// </summary>
        /// <param name="grid">The grid of tiles provided by the <see cref="InputManager"/>.</param>
        /// <param name="patternSize">The size of the patterns to be generated.</param>        
        public PatternManager(Tile[,] grid, Vector2Int patternSize, bool isOverlapping = false, bool isWrapping = false)
        {
            _grid = grid;
            _patternSize = patternSize;
            _gridSize = new Vector2Int(grid.GetLength(0), grid.GetLength(1));
            _isOverlapping = isOverlapping;
            _isWrapping = isWrapping;
        }

        /// <summary>
        /// Processes the grid and returns a dictionary of unique patterns and their corresponding integer values.
        /// </summary>
        /// <returns>A dictionary of unique patterns and their corresponding integer values.</returns>
        public List<Pattern> ProcessGrid()
        {
            var patterns = GetPatterns();
            return MakeIndices(patterns);
        }

        private Queue<Pattern> GetPatterns()
        {
            var patterns = new Queue<Pattern>();

            var xMax = 0;
            var yMax = 0;

            int xStep = _isOverlapping ? 1 : _patternSize.x;
            int yStep = _isOverlapping ? 1 : _patternSize.y;



            for (int x = 0; x < _gridSize.x; x += xStep)
            {
                for (int y = 0; y < _gridSize.y; y += yStep)
                {
                    var position = new Vector2Int(x, y);
                    var pattern = GetPatternAtPosition(position, _patternSize);
                    if (pattern == null) continue;
                    patterns.Enqueue(new Pattern(pattern));

                    if (yMax < y) yMax = y;
                }
                if (xMax < x) xMax = x;
            }

            PatternGridSize = new Vector2Int(xMax, yMax + 1);
            return patterns;
        }

        private List<Pattern> MakeIndices(Queue<Pattern> patterns)
        {
            var hashSet = new HashSet<Pattern>();
            int index = 0;
            while (patterns.Count > 0)
            {
                var pattern = patterns.Dequeue();
                if (hashSet.Contains(pattern)) continue;
                pattern.Id = index++;
                hashSet.Add(pattern);
            }

            return hashSet.ToList();
        }

        /// <summary>
        /// Returns a pattern of tiles at the given grid position with the specified size.
        /// The pattern will be solved from the lower left corner to the upper right corner.
        /// </summary>
        /// <param name="gridPosition">The position of the pattern on the grid.</param>
        /// <param name="patternSize">The size of the pattern.</param>
        /// <returns>The pattern of tiles at the given position, or null if the pattern is incomplete.</returns>
        private Tile[,] GetPatternAtPosition(Vector2Int gridPosition, Vector2Int patternSize)
        {
            Tile[,] pattern = new Tile[patternSize.x, patternSize.y];
            for (int px = 0; px < _patternSize.x; px++)
            {
                for (int py = 0; py < _patternSize.y; py++)
                {
                    var position = new Vector2Int(gridPosition.x + px, gridPosition.y + py);
                    if (!IsValidPosition(position)) continue;
                    pattern[px, py] = _grid[position.x, position.y];
                }
            }

            if (pattern.Count(x => x != null) < _patternSize.x * _patternSize.y) return null;
            return pattern;
        }

        /// <summary>
        /// Determines whether the given position is a valid position within the grid.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>True if the position is valid, false otherwise.</returns>
        private bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < _gridSize.x && position.y >= 0 && position.y < _gridSize.y;
        }
    }
}