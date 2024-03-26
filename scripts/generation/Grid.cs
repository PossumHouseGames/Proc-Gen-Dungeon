using System;
using System.Collections.Generic;
using ToolShed.Utilities;
using Godot;
using ToolShed.Debug;

namespace ToolShed.MazeGeneration
{
    public class Grid : Grid<Cell>
    {
        public Grid(Mask mask) : base(mask)
        {
        }

        public Grid(int rows, int columns, int levels = 1) : base(rows, columns, levels)
        {
        }

        public Grid(Grid<Cell> grid) : base(grid)
        {
        }

        public void AddSubgrid(Grid subGrid, Vector3I targetPosition)
        {
            if (subGrid == null ||
                targetPosition.X < 0 || targetPosition.X >= Columns ||
                targetPosition.Z < 0 || targetPosition.Z >= Rows ||
                targetPosition.Y < 0 || targetPosition.Y >= Levels)
            {
                return;
            }
            
            
        }
    }
    
    public class Grid<T> where T : Cell
    { 
        public int Rows { get; protected set; }
        public int Columns { get; protected set; }
        public int Levels { get; protected set; }

        protected T[] _cells;
        public T[] Cells { get =>_cells; set => _cells = value; }

        public T this[int cellIndex]
        {
            get
            {
                if (cellIndex < 0 || cellIndex >= _cells.Length)
                    return null;

                return Cells[cellIndex];
            }    
        }
        
        public T this[int column, int row]
        {
            get
            {
                if (row < 0 || row > Rows - 1 ||
                    column < 0 || column > Columns - 1)
                    return null;
                
                return Cells[GetCellIndex(column, 0, row)];
            }
        }
        
        public virtual T this[int column, int level, int row]
        {
            get
            {
                if (row < 0 || row > Rows - 1 ||
                    column < 0 || column > Columns - 1 ||
                    level < 0 || level > Levels - 1)
                    return null;

                return Cells[GetCellIndex(column, level, row)];
            }
        }

        public virtual T this[(int column, int level, int row) cell]
        {
            get
            {
                if (cell.row < 0 || cell.row > Rows - 1 ||
                    cell.column < 0 || cell.column > Columns - 1 ||
                    cell.level < 0 || cell.level > Levels - 1)
                    return null;

                return Cells[GetCellIndex(cell.column, cell.level, cell.row)];
            }
        }



        // public T this[Vector3I coord] => this[coord.X, coord.Y, coord.Z];

        protected Mask CurrentMask;

        public virtual int Size => Rows * Columns * Levels;

        public Grid(Mask mask)
        {
            CurrentMask = mask;
            Initialize(mask.Rows, mask.Columns);
        }

        public Grid(int rows, int columns, int levels = 1)
        {
            Initialize(rows, columns, levels);
        }

        public Grid(Grid<T> grid)
        {
            this.Rows = grid.Rows;
            this.Columns = grid.Columns;
            this.Levels = grid.Levels;
            this.Cells = grid.Cells;
        }

        protected virtual void Initialize(int rows, int columns, int levels = 1)
        {
            if (levels == 0)
                levels = 1;
            
            this.Rows = rows;
            this.Columns = columns;
            this.Levels = levels;

            Cells = PrepareGrid3D();
            ConfigureCells();
        }

        protected virtual T[] PrepareGrid3D()
        {
            T[] grid = new T[Rows * Columns * Levels];

            for (int level =0; level < Levels; level++) // height, Y
            {
                for (int row = 0; row < Rows; row++) // depth, Z
                {
                    for (int column = 0; column < Columns; column++) // width, X
                    {
                        int cellIndex = GetCellIndex(column, level, row);
                        if (cellIndex >= grid.Length)
                        {
                            GameDebug.LogError($"Index {cellIndex} out of bounds, ArraySize: {grid.Length}");
                            continue;
                        }
                        grid[cellIndex] = (T)Activator.CreateInstance(typeof(T), row, column, level);
                        grid[cellIndex].CellIndex = cellIndex;
                    }
                }
            }

            return grid;
        }
        
        public int GetCellIndex(Vector2I coord)
        {
            return GetCellIndex(coord.X, 0, coord.Y);
        }

        public int GetCellIndex(Vector3I coord)
        {
            return GetCellIndex(coord.X, coord.Y, coord.Z);
        }
        
        public int GetCellIndex(int column, int level, int row )
        {
            // x + (z * zMax) + (y * xMax * zMax)
            // Debug.Log($"CellCoord ({column},{row},{level});CellIndex:{column + (row * Rows) + (level  * Columns * Rows)}");
            return column + (row * Columns) + (level * Columns * Rows);
        }

