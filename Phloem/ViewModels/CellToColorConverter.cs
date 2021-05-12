using Phloem.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Phloem.ViewModels
{
    /// <summary>
    /// This class will convert any <see cref="Cell"/> object into a
    /// corresponding <see cref="SolidColorBrush"/> color. It is used to color
    /// the Game of Life grid.
    /// </summary>
    internal class CellToColorConverter : IValueConverter
    {
        /// <summary>
        /// The color of a living cell.
        /// </summary>
        public static SolidColorBrush AliveColor = Brushes.Purple;

        /// <summary>
        /// The color of a dead cell.
        /// </summary>
        public static SolidColorBrush DeadColor = Brushes.White;

        /// <summary>
        /// Convert any <see cref="Cell"/> object into a <see cref="SolidColorBrush"/> color.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cell = (Cell)value;
            if (cell != null)
            {
                return cell.IsAlive ? AliveColor : DeadColor;
            }
            return DeadColor;
        }

        /// <summary>
        /// Convert any <see cref="Brushes"/> color into a <see cref="Cell"/> object.
        /// Is not implemented because not used.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
