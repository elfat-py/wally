using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using PexelsDotNetSDK.Api;

namespace wally
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync("HD Wallpapers");
        }
        private async void InitializeAsync(string query)//Should pass the query here
        {
            int defineColumn = 1;
            ApiLogic connect = new ApiLogic();
            List<ImagesCollectionUrl> ImageUrlPath = await connect.JsonConnection(query);

            foreach (ImagesCollectionUrl imageUrl in ImageUrlPath)
            {
                ListBoxStackPanel1.ItemsSource = ImageUrlPath;
                if (defineColumn % 2 == 0)
                {
                    ListBoxStackPanel1.ItemsSource = ImageUrlPath;
                    defineColumn++;
                }
                else
                {
                    //we may are going to need to use listview instead of image stack
                    ListBoxStackPanel2.ItemsSource = ImageUrlPath;
                    defineColumn++;
                }
            }
        }
        public class ImagesCollectionUrl
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
            ImagesCollectionUrl selectedImage = (ImagesCollectionUrl)ListBoxStackPanel2.SelectedItem;

            if (ListBoxStackPanel2.SelectedItem != null)
            {
                ShowOptions(selectedImage);
            }
        }

        private void ShowOptions(ImagesCollectionUrl selectedImage)
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

            // Method to set the wallpaper
            public void SetWallpaper(string imagePath)
            {
                // Check if the file exists
                if (!System.IO.File.Exists(imagePath))
                {
                    MessageBox.Show($"Image file not found: {imagePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Set the wallpaper
                int result = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);

                // Check the result
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

                // Perform action based on the selected item
                switch (selectedItem.Name)
                {
                    case "All":
                        InitializeAsync("HD Wallpapers");
                        break;
                    case "Animals":                       
                        InitializeAsync("Animals");
                        break;
                    case "Artwork":
                        InitializeAsync("Art");
                        break;
                    case "Gaming":
                        InitializeAsync("Gaming");
                        break;
                    case "Motivation":
                        InitializeAsync("Motivation");
                        break;
                    case "Music":
                        InitializeAsync("Music");
                        break;
                    case "Nature":
                        InitializeAsync("nature wallpaper");
                        break;
                    case "Holiday":
                        InitializeAsync("Holiday");
                        break;
                    case "Space":
                        InitializeAsync("galaxy wallpaper");
                        break;
                    case "Tech":
                        InitializeAsync("Tech");
                        break;
                    case "Lifestyle":
                        InitializeAsync("Lifestyle");
                        break;
                    case "Food":
                        InitializeAsync("Food");
                        break;
                    case "Cartoon":
                        InitializeAsync("Cartoon");
                        break;
                    default:
                        break;
                }
            }
        }
        class ApiLogic
        {
            public async Task<List<ImagesCollectionUrl>> JsonConnection(string ourQuery)
            {
                PexelsApiCLient client = new PexelsApiCLient("R2uHSUSWSF7nmqxIl6Q0yAU0MjQCgtHpGCs56qChJn77SA5HCrudg4sr");
                 //await client.RetrieveJson();

                ParseJson parseJson = new ParseJson();
                parseJson.ExtractJsonData();
                List<string> imageLinks = await client.RetrieveJson(ourQuery);
                return imageLinks.Select(link => new ImagesCollectionUrl { ImageUrlPath = link }).ToList();

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

            public async Task<List<string>> RetrieveJson(string query) //Here we should also pass the query
            {
                try
                {
                    var resultCuratedPhotos =
                        await pexelsClient
                            .CuratedPhotosAsync(pageSize: 5); //This is supposed to get us back professional photos only

                    var result =
                        await pexelsClient.SearchPhotosAsync(query: query,
                            orientation: "landscape"); //The query can be whatever

                    // Convert the result to a JSON string and display it
                    var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
                    var filePath = @"C:\Users\User\RiderProjects\ConsoleApp2\ConsoleApp2\results.json"; //This should be for one group of wallpapers we got a lot more
                    await File.WriteAllTextAsync(filePath, jsonResult);
                    Console.WriteLine("Saved data");
                    //Console.WriteLine(jsonResult);
                    return ParseJson.GetLandscapeUrls(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return new List<string>();
                }
            }
        }

        public class ParseJson
        {
            string filePath = @"C:\Users\User\RiderProjects\ConsoleApp2\ConsoleApp2\results.json";

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
            public void ExtractJsonData()
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
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}