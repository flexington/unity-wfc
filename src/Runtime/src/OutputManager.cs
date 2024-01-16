using UnityEngine;
using UnityEngine.Tilemaps;

namespace flexington.WFC
{
    public class OutputManager
    {
        public Tilemap CreateOutput(Cell[,] grid, Tilemap tilemap)
        {
            tilemap.ClearAllTiles();

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    var pattern = grid[x, y].Pattern;
                    var cellSize = new Vector3Int(pattern.Tiles.GetLength(0), pattern.Tiles.GetLength(1), 0);
                    for (int px = 0; px < cellSize.x; px++)
                    {
                        for (int py = 0; py < cellSize.y; py++)
                        {
                            TileBase tile = pattern.Tiles[px, py].TileBase;
                            var position = (new Vector3Int(x, y, 0) * cellSize) + new Vector3Int(px, py, 0);
                            tilemap.SetTile(position, tile);
                        }
                    }
                }
            }

            return tilemap;
        }
    }
}