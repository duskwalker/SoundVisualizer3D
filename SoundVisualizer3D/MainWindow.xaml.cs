using System;
using System.Windows;

namespace SoundVisualizer3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel =  new MainWindowViewModel();
             Loaded += (sender, args) => DataContext = _viewModel;
        }

        private void OnPlay_Clicked(object sender, RoutedEventArgs e)
        {
            _viewModel.Play();
        }

        private void OnStop_Clicked(object sender, RoutedEventArgs e)
        {
            _viewModel.Stop();
        }
    }
}
