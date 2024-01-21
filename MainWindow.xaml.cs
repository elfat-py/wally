using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using PexelsDotNetSDK.Api;
using static wally.MainWindow;

namespace wally
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync("HD Wallpapers", "Cool Wallpapers");
        }
        
        private async void InitializeAsync(string query1, string query2)//I think we should pass two query here
        {
            string fileName1 = @"C:\Users\User\RiderProjects\ConsoleApp2\ConsoleApp2\results.json";
            string fileName2 = @"C:\Users\User\RiderProjects\ConsoleApp2\ConsoleApp2\results1.json";

            
            ApiLogic connect = new ApiLogic();

            try
            {
                List<ImagesCollectionUrl1> ImageUrlPath1 = await connect.JsonConnection1(query1);
                List<ImagesCollectionUrl2> ImageUrlPath2 = await connect.JsonConnection2(query2);

                // Update your UI using the results, assuming ListBoxStackPanel1 and ListBoxStackPanel2 are your UI elements
                ListBoxStackPanel1.ItemsSource = ImageUrlPath1; //These one seem alright
                ListBoxStackPanel2.ItemsSource = ImageUrlPath2;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //List<ImagesCollectionUrl> ImageUrlPath = await connect.JsonConnection(query);

            //foreach (ImagesCollectionUrl imageUrl in ImageUrlPath)
            //{
            //    ListBoxStackPanel1.ItemsSource = ImageUrlPath;
            //    if (defineColumn % 2 == 0)
            //    {
            //        ListBoxStackPanel1.ItemsSource = ImageUrlPath;
            //        defineColumn++;
            //    }
            //    else
            //    {
            //        //we may are going to need to use listview instead of image stack
            //        ListBoxStackPanel2.ItemsSource = ImageUrlPath;
            //        defineColumn++;
            //    }
            //}
        }

        public class ImagesCollectionUrl2
        {
            public string ImageUrlPath { get; set; }
        }

        public class ImagesCollectionUrl1
        {
            public string ImageUrlPath { get; set; }
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
        private void ListBoxStackPanel2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImagesCollectionUrl2 selectedImage = (ImagesCollectionUrl2)ListBoxStackPanel2.SelectedItem;

            if (ListBoxStackPanel2.SelectedItem != null)
            {
                ShowOptions2(selectedImage);
            }
        }
        private void ListBoxStackPanel1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImagesCollectionUrl1 selectedImage = (ImagesCollectionUrl1)ListBoxStackPanel1.SelectedItem;

            if (ListBoxStackPanel1.SelectedItem != null)
            {
                ShowOptions1(selectedImage);
            }
        }
        private void ShowOptions2(ImagesCollectionUrl2 selectedImage)
        {
            string urlPath = selectedImage.ImageUrlPath; //This is just the url address 
            ContextMenu contextMenu = new ContextMenu();

            MenuItem downloadMenuItem = new MenuItem();
            downloadMenuItem.Header = "Download as Image";
            downloadMenuItem.Icon = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\User\OneDrive\Desktop\IconForDisplaymentMenu\downloadIcon.png",
                    UriKind.RelativeOrAbsolute))
            };
            downloadMenuItem.Click += (sender, e) => SaveWallpaper(urlPath);

            MenuItem setWallpaperMenuItem = new MenuItem();
            setWallpaperMenuItem.Header = "Set as Wallpaper";
            setWallpaperMenuItem.Icon = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\User\OneDrive\Desktop\IconForDisplaymentMenu\setWallpaper.png",
                    UriKind.RelativeOrAbsolute))
            };
            setWallpaperMenuItem.Click += (sender, e) => SaveAndSetWallpaper(urlPath);

            MenuItem reviewMenuItem = new MenuItem();//Here should be an option to e-mail us the problem the user might be having or if he likes the app
            reviewMenuItem.Header = "Review";
            reviewMenuItem.Icon = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\User\OneDrive\Desktop\IconForDisplaymentMenu\review.png",
                    UriKind.RelativeOrAbsolute))
            };
            //reviewMenuItem.Click += (sender, e) => ReviewImage(selectedImage);

            contextMenu.Items.Add(downloadMenuItem);
            contextMenu.Items.Add(setWallpaperMenuItem);
            contextMenu.Items.Add(reviewMenuItem);

            // Attach the context menu to the ListBox or any UI element you want
            ListBoxStackPanel2.ContextMenu = contextMenu;
        }

        private void ShowOptions1(ImagesCollectionUrl1 selectedImage)
        {
            string urlPath = selectedImage.ImageUrlPath; 
            ContextMenu contextMenu = new ContextMenu();

            MenuItem downloadMenuItem = new MenuItem();
            downloadMenuItem.Header = "Download as Image"; 
            downloadMenuItem.Icon = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\User\OneDrive\Desktop\IconForDisplaymentMenu\downloadIcon.png",
                    UriKind.RelativeOrAbsolute))
            };
            downloadMenuItem.Click += (sender, e) => SaveWallpaper(urlPath);

            MenuItem setWallpaperMenuItem = new MenuItem();
            setWallpaperMenuItem.Header = "Set as Wallpaper";
            setWallpaperMenuItem.Icon = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\User\OneDrive\Desktop\IconForDisplaymentMenu\setWallpaper.png",
                    UriKind.RelativeOrAbsolute))
            };
            setWallpaperMenuItem.Click += (sender, e) => SaveAndSetWallpaper(urlPath);

            MenuItem reviewMenuItem = new MenuItem();//Here should be an option to e-mail us the problem the user might be having or if he likes the app
            reviewMenuItem.Header = "Review";
            reviewMenuItem.Icon = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\User\OneDrive\Desktop\IconForDisplaymentMenu\review.png",
                    UriKind.RelativeOrAbsolute))
            };

            contextMenu.Items.Add(downloadMenuItem);
            contextMenu.Items.Add(setWallpaperMenuItem);
            contextMenu.Items.Add(reviewMenuItem);

            ListBoxStackPanel1.ContextMenu = contextMenu;
        }

        private int NumberIdGenerator()
        {
            Random random = new Random();
            return random.Next(1, 99999);
        }

        private string SaveWallpaper(string imageUrl)
        {
            int numberOfWalli = NumberIdGenerator(); //This one doesn't work will need something more standard
            string directoryPath = @"C:\Users\User\source\repos\wally\Resources\SavedWallpapersWalli";
            string customFileName = $"walli{numberOfWalli}.jpg";
            string fullPath = System.IO.Path.Combine(directoryPath, customFileName);
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                try
                {
                    client.DownloadFile(imageUrl, fullPath);
                    // Return the full path after successfully downloading the image
                    return fullPath;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"There was a problem downloading the image: {e}");

                    return null;
                }
            }
        }

        private void SaveAndSetWallpaper(string imageUrl)
        {
            string walliPath = SaveWallpaper(imageUrl);
            if (walliPath != null)
            {
                WallpaperSetter setter = new WallpaperSetter();
                setter.SetWallpaper(walliPath);
                MessageBox.Show("Wallpaper set successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Wallpaper couldn't be set!!", "Failed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public class WallpaperSetter
        {
            // Import the SystemParametersInfo function
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

            // Define the constants for SystemParametersInfo
            private const int SPI_SETDESKWALLPAPER = 0x0014;
            private const int SPIF_UPDATEINIFILE = 0x01;
            private const int SPIF_SENDCHANGE = 0x02;

            public void SetWallpaper(string imagePath)
            {
                if (!System.IO.File.Exists(imagePath))
                {
                    MessageBox.Show($"Image file not found: {imagePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int result = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
                if (result == 0)
                {
                    MessageBox.Show("Failed to set wallpaper", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ListBoxLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxLibrary.SelectedItem != null)
            {
                ListBoxItem selectedItem = (ListBoxItem)ListBoxLibrary.SelectedItem;

                // Should find a better way of searching probably
                switch (selectedItem.Name)
                {
                    case "All":
                        InitializeAsync("HD Wallpapers", "PC Wallpapers");
                        break;
                    case "Animals":                       
                        InitializeAsync("Animals", "Cute Animals");
                        break;
                    case "Artwork":
                        InitializeAsync("Art", "Abstract Art Backgrounds");
                        break;
                    case "Gaming":
                        InitializeAsync("Video Games", "Gaming wallpapers"); //Some problem here
                        break;
                    case "Motivation":
                        InitializeAsync("Motivation", "Inspiration");
                        break;
                    case "Music":
                        InitializeAsync("Music","Mood");
                        break;
                    case "Nature":
                        InitializeAsync("nature wallpaper", "Scenic Views Wallpaper");
                        break;
                    case "Holiday":
                        InitializeAsync("Holiday", "Adventure is Out There"); //There seems to be some problem here as well
                        break;
                    case "Space":
                        InitializeAsync("galaxy wallpaper", "Space Wallpapers");
                        break;
                    case "Tech":
                        InitializeAsync("Tech", "Inventions");
                        break;
                    case "Lifestyle":
                        InitializeAsync("Lifestyle", "Into The Woods");
                        break;
                    case "Food":
                        InitializeAsync("Food", "Drinks");
                        break;
                    case "Anime":
                        InitializeAsync("Cartoon", "Animation");
                        break;
                    case "Cars":
                        InitializeAsync("Cars wallpaper", "Super Car 4k Wallpaper");
                        break;
                    case "Weather":
                        InitializeAsync("Weather", "Weather Landscapes"); 
                        break;
                    default:
                        MessageBox.Show("There seems to be some problem check your internet!", caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }
        class ApiLogic
        {
            public async Task<List<ImagesCollectionUrl1>> JsonConnection1(string ourQuery)
            {
                PexelsApiCLient client = new PexelsApiCLient("R2uHSUSWSF7nmqxIl6Q0yAU0MjQCgtHpGCs56qChJn77SA5HCrudg4sr");
                ParseJson parseJson = new ParseJson();
                string firstFilePath = @"C:\Users\User\RiderProjects\ConsoleApp2\ConsoleApp2\results.json";

                try
                {
                    parseJson.ExtractJsonData(firstFilePath);

                    List<string> imageLinks = await client.RetrieveJson(ourQuery, firstFilePath);
                    return imageLinks.Select(link => new ImagesCollectionUrl1 { ImageUrlPath = link }).ToList();

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<ImagesCollectionUrl1>();
                }
                
            }
            public async Task<List<ImagesCollectionUrl2>> JsonConnection2(string ourQuery)
            {
                PexelsApiCLient client = new PexelsApiCLient("R2uHSUSWSF7nmqxIl6Q0yAU0MjQCgtHpGCs56qChJn77SA5HCrudg4sr");
                ParseJson parseJson = new ParseJson();
                string secFilePath = @"C:\Users\User\RiderProjects\ConsoleApp2\ConsoleApp2\results1.json";

                try
                {
                    parseJson.ExtractJsonData(secFilePath);
                    List<string> imageLinks = await client.RetrieveJson(ourQuery, secFilePath);
                    return imageLinks.Select(link => new ImagesCollectionUrl2 { ImageUrlPath = link }).ToList();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<ImagesCollectionUrl2>();
                }
            }
        }

        public class PexelsApiCLient
        {
            private readonly string APIkey; //"R2uHSUSWSF7nmqxIl6Q0yAU0MjQCgtHpGCs56qChJn77SA5HCrudg4sr"
            private readonly PexelsClient pexelsClient;

            public PexelsApiCLient(string APIkey)
            {
                if (string.IsNullOrWhiteSpace(APIkey))
                {
                    throw new ArgumentException("The api key is empty");
                }
                this.APIkey = APIkey;
                this.pexelsClient = new PexelsClient(APIkey);

            }

            public async Task<List<string>> RetrieveJson(string query, string filePath) //Here we should also pass the query
            {
                try
                {
                    var resultCuratedPhotos = await pexelsClient.CuratedPhotosAsync(pageSize: 5); //This is supposed to get us back professional photos only

                    var result = await pexelsClient.SearchPhotosAsync(query: query, orientation: "landscape"); //The query can be whatever

                    // Convert the result to a JSON string and display it
                    var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
                    //var filePath = @"C:\Users\User\RiderProjects\ConsoleApp2\ConsoleApp2\results.json"; //This should be for one group of wallpapers we got a lot more
                    await File.WriteAllTextAsync(filePath, jsonResult);
                    Console.WriteLine($"Saved data {filePath}");
                    
                    return ParseJson.GetLandscapeUrls(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, caption:"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                   
                    return new List<string>();
                }
            }
        }

        public class ParseJson
        {
            public class Src
            {
                public string Original { get; set; }
                public string Landscape { get; set; }

                // public string Large { get; set; }
                // public string Large2x { get; set; }
                // public string Medium { get; set; }
                // public string Small { get; set; }
                // public string Portrait { get; set; }
                // public string Tiny { get; set; }
            }

            public class Photo
            {
                public int Id { get; set; }
                public Src Src { get; set; }
                public string Alt { get; set; }
                public string Url { get; set; }

                // WE DO NOT NEED THESE AT THE MOMENT
                // public int Width { get; set; }
                // public int Height { get; set; }
                // public string Photographer { get; set; }
                // public string PhotographerUrl { get; set; }
                // public string PhotographerId { get; set; }
                // public bool Liked { get; set; }
                // public string AvgColor { get; set; }
                // public string Alt { get; set; }
            }

            public class RootObject
            {
                public List<Photo> Photos { get; set; }
            }

            public static List<string> GetLandscapeUrls(string filePath)
            {
                string json = File.ReadAllText(filePath);
                List<string> landscapeUrls = new List<string>();
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(json);

                if (rootObject != null)
                {
                    foreach (var url in rootObject.Photos)
                    {
                        string landscapeUrl = url.Src?.Landscape;

                        if (!string.IsNullOrEmpty(landscapeUrl))
                        {
                            landscapeUrls.Add(landscapeUrl);
                        }

                    }
                }
                return landscapeUrls;
            }

            public void ExtractJsonData(string filePath)
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    //Console.WriteLine(json);
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(json);
                    if (rootObject != null && rootObject.Photos != null)
                    {
                        foreach (var photo in rootObject.Photos)
                        {
                            //I think we should save them in some class or somewhere 
                            Console.WriteLine($"Photo ID: {photo.Id}");
                            Console.WriteLine($"URL: {photo.Url}");
                            Console.WriteLine($"Alt: {photo.Alt}");

                            if (photo.Src != null)
                            {
                                Console.WriteLine($"Original: {photo.Src.Original}");

                                Console.WriteLine($"Landscape: {photo.Src.Landscape}");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error extracting data!", MessageBoxButton.OK, MessageBoxImage.Error);
                    //Console.WriteLine(e);
                    throw;
                }
            }
        }
        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            string searchBoxQuery = SearchTextBox.Text; 
            string addedPart = " new wallpapers"; //Temprorary solution
            string searchBoxQuery2 = string.Concat(searchBoxQuery, addedPart);
            InitializeAsync(searchBoxQuery, searchBoxQuery2);
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;

        }
    }
}
