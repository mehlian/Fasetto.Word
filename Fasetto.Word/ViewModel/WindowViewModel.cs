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

        public double WindowMinimumWidth { get; set; } = 400;
        public double WindowMinimumHeight { get; set; } = 400;
        // The size of the border around the window in px
        public int ResizeBorder { get; set; } = 6;
        // Size of the resize border around the window, taking account the outer margin
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder+OuterMarginSize); } }
        public int OuterMarginSize
        {
            get
            {
                return _window.WindowState == WindowState.Maximized ? 0 : _outerMarginSize;
            }
            set
            {
                _outerMarginSize = value;
            }
        }
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }
        public int WindowRadius
        {
            get
            {
                return _window.WindowState == WindowState.Maximized ? 0 : _windowRadius;
            }
            set
            {
                _windowRadius = value;
            }
        }
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }
        public int TitleHeight { get; set; } = 42;
        public GridLength TitleHeightGridLenght { get { return new GridLength(TitleHeight+ResizeBorder); } }
        public Thickness InnerContentPadding { get { return new Thickness(ResizeBorder); } }

        
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
                 //Fire off events for all properties that are affected by resize
                 OnPropertyChanged(nameof(ResizeBorderThickness));
                 OnPropertyChanged(nameof(OuterMarginSize));
                 OnPropertyChanged(nameof(OuterMarginSizeThickness));
                 OnPropertyChanged(nameof(WindowRadius));
                 OnPropertyChanged(nameof(WindowCornerRadius));
             };

            // Create commands
            MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => _window.WindowState ^= WindowState.Maximized); // WindowState == Max? 0 : Max //0-Normal state
            CloseCommand = new RelayCommand(() => _window.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(_window,_window.PointToScreen(Mouse.GetPosition(_window))));

            //Fix window resize issue
            var resizer = new WindowResizer(_window);
        }
    }
}
