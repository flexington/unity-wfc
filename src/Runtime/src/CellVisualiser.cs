using flexington.Grid;
using UnityEngine;

namespace flexington.WFC
{
    public class CellVisualiser : GridVisualiser<Cell>
    {
        public override void Visualise(Cell[,] grid, Transform parent = null, Vector2 cellSize = default, Vector2 origin = default)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    var cell = grid[x, y];
                    if(cell == null) continue;
                    if (cell.IsCollapsed)
                    {
                        for (int px = 0; px < cell.Pattern.Tiles.GetLength(0); px++)
                        {
                            for (int py = 0; py < cell.Pattern.Tiles.GetLength(1); py++)
                            {
                                var tile = cell.Pattern.Tiles[px, py];
                                if (tile == null) continue;

                                var position = (new Vector2Int(x, y) * cellSize) + new Vector2Int(px, py) + origin + new Vector2(0.5f, 0.5f);
                                var tileObject = new GameObject($"Tile ({x + px}, {y + py})");
                                tileObject.transform.SetParent(parent);
                                tileObject.transform.localPosition = position;
                                tileObject.AddComponent<SpriteRenderer>().sprite = (tile.TileBase as UnityEngine.Tilemaps.Tile).sprite;
                            }
                        }
                        CreateWorldText($"{cell.Pattern.Id}", parent, (new Vector2(x, y) * cellSize) + new Vector2(origin.x, origin.y) + Vector2.one);
                    }
                    else
                    {
                        CreateWorldText($"{cell.Options.Count}", parent, (new Vector2(x, y) * cellSize) + new Vector2(origin.x, origin.y) + Vector2.one);
                    }

                }
            }
        }
    }
}