        public Vector3I GetCoordFromIndex(int cellIndex)
        {
            Vector3I coord = Vector3I.Zero;
            coord.Y = cellIndex / (Rows * Columns);
            cellIndex -= coord.Y * Columns * Rows;
            coord.Z = cellIndex / Rows;
            coord.X = cellIndex % Columns;
            return coord;
        }

        /// <summary>
        /// Updates each cell with info about its neighbors
        /// </summary>
        private void ConfigureCells()
        {
            foreach (var cell in Cells)
            {
                if (cell == null) continue;
                
                int row = cell.Row;
                int column = cell.Column;
                int level = cell.Level;
                
                cell.North = this[column, level, row + 1];
                cell.South = this[column, level, row - 1];
                cell.East = this[column + 1, level, row];
                cell.West = this[column - 1, level, row];
                cell.Up = this[column, level + 1, row];
                cell.Down = this[column, level - 1, row];
            }
        }

        public virtual void AddLevel(bool connectLevels = true)
        {
            Levels++;
            System.Array.Resize(ref _cells, Columns*Levels*Rows );
            int level = Levels - 1;

            for (int row = 0; row < Rows; ++row)
            {
                for (int column = 0; column < Columns; ++column)
                {
                    int cellIndex = GetCellIndex(column, level, row);
                    Cells[cellIndex] = (T)Activator.CreateInstance(typeof(T), row, column, level);;
                }
            }

            for (int row = 0; row < Rows; ++row)
            {
                for (int column = 0; column < Columns; ++column)
                {
                    int cellIndex = GetCellIndex(column, level, row);
                    ref var cell = ref _cells[cellIndex];
                    cell.North = this[column, level, row + 1];
                    cell.South = this[column, level, row - 1];
                    cell.East = this[column + 1, level, row];
                    cell.West = this[column - 1, level, row];

                    if (connectLevels)
                    {
                        cell.Up = this[column, level + 1, row];
                        cell.Down = this[column, level - 1, row];
                    }
                }
            }
        }
        
        public void AddCell(int column, int row) => AddCell(column, 0, row);
        public void AddCell((int column, int level, int row) cell)
        {
           AddCell(cell.column, cell.level, cell.row);
        }
        public virtual void AddCell(int column, int level, int row)
        {
            int cellIndex = GetCellIndex(column, level, row);
            Cells[cellIndex] = (T)Activator.CreateInstance(typeof(T), row, column, level);
            ref var cell = ref Cells[cellIndex];

            this[column, level, row + 1]?.AddNeighbor(cell, Cell.NeighborDirection.South);
            this[column, level, row - 1]?.AddNeighbor(cell, Cell.NeighborDirection.North);
            this[column + 1, level, row]?.AddNeighbor(cell, Cell.NeighborDirection.West);
            this[column - 1,level, row]?.AddNeighbor(cell, Cell.NeighborDirection.East);
            this[column, level + 1, row]?.AddNeighbor(cell, Cell.NeighborDirection.Down);
            this[column, level - 1, row]?.AddNeighbor(cell, Cell.NeighborDirection.Up);
        }

        public void AddCell(int cellIndex)
        {
            
        }
        
        public void RemoveCell(int column, int row) => RemoveCell(column, 0, row);
        public void RemoveCell(int column, int level, int row)
        {
            int cellIndex = GetCellIndex(column, level, row);
            if (cellIndex >= Cells.Length || cellIndex < 0) return;
            
            this[column, level, row + 1]?.RemoveNeighbor(Cell.NeighborDirection.South);
            this[column, level, row - 1]?.RemoveNeighbor(Cell.NeighborDirection.North);
            this[column + 1, level, row]?.RemoveNeighbor(Cell.NeighborDirection.West);
            this[column - 1, level, row]?.RemoveNeighbor(Cell.NeighborDirection.East);
            this[column, level + 1, row]?.RemoveNeighbor(Cell.NeighborDirection.Down);
            this[column, level - 1, row]?.RemoveNeighbor(Cell.NeighborDirection.Up);
            
            Cells[cellIndex] = null;

        }

        public void RemoveCell((int column, int level, int row) cell)
        {
            RemoveCell(cell.column, cell.level, cell.row);
        }

        public void RemoveCell(int cellIndex)
        {
            if (cellIndex >= Cells.Length || cellIndex < 0) return;
            
        }

