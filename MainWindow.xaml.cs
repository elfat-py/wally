using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        private void ListBoxLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxLibrary.SelectedItem != null)
            {
                // Get the selected item
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
        private async void InitializeAsync(string query)//Should pass the query here
        {
            int defineColumn = 1;
            ApiLogic connect = new ApiLogic();      
            List<ImagesCollectionUrl> ImageUrlPath = await connect.JsonConnection(query);

            foreach(ImagesCollectionUrl imageUrl in ImageUrlPath)
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

        class ImagesCollectionUrl
        {
            public required string ImageUrlPath { get; set; }
        }

        private void loadpreviewimages(List<ImagesCollectionUrl>ImageUrlPath)
        {
            int definecolumn = 1;
            foreach (var imageUrl in ImageUrlPath)
            {
                // create a new image control
                Image imagecontrol = new Image();

                // create a bitmapimage and set its urisource to the image url
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                //bitmapimage.urisource = new uri(imageurl);
                bitmapimage.UriSource = new Uri(imageUrl.ImageUrlPath);
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                imagecontrol.Source = bitmapimage;

                
               
                
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