using System;
using Phloem.Repositories;

namespace Phloem.Models
{
    /// <summary>
    /// This is the main model class that manage the Game of Life.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Represents the number of <see cref="Generation"/> since the first one.
        /// </summary>
        public int GenerationNumber { get; private set; }

        /// <summary>
        /// This is the first <see cref="Generation"/>, this will be used by the reset method.
        /// </summary>
        public Generation FirstGeneration { get; private set; }

        /// <summary>
        /// This is the current <see cref="Generation"/>.
        /// </summary>
        public Generation CurrentGeneration { get; private set; }

        /// <summary>
        /// Create a new <see cref="Game"/> instance.
        /// </summary>
        public Game()
        {
            GenerationNumber = 0;
            FirstGeneration = new Generation(10, 10);
            CurrentGeneration = new Generation(FirstGeneration);
        }

        /// <summary>
        /// Jump to the next <see cref="Generation"/>.
        /// </summary>
        public void FastForward()
        {
            GenerationNumber++;
            CurrentGeneration.AsyncGetNextGeneration();
        }

        /// <summary>
        /// Reset the game to the first <see cref="Generation"/>.
        /// </summary>
        public void Reset()
        {
            GenerationNumber = 0;
            CurrentGeneration = new Generation(FirstGeneration);
        }

        /// <summary>
        /// Import a config file using the <see cref="SaveRepository"/>.
        /// </summary>
        public bool Import(string fileName)
        {
            try
            {
                FirstGeneration = SaveRepository.Import(fileName);
                CurrentGeneration = new Generation(FirstGeneration);
                GenerationNumber = 0;
                return false;
            }
            catch(Exception)
            {
                return true;
            }
        }
    }
}
