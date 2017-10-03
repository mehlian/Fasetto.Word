using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Fasetto.Word
{
    // The View Model for custom flat window
    public class WindowViewModel : BaseViewModel
    {
        // Window controlled by this VM
        private Window _window;
        // Margin around the window to allow for a drop shadow
        private int _outerMarginSize = 10;
        // Radius of the edges of the window
        private int _windowRadius = 10;
        // The last known dock position
        private WindowDockPosition _dockPosition = WindowDockPosition.Undocked;

        public double WindowMinimumWidth { get; set; } = 400;
        public double WindowMinimumHeight { get; set; } = 400;
        // True if the window should be borderless because it is docked or maximized
        public bool Borderless { get { return (_window.WindowState == WindowState.Maximized || _dockPosition != WindowDockPosition.Undocked); } }
        // The size of the border around the window in px
        public int ResizeBorder { get; set; } = 6;
        // Size of the resize border around the window, taking account the outer margin
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder+OuterMarginSize); } }
        // The padding of the inner content of the main window
        public Thickness InnerContentPadding { get { return new Thickness(ResizeBorder); } }
        // The margin around the window to allow for a drop shadow
        public int OuterMarginSize
        {
            get
            {
                // If it is maximized or docked, no border
                return Borderless ? 0 : _outerMarginSize;
            }
            set
            {
                _outerMarginSize = value;
            }
        }
        // The margin around the window to allow for a drop shadow
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }
        // The radius of the edges of the window
        public int WindowRadius
        {
            get
            {
                // If it is maximized or docked, no border
                return Borderless ? 0 : _windowRadius;
            }
            set
            {
                _windowRadius = value;
            }
        }
        // The radius of the edges of the window
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }
        // The height of the title bar / caption of the window
        public int TitleHeight { get; set; } = 42;
        // The height of the title bar / caption of the window
        public GridLength TitleHeightGridLenght { get { return new GridLength(TitleHeight+ResizeBorder); } }
                
        // Command to minimize window
        public ICommand MinimizeCommand { get; set; }
        // Command to maximize window
        public ICommand MaximizeCommand { get; set; }
        // Command to close window
        public ICommand CloseCommand { get; set; }
        // Command for system MenuIcon
        public ICommand MenuCommand { get; set; }

        // Parameterless ctor for xaml's Designtime
        public WindowViewModel()
        {
        }

        public WindowViewModel(Window window)
        {
            _window = window;

            // Listen out for window resizing
            _window.StateChanged += (sender, e) =>
             {
                 WindowResized();
             };

            // Create commands
            MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => _window.WindowState ^= WindowState.Maximized); // WindowState == Max? 0 : Max //0-Normal state
            CloseCommand = new RelayCommand(() => _window.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(_window,_window.PointToScreen(Mouse.GetPosition(_window))));

            //Fix window resize issue
            var resizer = new WindowResizer(_window);

            // Listen out for dock changes
            resizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                _dockPosition = dock;

                // Fire off resize events
                WindowResized();
            };
        }

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }
    }
}
