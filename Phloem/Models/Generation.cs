using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phloem.Models
{
    // TODO[baptiste]: Docs
    public class Generation
    {
        /// <summary>
        /// Gets the horizontal size of the current <see cref="Generation"/>
        /// instance.
        /// This field is readonly.
        /// </summary>
        public readonly ushort SizeX;

        /// <summary>
        /// Gets the vertical size of the current <see cref="Generation"/>
        /// instance.
        /// This field is readonly.
        /// </summary>
        public readonly ushort SizeY;

        private readonly ushort RealSizeX;
        private readonly ushort RealSizeY;

        /// <summary>
        /// Gets the grid of all <see cref="Cell"/> of the current
        /// <see cref="Generation"/> instance.
        /// This field is readonly.
        /// </summary>
        public readonly Cell[] Grid;

        public Generation(ushort sizeX, ushort sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            RealSizeX = (ushort) (SizeX + 2);
            RealSizeY = (ushort) (SizeY + 2);
            Grid = new Cell[RealSizeX * RealSizeY];
            InitializeGrid();
            InitializeCells();
        }

        public Generation(Generation generation)
        {
            SizeX = generation.SizeX;
            SizeY = generation.SizeY;
            Grid = generation.Grid.ToArray();
        }

        private void InitializeGrid()
        {
            // I Init the grid with a 1 wide gutter to gain performances during
            // the generation calculation.
            for (ushort y = 0; y < SizeY+2; y++)
            {
                for (ushort x = 0; x < SizeX+2; x++)
                {
                    var cell = new Cell(x, y);
                    Grid[y * RealSizeX + x] = cell;
                }
            }
        }

        private void InitializeCells(float chance=0.2f)
        {
            Random r = new();
            for (ushort y = 1; y < SizeY; y++)
            {
                for (ushort x = 1; x < SizeX; x++)
                {
                    // I don't check null value because x and y cannot be
                    // less than 0 or greater than the grid size.
                    var cell = GetCellFast(x, y);
                    float randomFloat = (float) r.NextDouble();
                    if (randomFloat <= chance) cell!.Resurrect();
                }
            }
        }

        public Cell? GetCell(ushort x, ushort y)
        {
            if (x < 1 || y < 1) return null;
            if (x >= SizeX || y >= SizeY) return null;
            // I take the gutter in mind to calculate the cell index
            return Grid[y * RealSizeX + x];
        }

        private Cell GetCellFast(ushort x, ushort y)
        {
            return Grid[y * RealSizeX + x];
        }

        public int GetNumberAliveNeighbors(Cell cell)
        {
            int neighbors = 0;
            // this is weirdly more fast than using int
            ushort cellXMin = (ushort) (cell.PositionX - 1);
            ushort cellXMax = (ushort) (cell.PositionX + 2);
            ushort cellYMin = (ushort) (cell.PositionY - 1);
            ushort cellYMax = (ushort) (cell.PositionY + 2);
            for (ushort y = cellYMin; y < cellYMax; y++)
            {
                for (ushort x = cellXMin; x < cellXMax; x++)
                {
                    if (GetCellFast(x, y).IsAlive) neighbors++;
                }
            }
            // subtract the center cell
            if (cell.IsAlive) neighbors--;
            return neighbors;
        }

        public Generation GetNextGeneration()
        {
            Generation nextGeneration = new(this);
            for (ushort y = 1; y < SizeY; y++)
            {
                for (ushort x = 1; x < SizeX; x++)
                {
                    ProcessCell(x, y, nextGeneration);
                }
            }
            return nextGeneration;
        }

        public Generation AsyncGetNextGeneration()
        {
            List<Task> tasks = new();
            Generation nextGeneration = new(this);
            for (ushort y = 1; y < SizeY; y++)
            {
                var y1 = y;
                var task = new Task(() => ProcessRow(y1, nextGeneration));
                task.Start();
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            return nextGeneration;
        }

        private void ProcessRow(ushort y, Generation nextGeneration)
        {
            for (ushort x = 1; x < SizeX; x++)
            {
                var cell = GetCellFast(x, y);
                var nextCell = nextGeneration.GetCellFast(x, y);

                int n = GetNumberAliveNeighbors(cell);
                if (cell.IsAlive)
                {
                    if (n < 2 || n > 3) nextCell!.Kill();
                }

                else
                {
                    if (n == 3) nextCell!.Resurrect();
                }
            }
        }

        private void ProcessCell(ushort x, ushort y, Generation nextGeneration)
        {
            var cell = GetCellFast(x, y);
            var nextCell = nextGeneration.GetCellFast(x, y);

            int n = GetNumberAliveNeighbors(cell);
            if (cell.IsAlive)
            {
                if (n < 2 || n > 3) nextCell!.Kill();
            }

            else
            {
                if (n == 3) nextCell!.Resurrect();
            }
        }

        public override string ToString()
        {
            string message = "";
            for (ushort y = 0; y < SizeY; y++)
            {
                for (ushort x = 0; x < SizeX; x++)
                {
                    message += "|";
                    var cell = GetCellFast(x, y);
                    message += cell.IsAlive ? "X" : " ";
                }
                message += "|\n";
            }

            return message;
        }
    }
}
