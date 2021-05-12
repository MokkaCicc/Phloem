using Phloem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Phloem.Repositories
{
    /// <summary>
    /// This class is used to load a <see cref="Game"/> from a text file.
    /// </summary>
    internal static class SaveRepository
    {
        /// <summary>
        /// Represents the minimum numbers of X cases.
        /// </summary>
        const int MIN_X = 3;

        /// <summary>
        /// Represents the maximum numbers of X cases.
        /// </summary>
        const int MAX_X = 50;

        /// <summary>
        /// Represents the minimum numbers of X cases.
        /// </summary>
        const int MIN_Y = 3;

        /// <summary>
        /// Represents the maximum numbers of Y cases.
        /// </summary>
        const int MAX_Y = 50;

        /// <summary>
        /// Create a <see cref="Generation"/> instance from a config file.
        /// </summary>
        /// <param name="fileName">The full path of the file.</param>
        /// <exception cref="Exception">Thrown when the config file is invalid.</exception>
        /// <returns>A Generation object.</returns>
        public static Generation Import(string fileName)
        {
            // TODO[baptiste]: makes custom Exception.
            List<string> lines = new(File.ReadLines(fileName));

            // the file need to have at least 2 lines
            if (lines.Count < 2) throw new Exception("The config file is incorrect");

            StripSpaces(lines);

            // checks sizes
            string[] sizes = lines[0].Split(' ');
            lines.RemoveAt(0);
            int sizeY = Int32.Parse(sizes[0]);
            int sizeX = Int32.Parse(sizes[1]);
            if (sizeX < MIN_X || sizeX >= MAX_X || sizeY < MIN_Y || sizeY >= MAX_Y)
                throw new Exception("The config file is incorrect");

            // check grid
            Generation generation = new(sizeX, sizeY);
            if (lines.Count != sizeY)
                throw new Exception("The config file is incorrect");
            for (int y = 0; y < lines.Count; y++)
            {
                var cells = lines[y];
                if (cells.Length != sizeX)
                    throw new Exception("The config file is incorrect");

                for (int x = 0; x < cells.Length; x++)
                {
                    switch(cells[x])
                    {
                        case '0':
                            // The cell is already dead.
                            break;
                        case '1':
                            generation.GetCell(x, y)!.Resurrect();
                            break;
                        default:
                            // The cell is already dead.
                            break;
                    }
                }
            }
            return generation;
        }

        /// <summary>
        /// Trim all ending and starting whitespaces and remove duplicate whitespaces.
        /// </summary>
        /// <param name="lines">The list of all lines.</param>
        private static void StripSpaces(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].TrimStart();
                lines[i] = lines[i].TrimEnd();
                lines[i] = Regex.Replace(lines[i], @"\s+", " ");
            }
        }
    }
}
