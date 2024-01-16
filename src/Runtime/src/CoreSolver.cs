using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace flexington.WFC
{
    public class CoreSolver
    {
        /// <summary>
        /// Size of the output grid.
        /// </summary>
        private readonly Vector2Int _outputGridsize;

        /// <summary>
        /// Size of the pattern.
        /// </summary>
        private readonly Vector2Int _patternSize;

        /// <summary>
        /// Size of the cell grid.
        /// </summary>
        private Vector2Int _cellGridSize;

        /// <summary>
        /// Dictionary of patterns and their indices.
        /// </summary>
        private readonly List<Pattern> _patterns;

        private readonly System.Random _random;

        private Cell[] _cells;
        public Cell[] Cells => _cells;
        public Cell[,] Grid => MakeGrid(_cells, _cellGridSize);

        public CoreSolver(Vector2Int outputGridSize, List<Pattern> patterns)
        {
            _outputGridsize = outputGridSize;
            _patterns = patterns;

            _patternSize = new Vector2Int(patterns.First().Tiles.GetLength(0), patterns.First().Tiles.GetLength(1));
            _cellGridSize = new Vector2Int(_outputGridsize.x / _patternSize.x, _outputGridsize.y / _patternSize.y);

            _random = new System.Random();
            _cells = InitializeCells();
        }

        /// <summary>
        /// Solves the WFC algorithm for a given shape.
        /// </summary>
        /// <param name="shape">The shape to solve for.</param>
        /// <returns>The output grid.</returns>
        public Cell[,] Solve(List<Vector2Int> shape, TileBase @default, int maxIteration = 100)
        {
            _cells = InitializeCells(shape, @default);

            int i = 0;
            while (i < maxIteration)
            {
                try
                {
                    while (_cells.Any(cell => !cell.IsCollapsed))
                    {
                        Cell cell = GetNextCandidate(_cells);
                        cell = Collapse(cell);
                        _cells = CollapseNeighbours(_cells, cell.Position);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                i++;
            }

            return MakeGrid(_cells, _cellGridSize);
        }

        public Cell[,] Solve(int maxIteration = 100, CellObject cellObject = null)
        {
            if (cellObject != null) ProcessCellObject(cellObject);


            Cell[,] output = null;
            int i = 0;
            while (i < maxIteration)
            {
                try
                {
                    while (_cells.Any(cell => !cell.IsCollapsed))
                    {
                        Cell cell = GetNextCandidate(_cells);
                        cell = Collapse(cell);
                        _cells = CollapseNeighbours(_cells, cell.Position);

                    }

                    output = MakeGrid(_cells, _cellGridSize);

                    return output;
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
                i++;
            }
            return output;
        }

        private void ProcessCellObject(CellObject cellObject)
        {
            for (int i = 0; i < cellObject.Cells.Length; i++)
            {
                var id = cellObject.Cells[i];
                if (id == -1) continue;
                var cell = _cells[i];
                var index = cell.Options.IndexOf(cell.Options.First(option => option.Id == id));
                Collapse(_cells[i], index);

                CollapseNeighbours(_cells, cell.Position);
            }
        }

        public void ResetCell(Cell selectedCell)
        {
            selectedCell.IsCollapsed = false;
            selectedCell.Options = new List<Pattern>(_patterns);
            selectedCell.Pattern = null;
        }

        public Cell[,] Step()
        {
            Cell cell = GetNextCandidate(_cells);
            cell = Collapse(cell);
            _cells = CollapseNeighbours(_cells, cell.Position);

            var output = MakeGrid(_cells, _cellGridSize);

            return output;
        }

        private Cell[] InitializeCells()
        {
            Cell[] output = new Cell[_cellGridSize.x * _cellGridSize.y];
            for (int x = 0; x < _cellGridSize.x; x++)
            {
                for (int y = 0; y < _cellGridSize.y; y++)
                {
                    var i = PositionToIndex(new Vector2Int(x, y), _cellGridSize);
                    output[i] = new Cell
                    {
                        IsCollapsed = false,
                        Options = new List<Pattern>(_patterns),
                        Pattern = null,
                        Position = new Vector2Int(x, y)
                    };
                }
            }

            return output;
        }

        private Cell[] InitializeCells(List<Vector2Int> shape, TileBase @default)
        {
            var xMax = shape.Max(v => v.x) + 1;
            var yMax = shape.Max(v => v.y) + 1;

            var grid = new Cell[xMax * yMax];
            _cellGridSize = new Vector2Int(xMax, yMax);

            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    var i = PositionToIndex(new Vector2Int(x, y), _cellGridSize);
                    Cell cell = null;
                    if (!shape.Any(v => v == new Vector2Int(x, y)))
                    {
                        cell = new Cell
                        {
                            IsCollapsed = true,
                            Options = new List<Pattern>(),
                            Pattern = new Pattern(new Tile[,] { { new Tile(@default, 0) } }),
                            Position = new Vector2Int(x, y)
                        };
                    }
                    else
                    {
                        cell = new Cell
                        {
                            IsCollapsed = false,
                            Options = new List<Pattern>(_patterns),
                            Pattern = null,
                            Position = new Vector2Int(x, y)
                        };
                    }
                    grid[i] = cell;
                }
            }

            for (int i = 0; i < grid.Length; i++)
            {
                try
                {
                    ;
                    var position = IndexToPosition(i, _cellGridSize);
                    if (!grid[i].IsCollapsed) continue;
                    grid = CollapseNeighbours(grid, position);
                }
                catch (System.Exception)
                {
                    Debug.Log($"Error at {i}");
                    throw;
                }
            }

            return grid;
        }


        private Cell GetNextCandidate(Cell[] cells)
        {
            cells = cells.Where(cell => cell != null).ToArray();
            cells = cells.Where(cell => !cell.IsCollapsed && cell.Options.Count > 0).OrderBy(cell => cell.Options.Count).ToArray();
            var smalles = cells.First(cell => !cell.IsCollapsed);
            var same = cells.Where(cell => cell.Options.Count == smalles.Options.Count).ToList();

            var cell = same[_random.Next(same.Count)];

            return cell;
        }

        public Cell Collapse(Cell cell, Nullable<int> patternIndex = null)
        {
            if (cell.IsCollapsed) return cell;

            if (cell.Options.Count == 1)
            {
                cell.Pattern = cell.Options.First();
                cell.IsCollapsed = true;
                cell.Options = new List<Pattern>();

                return cell;
            }

            var index = patternIndex ?? _random.Next(cell.Options.Count - 1);

            cell.Pattern = cell.Options.ToList()[index];
            cell.IsCollapsed = true;
            cell.Options = new List<Pattern>();

            return cell;
        }

        /// <summary>
        /// Reduces the options of the neighbours of the cell based on the cell's pattern.
        /// </summary>
        /// <param name="cells">The cells representing the grid.</param>
        /// <param name="position">The position of the cell.</param>
        /// <returns>The updated grid or null if a neighbour has no options left.</returns>
        public Cell[] CollapseNeighbours(Cell[] cells, Vector2Int position)
        {
            var cell = cells[PositionToIndex(position, _cellGridSize)];

            cells = ReduceOptions(cells, cell, Vector2Int.up);
            cells = ReduceOptions(cells, cell, Vector2Int.right);
            cells = ReduceOptions(cells, cell, Vector2Int.down);
            cells = ReduceOptions(cells, cell, Vector2Int.left);

            return cells;
        }

        /// <summary>
        /// Reduces the options of a neighbour based on the cell's pattern.
        /// </summary>
        /// <param name="cells">The cells representing the grid.</param>
        /// <param name="cell">The cell to reduce the neighbours of.</param>
        /// <param name="direction">The direction of the neighbour.</param>
        /// <returns>The updated grid or null if the neighbour has no options left.</returns>
        private Cell[] ReduceOptions(Cell[] cells, Cell cell, Vector2Int direction)
        {
            var position = cell.Position + direction;
            position = WrapPosition(position, _cellGridSize);
            if (position.x < 0 || position.x >= _cellGridSize.x || position.y < 0 || position.y >= _cellGridSize.y) return cells;

            var neighbour = cells[PositionToIndex(position, _cellGridSize)];

            if (neighbour.IsCollapsed) return cells;


            for (int i = neighbour.Options.Count - 1; i >= 0; i--)
            {
                int cellHash = 0;
                int neighbourHash = 0;
                var option = neighbour.Options.ElementAt(i);

                switch (direction)
                {
                    case Vector2Int v when v == Vector2Int.up:
                        cellHash = cell.Pattern.Hash.Top;
                        neighbourHash = option.Hash.Bottom;
                        break;
                    case Vector2Int v when v == Vector2Int.right:
                        cellHash = cell.Pattern.Hash.Right;
                        neighbourHash = option.Hash.Left;
                        break;
                    case Vector2Int v when v == Vector2Int.down:
                        cellHash = cell.Pattern.Hash.Bottom;
                        neighbourHash = option.Hash.Top;
                        break;
                    case Vector2Int v when v == Vector2Int.left:
                        cellHash = cell.Pattern.Hash.Left;
                        neighbourHash = option.Hash.Right;
                        break;
                    default:
                        break;
                }

                if (cellHash != neighbourHash) neighbour.Options.Remove(option);
            }

            return cells;
        }

        private Cell[,] MakeGrid(Cell[] cells, Vector2Int size)
        {
            Cell[,] output = new Cell[size.x, size.y];

            for (int i = 0; i < cells.Length; i++)
            {
                int x = i % size.x;
                int y = i / size.x;

                output[x, y] = cells[i];
            }

            return output;
        }

        private int PositionToIndex(Vector2Int position, Vector2Int size)
        {
            return position.x + position.y * size.x;
        }

        private Vector2Int IndexToPosition(int index, Vector2Int size)
        {
            return new Vector2Int(index % size.x, index / size.x);
        }

        private bool IsValidPosition(Vector2Int position, Vector2Int size)
        {
            return position.x >= 0 && position.x < size.x && position.y >= 0 && position.y < size.y;
        }

        /// <summary>
        /// If a position is outside the grid, wrap it around.
        /// </summary>
        /// <param name="position">Position to wrap.</param>
        /// <param name="size">Size of the grid.</param>
        /// <returns>The wrapped position.</returns>
        private Vector2Int WrapPosition(Vector2Int position, Vector2Int size)
        {
            if (IsValidPosition(position, size)) return position;
            return new Vector2Int((position.x + size.x) % size.x, (position.y + size.y) % size.y);
        }


    }
}