using Avalonia.Controls;
using PlaylistParserApp.ViewModels;

namespace PlaylistParserApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        
    }
}
