namespace Phloem.Models
{
    /// <summary>
    /// Represents one Cell in a <see cref="Generation"/> grid.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Gets the position on the <see cref="Generation.Grid"/> X axis of the
        /// current <see cref="Cell"/> instance.
        /// This field is read-only.
        /// </summary>
        public readonly int PositionX;

        /// <summary>
        /// Gets the position on the <see cref="Generation.Grid"/> Y axis of the
        /// current <see cref="Cell"/> instance.
        /// This field is read-only.
        /// </summary>
        public readonly int PositionY;

        /// <summary>
        /// Gets if the current <see cref="Cell"/> instance is alive.
        /// </summary>
        public bool IsAlive { get; private set; }

        /// <summary>
        /// Create a new instance of the <see cref="Cell"/> with a X position and
        /// a Y position. It will be dead by default.
        /// </summary>
        /// <param name="positionX">The X position on the
        /// <see cref="Generation.Grid"/>.</param>
        /// <param name="positionY">The Y position on the
        /// <see cref="Generation.Grid"/>.</param>
        public Cell(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
            IsAlive = false;
        }

        /// <summary>
        /// Kill the <see cref="Cell"/> if alive, resurrect it otherwise.
        /// </summary>
        public void Toggle()
        {
            IsAlive = !IsAlive;
        }

        /// <summary>
        /// Bring the <see cref="Cell"/> to life.
        /// </summary>
        public void Resurrect()
        {
            IsAlive = true;
        }

        /// <summary>
        /// Kill the <see cref="Cell"/>.
        /// </summary>
        public void Kill()
        {
            IsAlive = false;
        }
    }
}