        public void RemoveAllCells()
        {
            for (int cellIndex = 0; cellIndex < _cells.Length; cellIndex++)
            {
                _cells[cellIndex] = null;
            }
        }

        public void RotateGrid(int rotation)
        {
            Grid rotatedGrid = new Grid(Rows, Columns, Levels);
            rotatedGrid.RemoveAllCells();

            for (int cellIndex = 0; cellIndex < _cells.Length; cellIndex++)
            {
                if (_cells[cellIndex] != null)
                {
                    var coord = GetCoordFromIndex(cellIndex);
                    var rotatedCoord = MapFromLocalCoordinateWithRotationAndOffset(coord, rotation);
                    rotatedGrid.AddCell(rotatedCoord.X, rotatedCoord.Y, rotatedCoord.Z);
                }
            }
            
            RemoveAllCells();
            for (int cellIndex = 0; cellIndex < rotatedGrid._cells.Length; cellIndex++)
            {
                if (rotatedGrid.Cells[cellIndex] == null)
                    continue;

                var coord = GetCoordFromIndex(cellIndex);
                AddCell(coord.X, coord.Y, coord.Z);
            }
        }
        
        public Vector3I MapFromLocalCoordinateWithRotationAndOffset(Vector3I coord, int yawRot)
        {
            int tmp;
            switch (yawRot)
            {
                case 0:
                    break;
                case 180:
                    coord.X = Columns - coord.X - 1;
                    coord.Z = Rows - coord.Z - 1;
                    break;
                case 90:
                    tmp = Columns - coord.X - 1;
                    coord.X = coord.Z;
                    coord.Z = tmp;
                    break;
                case 270:
                    tmp = coord.X;
                    coord.X = Columns - coord.Z - 1;
                    coord.Z = tmp;
                    break;
            }
            return coord;
        }

        public virtual T GetRandomCell(System.Random rng)
        {
            int row = rng.Next(0, Rows);
            int column = rng.Next(0, Columns);
            int level = rng.Next(0, Levels);
            
            return this[column, level, row];
        }

        public IEnumerable<T[]> EachRow()
        {
            T[] gridRow = new T[Columns];

            for (int level = 0; level < Levels; level++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    for (int column = 0; column < Columns; column++)
                        gridRow[column] = this[column, level, row];

                    yield return gridRow;
                }
            }
        }

