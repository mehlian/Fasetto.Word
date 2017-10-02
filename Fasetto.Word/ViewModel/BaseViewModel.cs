using PropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Fasetto.Word
{
    // A base view model that fires Property Changed events as needed
    public class BaseViewModel : INotifyPropertyChanged
    {
        // The event that is fired when any child property changes its value
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            // If PropertChanged != null, -> Invoke. (? - null checker)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
