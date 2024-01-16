using System;
using System.Collections.Generic;
using UnityEngine;

namespace flexington.WFC
{
    public class CellObject : ScriptableObject
    {

        [SerializeField] int[] _cells;
        public int[] Cells => _cells;

        public void SetCells(Cell[] cells)
        {
            _cells = new int[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                var cell = cells[i];
                if (cell.IsCollapsed) Cells[i] = cell.Pattern.Id;
                else Cells[i] = -1;
            }
        }
    }
}