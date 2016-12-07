using SoundVisualizer3D.ViewModels;

namespace SoundVisualizer3D.Views
{
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel =  new MainWindowViewModel();

             Loaded += (sender, args) => DataContext = _viewModel;
        }
    }
}