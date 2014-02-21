using System.Windows;
using System.Windows.Input;

namespace AnimeBox 
{
    /// <summary>
    /// minimusic.xaml 的交互逻辑
    /// </summary>
    public partial class minimusic : Window
    {
        MainWindow main = new MainWindow();
        public minimusic()
        {
            InitializeComponent();
        }

        private void btnminiplay_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnmininext_Click(object sender, RoutedEventArgs e)
        {
            main.next();
        }

        private void btnminiback_Click(object sender, RoutedEventArgs e)
        {
            main.back();
        }

        private void btnminipause_Click(object sender, RoutedEventArgs e)
        {
            main.pause();
        }

        private void minimusicbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            main.mainmusicbox.Visibility = Visibility.Visible;
            this.Close();
        }

      
    }
}