        public IEnumerable<T> EachCell()
        {
            for (int level = 0; level < Levels; level++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    for (int column = 0; column < Columns; column++)
                    {
                        yield return this[column, level, row];
                    }
                }
            }
        }

        public IEnumerable<T> EachCellInColumn(int column)
        {
            if (column < 0 || column >= Columns)
                yield break;
            
            for (int level = 0; level < Levels; level++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    yield return this[column, level, row];
                }
            }            
        }
        
        public IEnumerable<T> EachCellInRow(int row)
        {
            if (row < 0 || row >= Rows)
                yield break;
            
            for (int level = 0; level < Levels; level++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    yield return this[column, level, row];
                }
            }            
        }
        
        public virtual string ContentsOf(T cell)
        {
            string contents = " ";

            return contents;
        }

        public virtual Color BackgroundColorFor(T cell)
        {
            return Colors.Gray;
        }

        public override string ToString()
        {
            string output = "+" + "---+".Multiply(Columns) + "\n";

            foreach (var row in EachRow())
            {
                string top = "|";
                string bottom = "+";
                //Debug.Log("CellArray row: " + row.Length);

                foreach (var cell in row)
                {
                    //Debug.Log("cell" + cell.ToString());

                    var c = cell ?? new Cell(-1, -1);
                    //Debug.Log(c.ToString());
                    string body = $" {ContentsOf(cell)} "; // 3 spaces
                    string eastBoundary = c.IsLinked(c.East) ? " " : "|";
                    top += body + eastBoundary;

                    string southBoundary = c.IsLinked(c.South) ? "   " : "---";
                    string corner = "+";
                    bottom += southBoundary + corner;

                }

                output += top + "\n" + bottom + "\n";
            }

            return output;
        }

        public Image[] ToTextures()
        {
            Image[] allTextures = new Image[Levels];
            for (int level = 0; level < Levels; ++level)
            {
                allTextures[level] = ToTexture(level);
            }

            return allTextures;
        }

        public Image ToSingleTexture(int cellSize = 10)
        {
            var width = Columns * cellSize * Levels + (cellSize * (Levels - 1));
            var height = Rows * cellSize;
            // var texture = new Texture2D(width, height, TextureFormat.ARGB32, false)
            // {
            //     filterMode = FilterMode.Point
            // };

            var texture = new Image();

            var wallColor = Colors.Black;
            var offset = 0;
            for (int level = 0; level < Levels; level++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    for (int column = 0; column < Columns; column++)
                    {
                        var x1 = column * cellSize + offset;
                        var y1 = row * cellSize;
                        var x2 = (column + 1) * cellSize + offset;
                        var y2 = (row + 1) * cellSize;

                        var cellBackgroundColor = Colors.Gray;

                        var cell = this[column, level, row];
                        if (cell != null)
                            cellBackgroundColor = BackgroundColorFor(cell);

                        // Draw background
                        for (int cx = x1; cx < x2; cx++)
                        for (int cy = y1; cy < y2; cy++)
                            if (texture.GetPixel(cx, cy) != wallColor)
                                texture.SetPixel(cx, cy, cellBackgroundColor);

                        if (cell == null) continue;

                        if (cell.North == null)
                        {
                            for (int x = x1; x < x2; x++)
                                texture.SetPixel(x, y2 == height ? y2 - 1 : y2, wallColor);
                            // Debug.Log($"Cell {cell.Row},{cell.Column} Drawing North Wall");
                        }

                        if (cell.West == null)
                        {
                            for (int y = y1; y <= y2; y++)
                                texture.SetPixel(x1, y, wallColor);
                            // Debug.Log($"Cell {cell.Row},{cell.Column} Drawing West Wall");
                        }

                        if (!cell.IsLinked(cell.East))
                        {
                            for (int y = y1; y <= y2; y++)
                                texture.SetPixel(x2 == width ? x2 - 1 : x2, y, wallColor);
                            // Debug.Log($"Cell {cell.Row},{cell.Column} Drawing East Wall");

                        }

                        if (!cell.IsLinked(cell.South))
                        {
                            for (int x = x1; x < x2; x++)
                                texture.SetPixel(x, y1, wallColor);
                            // Debug.Log($"Cell {cell.Row},{cell.Column} Drawing South Wall");
                        }

                        if (cell.IsLinked(cell.Up))
                        {
                            // texture.SetPixel(x1 + 2, y1 + 6, wallColor);
                            // texture.SetPixel(x1 + 3, y1 + 7, wallColor);
                            // texture.SetPixel(x1 + 4, y1 + 8, wallColor);
                            // texture.SetPixel(x1 + 5, y1 + 8, wallColor);
                            // texture.SetPixel(x1 + 6, y1 + 7, wallColor);
                            // texture.SetPixel(x1 + 7, y1 + 6, wallColor);

                            texture.SetPixel(x1 + 6, y1 + 3, wallColor);
                            texture.SetPixel(x1 + 7, y1 + 4, wallColor);
                            texture.SetPixel(x1 + 8, y1 + 5, wallColor);
                            texture.SetPixel(x1 + 8, y1 + 6, wallColor);
                            texture.SetPixel(x1 + 7, y1 + 7, wallColor);
                            texture.SetPixel(x1 + 6, y1 + 8, wallColor);
                        }

                        if (cell.IsLinked(cell.Down))
                        {
                            // texture.SetPixel(x1 + 2, y1 + 5, wallColor);
                            // texture.SetPixel(x1 + 3, y1 + 4, wallColor);
                            // texture.SetPixel(x1 + 4, y1 + 3, wallColor);
                            // texture.SetPixel(x1 + 5, y1 + 3, wallColor);
                            // texture.SetPixel(x1 + 6, y1 + 4, wallColor);
                            // texture.SetPixel(x1 + 7, y1 + 5, wallColor);

                            texture.SetPixel(x1 + 5, y1 + 3, wallColor);
                            texture.SetPixel(x1 + 4, y1 + 4, wallColor);
                            texture.SetPixel(x1 + 3, y1 + 5, wallColor);
                            texture.SetPixel(x1 + 3, y1 + 6, wallColor);
                            texture.SetPixel(x1 + 4, y1 + 7, wallColor);
                            texture.SetPixel(x1 + 5, y1 + 8, wallColor);
                        }
                    }
                }

                offset += cellSize * Columns;
                offset += cellSize;
            }
            
            // texture.Apply();
            return texture;
        }
        
        public Image ToTexture(int level = 0)
        {
            var cellSize = 10;
            var width = Columns * cellSize;
            var height = Rows * cellSize;
            // var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            // texture.filterMode = FilterMode.Point;
            var texture = new Image();
            var wallColor = Colors.Black;
            // for (int x = 0; x < Columns * cellSize; x++)
            // {
            //     for (int y = 0; y < Rows * cellSize; y++)
            //     {
            //         texture.SetPixel(x, y, Color.white);
            //     }
            // }

            int count = 0;
            // foreach (var cell in EachCell())
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    var x1 = column * cellSize;
                    var y1 = row * cellSize;
                    var x2 = (column + 1) * cellSize;
                    var y2 = (row + 1) * cellSize;

                    var cellBackgroundColor = Colors.Gray;
                    
                    var cell = this[column, level, row];
                    if (cell != null)
                        cellBackgroundColor = BackgroundColorFor(cell);
                    
                    // Draw background
                    for (int cx = x1; cx < x2; cx++)
                        for (int cy = y1; cy < y2; cy++)
                            if (texture.GetPixel(cx,cy) != wallColor)
                                texture.SetPixel(cx, cy, cellBackgroundColor);

                    if (cell == null) continue;
                    
                    if (cell.North == null)
                    {
                        for (int x = x1; x < x2; x++)
                            texture.SetPixel(x, y2 ==height ? y2-1 : y2 , wallColor);
                        // Debug.Log($"Cell {cell.Row},{cell.Column} Drawing North Wall");
                    }

                    if (cell.West == null)
                    {
                        for (int y = y1; y <= y2; y++)
                            texture.SetPixel(x1, y, wallColor);
                        // Debug.Log($"Cell {cell.Row},{cell.Column} Drawing West Wall");
                    }

                    if (!cell.IsLinked(cell.East))
                    {
                        for (int y = y1; y <= y2; y++)
                            texture.SetPixel(x2==width ? x2-1: x2, y, wallColor);
                        // Debug.Log($"Cell {cell.Row},{cell.Column} Drawing East Wall");
                    
                    }
                    
                    if (!cell.IsLinked(cell.South))
                    {
                        for (int x = x1; x < x2; x++)
                            texture.SetPixel(x, y1, wallColor);
                        // Debug.Log($"Cell {cell.Row},{cell.Column} Drawing South Wall");
                    }

                    if (cell.IsLinked(cell.Up))
                    {
                        // texture.SetPixel(x1 + 2, y1 + 6, wallColor);
                        // texture.SetPixel(x1 + 3, y1 + 7, wallColor);
                        // texture.SetPixel(x1 + 4, y1 + 8, wallColor);
                        // texture.SetPixel(x1 + 5, y1 + 8, wallColor);
                        // texture.SetPixel(x1 + 6, y1 + 7, wallColor);
                        // texture.SetPixel(x1 + 7, y1 + 6, wallColor);
                        
                        texture.SetPixel(x1 + 6, y1 + 3, wallColor);
                        texture.SetPixel(x1 + 7, y1 + 4, wallColor);
                        texture.SetPixel(x1 + 8, y1 + 5, wallColor);
                        texture.SetPixel(x1 + 8, y1 + 6, wallColor);
                        texture.SetPixel(x1 + 7, y1 + 7, wallColor);
                        texture.SetPixel(x1 + 6, y1 + 8, wallColor);
                    }
                    
                    if (cell.IsLinked(cell.Down))
                    {
                        // texture.SetPixel(x1 + 2, y1 + 5, wallColor);
                        // texture.SetPixel(x1 + 3, y1 + 4, wallColor);
                        // texture.SetPixel(x1 + 4, y1 + 3, wallColor);
                        // texture.SetPixel(x1 + 5, y1 + 3, wallColor);
                        // texture.SetPixel(x1 + 6, y1 + 4, wallColor);
                        // texture.SetPixel(x1 + 7, y1 + 5, wallColor);
                        
                        texture.SetPixel(x1 + 5, y1 + 3, wallColor);
                        texture.SetPixel(x1 + 4, y1 + 4, wallColor);
                        texture.SetPixel(x1 + 3, y1 + 5, wallColor);
                        texture.SetPixel(x1 + 3, y1 + 6, wallColor);
                        texture.SetPixel(x1 + 4, y1 + 7, wallColor);
                        texture.SetPixel(x1 + 5, y1 + 8, wallColor);
                    }
                }
            }
            // texture.Apply();
            return texture;
        }
        

        public List<T> DeadEnds()
        {
            List<T> list = new List<T>();

            foreach (var cell in EachCell())
            {
                if (cell.Links.Length == 1)
                    list.Add(cell);
            }

            return list;
        }
    }
}