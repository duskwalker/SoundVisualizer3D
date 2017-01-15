using SoundVisualizer3D.ViewModels;

namespace SoundVisualizer3D.Views
{
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly ViewportViewModel _viewPortViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel =  new MainWindowViewModel();
            _viewPortViewModel = new ViewportViewModel();

            Loaded += (sender, args) =>
            {
                DataContext = _viewModel;
                ViewportContent.Content = _viewPortViewModel;
            };
        }
    }
}