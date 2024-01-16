using flexington.Grid;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace flexington.WFC
{
    [ExecuteInEditMode]
    public partial class WaveFunctionCollapseEditorComponent : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] public Tilemap _inputTilemap;

        [Header("Output")]
        [SerializeField] public Vector2Int _outputGridSize;

        [Header("Pattern")]
        [SerializeField] public Vector2Int _patternSize;
        [SerializeField] public bool _isOverlapping;

        [Header("Editor")]
        [SerializeField] public bool _editorIsActive = true;
        public bool EditorIsActive => _editorIsActive;

        private Grid<Cell> _grid;
        private CoreSolver _solver;
        private string _gridParent = "Grid";

        public void Initialize()
        {
            var inputManager = new InputManager(_inputTilemap);
            var inputGrid = inputManager.ReadTilemap();

            var patternManager = new PatternManager(inputGrid, _patternSize, _isOverlapping);
            var patterns = patternManager.ProcessGrid();

            _solver = new CoreSolver(_outputGridSize, patterns);

            var size = new Vector2Int(_outputGridSize.x / _patternSize.x, _outputGridSize.y / _patternSize.y);
            _grid = new Grid<Cell>(size, new Vector2Int(2, 2), Vector2.zero);
            _grid.SetValues(_solver.Grid);
            ShowGrid();
        }

        public void Reset()
        {
            var parent = transform.Find(_gridParent);
            if (Application.isEditor) DestroyImmediate(parent.gameObject);
            else Destroy(parent.gameObject);
        }

        private void ShowGrid()
        {
            var gridParent = new GameObject(_gridParent).transform;
            gridParent.position = Vector3.zero;
            gridParent.SetParent(transform);

            _grid.Visualise(new CellVisualiser(), gridParent);
        }

        private void HideGrid()
        {
            var gridParent = transform.Find(_gridParent);
            if (gridParent == null) return;

            if (Application.isEditor) DestroyImmediate(gridParent.gameObject);
            else Destroy(gridParent.gameObject);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Saves the current grid to a Unity asset.
        /// </summary>
        public void Save()
        {
            var cells = _solver.Cells;

            var cellObject = ScriptableObject.CreateInstance<CellObject>();
            cellObject.SetCells(cells);

            var path = EditorUtility.SaveFilePanelInProject("Save Grid", "Grid", "asset", "Save Grid");
            if (path.Length == 0) return;

            AssetDatabase.CreateAsset(cellObject, path);
        }
#endif

        public Cell GetCell(Vector2 mouseWorldPosition) => _grid.GetValue(mouseWorldPosition);

        public void CollapseCell(Cell cell, int patternIndex)
        {
            _solver.Collapse(cell, patternIndex);
            _solver.CollapseNeighbours(_solver.Cells, cell.Position);
            _grid.SetValues(_solver.Grid);

            HideGrid();
            ShowGrid();
        }

        public void ResetCell(Cell selectedCell)
        {
            _solver.ResetCell(selectedCell);
            _grid.SetValues(_solver.Grid);

            HideGrid();
            ShowGrid();
        }
    }
}