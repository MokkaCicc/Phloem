using Phloem.Models;
using System.Collections.Generic;
using System.Windows.Input;

namespace Phloem.ViewModels
{
    /// <summary>
    /// This is the viewmodel of the <see cref="Models.Game"/> model.
    /// </summary>
    class GameViewModel : BaseViewModel
    {
        /// <summary>
        /// Corresponding model.
        /// </summary>
        private readonly Game Game;

        /// <summary>
        /// Represents the number of <see cref="Generation"/> since the first one.
        /// </summary>
        private int GenerationNumberValue;
        public int GenerationNumber
        {
            get => GenerationNumberValue;
            set
            {
                if (GenerationNumberValue != value)
                {
                    GenerationNumberValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Represents the interval between two <see cref="Generation"/>.
        /// </summary>
        // TODO[baptiste]: Belongs to the view or viewmodel? Both seems acceptable.
        private int IntervalNumberValue;
        public int IntervalNumber
        {
            get => IntervalNumberValue;
            set
            {
                if (IntervalNumberValue != value)
                {
                    IntervalNumberValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the grid of all <see cref="Cell"/> of the current
        /// <see cref="Generation"/>.
        /// </summary>
        private List<List<Cell>>? GridValue;
        public List<List<Cell>> Grid
        {
            get => GridValue!;
            set
            {
                if (GridValue != value)
                {
                    GridValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Command for the reset button.
        /// </summary>
        public ICommand ResetCommand { get; }

        /// <summary>
        /// Command for the next button.
        /// </summary>
        public ICommand NextCommand { get; }

        /// <summary>
        /// Command for the next10 button.
        /// </summary>
        public ICommand Next10Command { get; }

        /// <summary>
        /// Create a new <see cref="GameViewModel"/> instances.
        /// </summary>
        public GameViewModel()
        {
            ResetCommand = new RelayCommand(Reset, CanReset);
            NextCommand = new RelayCommand(Next);
            Next10Command = new RelayCommand(Next10);
            Game = new();
            Update();
        }

        /// <summary>
        /// Import a config file for the Game.
        /// </summary>
        /// <param name="fileName">The full path of the file.</param>
        /// <returns>True if the import failed, false otherwise.</returns>
        public bool Import(string fileName)
        {
            bool error = Game.Import(fileName);
            if (!error)
            {
                Update();
            }
            return error;
        }

        /// <summary>
        /// Reset the game to the first <see cref="Generation"/>.
        /// </summary>
        /// <param name="parameter"></param>
        public void Reset(object parameter)
        {
            Game.Reset();
            Update();
        }

        /// <summary>
        /// Disable the Reset Button according to the <see cref="GenerationNumber"/>.
        /// </summary>
        /// <returns>If the button needs to be disable.</returns>
        public bool CanReset(object parameter)
        {
            return GenerationNumber > 0;
        }

        /// <summary>
        /// Jumps n times to the next <see cref="Generation"/>.
        /// </summary>
        /// <param name="number">The number of jumps.</param>
        private void Next(int number)
        {
            for (int i = 0; i < number; i++)
            {
                Game.FastForward();
            }
            Update();
        }

        /// <summary>
        /// Go to the next <see cref="Generation"/>.
        /// </summary>
        public void Next(object parameter)
        {
            Next(1);
        }

        /// <summary>
        /// Jumps 10 <see cref="Generation"/>.
        /// </summary>
        public void Next10(object parameter)
        {
            Next(10);
        }

        /// <summary>
        /// Update the value of the ViewModel from the Model.
        /// </summary>
        private void Update()
        {
            Grid = Game.CurrentGeneration.Grid;
            GenerationNumber = Game.GenerationNumber;
        }
    }
}
