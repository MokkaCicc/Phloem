using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        /// This field is readonly.
        /// </summary>
        public readonly List<Cell> Grid;

        public Generation(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Grid = new List<Cell>(sizeX * sizeY);
            InitializeGrid();
            InitializeCells();
        }

        public Generation(Generation generation)
        {
            SizeX = generation.SizeX;
            SizeY = generation.SizeY;
            Grid = generation.Grid.ToList();
        }

        private void InitializeGrid()
        {
            // I Init the grid with a 1 wide gutter to gain performances during
            // the generation calculation.
            for (int y = -1; y < SizeY+1; y++)
            {
                for (int x = -1; x < SizeX+1; x++)
                {
                    var cell = new Cell(x, y);
                    Grid.Add(cell);
                }
            }
        }

        private void InitializeCells(float chance=0.2f)
        {
            Random r = new();
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    // I don't check null value because x and y cannot be
                    // less than 0 or greater than the grid size.
                    var cell = GetCellFast(x, y);
                    float randomFloat = (float) r.NextDouble();
                    if (randomFloat <= chance) cell!.Resurrect();
                }
            }
        }

        public Cell? GetCell(int x, int y)
        {
            if (x < -1 || y < -1) return null;
            if (x > SizeX+1 || y > SizeY+1) return null;
            // I take the gutter in mind to calculate the cell index
            return Grid[(y + 1) * (SizeX + 2) + x + 1];
        }

        private Cell GetCellFast(int x, int y)
        {
            return Grid[(y + 1) * (SizeX + 2) + x + 1];
        }

        public int GetNumberAliveNeighbors(Cell cell)
        {
            int neighbors = 0;
            int cellX = cell.PositionX;
            int cellY = cell.PositionY;
            for (int y = cellY-1; y < cellY+2; y++)
            {
                for (int x = cellX-1; x < cellX+2; x++)
                {
                    if (GetCellFast(x, y)!.IsAlive) neighbors++;
                }
            }
            // subtract the center cell
            if (cell.IsAlive) neighbors--;
            return neighbors;
        }

        public Generation GetNextGeneration()
        {
            Generation nextGeneration = new(this);
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    ProcessCell(x, y, nextGeneration);
                }
            }
            return nextGeneration;
        }

        public Generation AsyncGetNextGeneration()
        {
            Generation nextGeneration = new(this);
            for (int y = 0; y < SizeY; y++)
            {
                var y1 = y;
                ThreadPool.QueueUserWorkItem(_ => ProcessRow(y1, nextGeneration));
            }
            while (ThreadPool.PendingWorkItemCount > 0) Thread.Sleep(1);
            return nextGeneration;
        }

        private void ProcessRow(int y, Generation nextGeneration)
        {
            for (int x = 0; x < SizeX; x++)
            {
                var cell = GetCellFast(x, y)!;
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

        private void ProcessCell(int x, int y, Generation nextGeneration)
        {
            var cell = GetCellFast(x, y)!;
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
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    message += "|";
                    var cell = GetCellFast(x, y);
                    message += cell!.IsAlive ? "X" : " ";
                }
                message += "|\n";
            }

            return message;
        }
    }
}
