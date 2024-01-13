using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wally
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var imagePaths = new List<string>
            {
                "C:\\Users\\User\\source\\repos\\wally\\Resources\\beautifulWallpaper.jpg",
                "C:\\Users\\User\\source\\repos\\wally\\Resources\\citySunset.jpg",
                "C:\\Users\\User\\source\\repos\\wally\\Resources\\beautifulWallpaper.jpg",
                "C:\\Users\\User\\source\\repos\\wally\\Resources\\citySunset.jpg",

            };

            ImagesItemsControl.ItemsSource = imagePaths;
            foreach(var imagePath in imagePaths)
            {
                var image = new Image
                {
                    Source = new BitmapImage(new Uri(imagePath)),
                    Stretch = Stretch.UniformToFill
                };
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            // Hide all ListBoxes
            ListBoxLibrary.Visibility = Visibility.Collapsed;
            ListBoxStore.Visibility = Visibility.Collapsed;

            // show the specific listbox based on the selected radiobutton
            if (radioButton.Content.ToString() == "Library")
            {
                ListBoxLibrary.Visibility = Visibility.Visible;
            }
            else if (radioButton.Content.ToString() == "Store")
            {
                ListBoxStore.Visibility = Visibility.Visible;
            }

        }

        
    }
    
}