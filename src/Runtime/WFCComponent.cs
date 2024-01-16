using System.Collections.Generic;
using flexington.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace flexington.WFC
{
    /// <summary>
    /// Component that generates output using the Wave Function Collapse (WFC) algorithm.
    /// </summary>
    public class WFCComponent : MonoBehaviour
    {
        /// <summary>
        /// The Tilemap component used for rendering the generated output.
        /// </summary>
        [Header("Input")]
        [SerializeField()] private Tilemap _inputTilemap;

        /// <summary>
        /// The Tilemap where the output of the WFC algorithm will be generated.
        /// </summary>
        [Header("Output")]
        [SerializeField] private Tilemap _outputTilemap;

        /// <summary>
        /// The size of the output grid.
        /// </summary>
        [SerializeField] private Vector2Int _outputGridSize;

        /// <summary>
        /// The maximum number of iterations to solve the WFC algorithm.
        /// </summary>
        [SerializeField] private int _maxIterations;


        [Header("Pattern")]
        [SerializeField] private Vector2Int _patternSize;
        [SerializeField] private bool _isOverlapping;

        private Tile[,] _inputGrid;
        private string _inputParent = "Input Grid";

        private List<Pattern> _patterns;
        private string _patternParent = "Pattern Grid";

        private Cell[,] _outputGrid;
        private string _outputParent = "Output Grid";

        public void ReadInput()
        {
            var inputManager = new InputManager(_inputTilemap);
            _inputGrid = inputManager.ReadTilemap();
        }

        public void ShowInputGrid()
        {
            var size = new Vector2Int(_inputGrid.GetLength(0), _inputGrid.GetLength(1));
            var grid = new Grid<Tile>(size, Vector2.one, Vector2.zero);
            grid.SetValues(_inputGrid);

            var parent = new GameObject(_inputParent).transform;
            parent.position = Vector3.zero;
            parent.SetParent(transform);

            grid.Visualise(new DefaultGridVisualiser<Tile>(), parent);
        }

        public void HideInputGrid()
        {

            var inputParent = transform.Find(_inputParent);
            if (inputParent == null) return;

            if (Application.isEditor) DestroyImmediate(inputParent.gameObject);
            else Destroy(inputParent.gameObject);
        }

        public void CreatePattern()
        {
            var patternManager = new PatternManager(_inputGrid, _patternSize, _isOverlapping);
            _patterns = patternManager.ProcessGrid();
        }

        public void ShowPatternGrid()
        {

            var width = (_inputGrid.GetLength(0) - (_patternSize.x - 1)) / (_isOverlapping ? 1 : _patternSize.x);
            var height = (_inputGrid.GetLength(1) - (_patternSize.y - 1)) / (_isOverlapping ? 1 : _patternSize.y);
            var size = new Vector2Int(width, height);

            Pattern[,] patternGrid = new Pattern[width, height];


            var i = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    patternGrid[x, y] = _patterns[i++];
                }
            }


            var grid = new Grid<Pattern>(size, new Vector2(2, 2), Vector2.zero);
            grid.SetValues(patternGrid);

            var parent = new GameObject(_patternParent).transform;
            parent.position = Vector3.zero;
            parent.SetParent(transform);

            grid.Visualise(new PatternGridVisualiser(), parent);
        }

        public void HidePatternGrid()
        {
            var patternParent = transform.Find(_patternParent);
            if (patternParent == null) return;

            if (Application.isEditor) DestroyImmediate(patternParent.gameObject);
            else Destroy(patternParent.gameObject);
        }

        public void OutputSolve()
        {
            _solver = new CoreSolver(_outputGridSize, _patterns);
            _outputGrid = _solver.Solve(_maxIterations);

            HideOutputGrid();
            ShowOutputGrid();
        }

        public void MakeTilemap()
        {
            _outputTilemap.ClearAllTiles();
            for (int y = 0; y < _outputGrid.GetLength(1); y++)
            {
                for (int x = 0; x < _outputGrid.GetLength(0); x++)
                {
                    var cell = _outputGrid[x, y];
                    for (int py = 0; py < cell.Pattern.Tiles.GetLength(1); py++)
                    {
                        for (int px = 0; px < cell.Pattern.Tiles.GetLength(0); px++)
                        {
                            var tile = cell.Pattern.Tiles[px, py];
                            var position = new Vector3Int(x * _patternSize.x + px, y * _patternSize.y + py, 0);
                            _outputTilemap.SetTile(position, tile.TileBase);
                        }
                    }
                }
            }
            HideOutputGrid();
        }

        CoreSolver _solver;
        public void OutputStep()
        {
            if (_solver == null) _solver = new CoreSolver(_outputGridSize, _patterns);
            _outputGrid = _solver.Step();

            HideOutputGrid();
            ShowOutputGrid();
        }

        public void ResetSteps()
        {
            _solver = null;
            _outputTilemap.ClearAllTiles();
        }

        public void ShowOutputGrid()
        {
            var size = new Vector2Int(_outputGridSize.x / _patternSize.x, _outputGridSize.y / _patternSize.y);
            var grid = new Grid<Cell>(size, _patternSize, Vector2.zero);
            grid.SetValues(_outputGrid);

            var parent = new GameObject(_outputParent).transform;
            parent.position = Vector3.zero;
            parent.SetParent(transform);

            grid.Visualise(new CellVisualiser(), parent);

        }

        public void HideOutputGrid()
        {
            var outputParent = transform.Find(_outputParent);
            if (outputParent == null) return;

            if (Application.isEditor) DestroyImmediate(outputParent.gameObject);
            else Destroy(outputParent.gameObject);
        }
    }
}