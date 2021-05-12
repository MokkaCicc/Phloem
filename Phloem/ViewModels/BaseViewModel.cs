using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Phloem.ViewModels
{
    /// <summary>
    /// This class is the base class for all ViewModels. It implements
    /// <see cref="INotifyPropertyChanged"/> to enable bindings betwenn Views
    /// and ViewModels.
    /// </summary>
    internal class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// This events is raised when the method 
        /// <see cref="NotifyPropertyChanged"/> is called inside a property.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// This method must be called each time a property need to raised a
        /// property changed event. The property will be the calling member name.
        /// </summary>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
