using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phloem.Models
{
    /// <summary>
    /// This class represents one generation of a Game of Life.
    /// </summary>
    public class Generation
    {
        /// <summary>
        /// Gets the horizontal size of the current <see cref="Generation"/>
        /// instance.
        /// This field is readonly.
        /// </summary>
        public readonly int SizeX;

        /// <summary>
        /// Gets the vertical size of the current <see cref="Generation"/>
        /// instance.
        /// This field is readonly.
        /// </summary>
        public readonly int SizeY;

        /// <summary>
        /// Gets the grid of all <see cref="Cell"/> of the current
        /// <see cref="Generation"/> instance.
        /// </summary>
        public List<List<Cell>> Grid { get; private set; }

        /// <summary>
        /// This is a List of List of <see cref="Cell"/> used for the calculation
        /// of the next <see cref="Generation"/>.
        /// </summary>
        public List<List<Cell>> NextGrid { get; private set; }

        public Generation(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Grid = new();
            NextGrid = new();
            InitializeGrid();
            InitializeNextGrid();
        }

        /// <summary>
        /// Create a new <see cref="Generation"/> object that copy another
        /// <see cref="Generation"/>.
        /// </summary>
        /// <param name="generation">The generation to copy.</param>
        public Generation(Generation generation)
        {
            SizeX = generation.SizeX;
            SizeY = generation.SizeY;
            Grid = CopyGrid(generation.Grid);
            NextGrid = new();
            InitializeNextGrid();
        }

        /// <summary>
        /// Utility method to easily duplicate a list of list.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        private List<List<Cell>> CopyGrid(List<List<Cell>> grid)
        {
            List<List<Cell>> copyGrid = new();
            foreach (var row in grid)
            {
                copyGrid.Add(new(row));
            }
            return copyGrid;
        }

        /// <summary>
        /// Initialize the current grid with dead <see cref="Cells"/>.
        /// </summary>
        private void InitializeGrid()
        {
            // TODO: I can add a 1 wide gutter around the grid to improve the
            // performance of the GetNumberAliveNeighbors method.
            for (int y = 0; y < SizeY; y++)
            {
                Grid.Add(new());
                for (int x = 0; x < SizeX; x++)
                {
                    var cell = new Cell(x, y);
                    Grid[y].Add(cell);
                }
            }
        }

        /// <summary>
        /// Initialize the next grid with null value.
        /// </summary>
        public void InitializeNextGrid()
        {
            for (int y = 0; y < SizeY; y++)
            {
                NextGrid.Add(new());
                for (int x = 0; x < SizeX; x++)
                {
                    // I silent this null because it will be remplaced in the
                    // next generation calculation.
                    NextGrid[y].Add(null!);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Cell"/> at x and y coordinate. Checks both values
        /// and return null if fail.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        /// <returns>The Cell object, can be null.</returns>
        public Cell? GetCell(int x, int y)
        {
            if (x < 0 || y < 0) return null;
            if (x >= SizeX || y >= SizeY) return null;
            return Grid[y][x];
        }

        /// <summary>
        /// Gets the <see cref="Cell"/> at x and y coordinate. Do not checks values.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        /// <returns>The Cell object.</returns>
        private Cell GetCellFast(int x, int y)
        {
            return Grid[y][x];
        }

        /// <summary>
        /// Get the number of alive neighbors of a <see cref="Cell"/>.
        /// </summary>
        /// <param name="cell">A Cell object.</param>
        /// <returns>The number of alive neighbors.</returns>
        public int GetNumberAliveNeighbors(Cell cell)
        {
            int neighbors = 0;
            int cellXMin = cell.PositionX - 1;
            int cellXMax = cell.PositionX + 2;
            int cellYMin = cell.PositionY - 1;
            int cellYMax = cell.PositionY + 2;
            for (int y = cellYMin; y < cellYMax; y++)
            {
                for (int x = cellXMin; x < cellXMax; x++)
                {
                    var neighborCell = GetCell(x, y);
                    if (neighborCell != null && neighborCell.IsAlive) neighbors++;
                }
            }
            // subtract the center cell
            if (cell.IsAlive) neighbors--;
            return neighbors;
        }

        /// <summary>
        /// Jump to the Next <see cref="Generation"/> without threading. Not used.
        /// </summary>
        public void GetNextGeneration()
        {
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    NextGrid[y][x] = GetNextCell(x, y);
                }
            }
            Grid = CopyGrid(NextGrid);
        }

        /// <summary>
        /// Jump to the Next <see cref="Generation"/> with threading.
        /// </summary>
        public void AsyncGetNextGeneration()
        {
            List<Task> tasks = new();
            // I make one task per row, this drastically upgrade performances
            // compared to one task per cell.
            for (int y = 0; y < SizeY; y++)
            {
                // need to prevent the ref to change
                int y1 = y;
                var task = new Task(() => GetRow(y1));
                task.Start();
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            Grid = CopyGrid(NextGrid);
        }

        /// <summary>
        /// Method used by the tasks in <see cref="AsyncGetNextGeneration"/>.
        /// </summary>
        /// <param name="y">The row to process.</param>
        private void GetRow(int y)
        {
            for (int x = 0; x < SizeX; x++)
            {
                NextGrid[y][x] = GetNextCell(x, y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Cell GetNextCell(int x, int y)
        {
            var cell = GetCellFast(x, y);
            Cell nextCell = new(x, y);
            int n = GetNumberAliveNeighbors(cell);

            if (cell.IsAlive)
            {
                // I "invert" this rule because the cell is dead by default.
                if (n >= 2 && n <= 3) nextCell.Resurrect();
            }
            else
            {
                if (n == 3) nextCell.Resurrect();
            }

            return nextCell;
        }
    }
}
