using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using Newtonsoft.Json;
using ComedyKing.Model;
using System.Text;
using System.Collections.ObjectModel;
using Windows.Networking.Connectivity;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Data.SqlClient;
using System.Data;

namespace ComedyKing.App.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<CelebrityJoke> CelebrityJokes { get; set; } = new ObservableCollection<CelebrityJoke>();
        static Uri celebrityJokeUri = new Uri("http://localhost:56383/api/CelebrityJokes");
        HttpClient httpCelebrityJokesUri = new HttpClient();
        public CelebrityJoke ViewModel { get; set; }
        private CancellationTokenSource cts;
        private CelebrityJoke selectedJoke;
        public CelebrityJoke SelectedJoke
        {
            get { return selectedJoke; }
            set
            {
                if (selectedJoke != value)
                {
                    selectedJoke = value;
                }
            }
        }

        public enum Days
        {
            Monday = 1,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        public void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
             CheckInternetAccessAvailableAsync();
        }

        public MainPage()
        {
            InitializeComponent();

            //Timer inteval for checking internet connection
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(1);
            System.Timers.Timer timer = new System.Timers.Timer
            {
                Interval = 1000
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
         
        //Detects if the device is connected to the internet or not
        public async Task CheckInternetAccessAvailableAsync()
        {    
            bool getIsInternetAccessAvailable()
            {
               switch (NetworkInformation.GetInternetConnectionProfile().GetNetworkConnectivityLevel())
               {
                   case NetworkConnectivityLevel.InternetAccess:
                       return true;
                   default:
                       return false;
               }
            }

            if (getIsInternetAccessAvailable())
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                   Internet.Foreground = new SolidColorBrush(Colors.Green);
                   Internet.Text = "•ONLINE";
                });
               
            }

            else
            {
               await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   Internet.Foreground = new SolidColorBrush(Colors.Red);
                   Internet.Text = "•OFFLINE";
               });
            }
        }
        

        // Loads jokes from the database when page is loaded
        private async void Page_Loaded_Async(object sender, RoutedEventArgs e)
        {
           //Shows a message with the countdown before the weekend
            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;
            switch (dayOfWeek)
            {
                    case DayOfWeek.Monday:
                        WeekendCountdownMessage.Text = Days.Friday - Days.Monday + " DAYS UNTIL THE WEEKEND";
                        break;
                    case DayOfWeek.Tuesday:
                        WeekendCountdownMessage.Text = Days.Friday - Days.Tuesday + " DAYS UNTIL THE WEEKEND";
                        break;
                    case DayOfWeek.Wednesday:
                        WeekendCountdownMessage.Text = Days.Friday - Days.Wednesday + " DAYS UNTIL THE WEEKEND";
                        break;
                    case DayOfWeek.Thursday:
                        WeekendCountdownMessage.Text = Days.Friday - Days.Thursday + " DAY UNTIL THE WEEKEND";
                        break;
                    case DayOfWeek.Friday:
                        WeekendCountdownMessage.Text = "WEEKEND STARTS TONIGHT";
                        break;
                    case DayOfWeek.Saturday:
                        WeekendCountdownMessage.Text = "LET'S ENJOY THE WEEKEND";
                        break;
                    case DayOfWeek.Sunday:
                        WeekendCountdownMessage.Text = "SUNDAY MOOD";
                        break;
            } 

            // Mouse cursor changes  when loading data
            var defaultCursor = Window.Current.CoreWindow.PointerCursor;
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
           
             try
             {
                 var result = await httpCelebrityJokesUri.GetAsync(celebrityJokeUri);
                 var celebrityJokeJson = await result.Content.ReadAsStringAsync();
                 var celebrityJokes = JsonConvert.DeserializeObject<CelebrityJoke[]>(celebrityJokeJson);
                 Window.Current.CoreWindow.PointerCursor = defaultCursor;

                 foreach (CelebrityJoke CJ in celebrityJokes)
                 {
                    CelebrityJokes.Add(CJ);
                 }
             }

             catch (HttpRequestException ex)
             {
                 Window.Current.CoreWindow.PointerCursor = defaultCursor;
                 var dialog = new MessageDialog(ex.GetType().Name+ ": Database connection error. Please check VPN and network connection.");
                 await dialog.ShowAsync();
                 FileLogger.LogExceptions(DateTime.Now +" "+ ex.GetType().Name+ ex.Message + ex.StackTrace + "\n"+ "\n");
             }
        }        


  
        // Selected joke from liestview is set to "Selected joke"
        private void LV_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (CelebrityJoke)e.ClickedItem;
            var collection = (ObservableCollection<CelebrityJoke>)ListView.ItemsSource;
            int index = collection.IndexOf(clickedItem);
            SelectedJoke = CelebrityJokes[index];
        }

        // Reload all the jokes from the database
        private async void Button_All_Jokes_Async(object sender, RoutedEventArgs e)
        {
            // Mouse cursor changes to wait when loading data
            var defaultCursor = Window.Current.CoreWindow.PointerCursor;
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);

           try
           {
              CelebrityJokes.Clear();
              var result = await httpCelebrityJokesUri.GetAsync(celebrityJokeUri);
              var celebrityJokeJson = await result.Content.ReadAsStringAsync();
              var celebrityJokes = JsonConvert.DeserializeObject<CelebrityJoke[]>(celebrityJokeJson);
              Window.Current.CoreWindow.PointerCursor = defaultCursor;
              foreach (CelebrityJoke CJ in celebrityJokes)
              {
                 CelebrityJokes.Add(CJ);
              }
           }

           catch (HttpRequestException ex)
           {
               Window.Current.CoreWindow.PointerCursor = defaultCursor;
               var dialog = new MessageDialog(ex.GetType().Name + ": Database connection error. Please check VPN and network connection.");
               await dialog.ShowAsync();
               FileLogger.LogExceptions(DateTime.Now + " " + ex.GetType().Name + ex.Message + ex.StackTrace + "\n" + "\n");
           }
        }

        // Creates a new joke
        private void Button_NewJoke(object sender, RoutedEventArgs e)
        {
           CelebrityJokes.Clear();
           var newJoke = new CelebrityJoke()
           {
              Text = "",
              CelebrityMentioned = "",
              Author = "",
              Rate = 0
           };

           CelebrityJokes.Add(newJoke);
        }



        //Adds the new joke to the list
        private async void Button_Add_Joke_Async(object sender, RoutedEventArgs e)
        {  
            var stringContent = new StringContent(JsonConvert.SerializeObject(SelectedJoke), Encoding.UTF8, "application/json");
            try
            {
                if (SelectedJoke != null)
                {
                    if (SelectedJoke.CelebrityMentioned != "" && SelectedJoke.Author != "" && SelectedJoke.Text != "")
                    {
                        // Add the new joke to the database
                        await httpCelebrityJokesUri.PostAsync(celebrityJokeUri, stringContent).ConfigureAwait(false);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        async () =>
                        {
                            var dialog = new MessageDialog("Joke added to the database successfully");
                            await dialog.ShowAsync();
                        });
                    }

                    else
                    {
                        var dialog = new MessageDialog("Please fill in the required fields.");
                        await dialog.ShowAsync();
                    }
                }

                else
                {
                    var dialog = new MessageDialog("Please select a joke.");
                    await dialog.ShowAsync();
                }
            }

            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog(ex.GetType().Name + ": Cannot add joke to database.Please check VPN or network connection.");
                await dialog.ShowAsync();
                FileLogger.LogExceptions(DateTime.Now + " " + ex.GetType().Name + ex.Message + ex.StackTrace + "\n" + "\n");
            }

            try
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
                    CelebrityJokes.Clear();
                    var result = await httpCelebrityJokesUri.GetAsync(celebrityJokeUri);
                    var celebrityJokeJson = await result.Content.ReadAsStringAsync();
                    var celebrityJokes = JsonConvert.DeserializeObject<CelebrityJoke[]>(celebrityJokeJson);

                    foreach (CelebrityJoke CJ in celebrityJokes)
                    {
                        CelebrityJokes.Add(CJ);

                    }
                });

            }
           
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog(ex.GetType().Name + ": Database connection error. Please check VPN and network connection.");
                await dialog.ShowAsync();
                FileLogger.LogExceptions(DateTime.Now + " " + ex.GetType().Name + ex.Message + ex.StackTrace + "\n" + "\n");

            }
        }


        //Save changes on existing joke
        private async void Button_Save_Joke_Async(object sender, RoutedEventArgs e)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(SelectedJoke), Encoding.UTF8, "application/json");
            try
            {
                if (SelectedJoke !=null)
                {
                    if (SelectedJoke.CelebrityMentioned !="" && SelectedJoke.Author !="" && SelectedJoke.Text !="")
                    {
                        await httpCelebrityJokesUri.PutAsync(celebrityJokeUri + "//" + SelectedJoke.JokeID, stringContent).ConfigureAwait(false);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                         async () =>
                         {
                           var dialog = new MessageDialog("Changes saved successfully");
                           await dialog.ShowAsync();
                         });

                    }
                    else
                    {
                        var dialog = new MessageDialog("Please fill in the required fields.");
                        await dialog.ShowAsync();
                    }
                }
                else
                {
                    var dialog = new MessageDialog("Please select a joke.");
                    await dialog.ShowAsync();
                }
            }

            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog(ex.GetType().Name + ": Cannot change existing joke. Please check VPN or network connection.");
                await dialog.ShowAsync();
                FileLogger.LogExceptions(DateTime.Now + " " + ex.GetType().Name + ex.Message + ex.StackTrace + "\n" + "\n");
            }
        }
    

        // Delete joke
        private async void Button_Delete_Joke_Async(object sender, RoutedEventArgs e)
        {
            cts = new CancellationTokenSource();            
            try
            {
                if (SelectedJoke != null)
                {
                    Uri celebrityJokeUri = new Uri("http://localhost:56383/api/CelebrityJokes/" + SelectedJoke.JokeID);
                    await PutTaskDelay();
                    await httpCelebrityJokesUri.DeleteAsync(celebrityJokeUri, cts.Token);
                    CelebrityJokes.Remove(SelectedJoke);
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                     async () =>
                     {
                         var dialog = new MessageDialog("Joke deleted from the database successfully");
                         await dialog.ShowAsync();
                     });  
                }
                else
                {
                    var dialog = new MessageDialog("Please select a joke.");
                    await dialog.ShowAsync();
                }
            }
            catch (OperationCanceledException ex)
            {
                var dialog = new MessageDialog(ex.GetType().Name + ": Could not cancel the operation: Deleting joke.");
                await dialog.ShowAsync();
                FileLogger.LogExceptions(DateTime.Now + " " + ex.GetType().Name + ex.Message + ex.StackTrace + "\n" + "\n");
            }
            

            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog(ex.GetType().Name + ": Cannot delete joke. Please check VPN or network connection.");
                await dialog.ShowAsync();
                FileLogger.LogExceptions(DateTime.Now + " " + ex.GetType().Name + ex.Message + ex.StackTrace + "\n" + "\n");
            }
        }

        async Task PutTaskDelay()
        {
            try
            {
                await Task.Delay(5000, cts.Token);
            }
            catch (TaskCanceledException ex)
            {
                var dialog = new MessageDialog(ex.GetType().Name + ": Deleting canceled.");
                await dialog.ShowAsync();
                FileLogger.LogExceptions(DateTime.Now + " " + ex.GetType().Name + ex.Message + ex.StackTrace + "\n" + "\n");

            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.GetType().Name + ": Deleting is not possible.");
                await dialog.ShowAsync();
                FileLogger.LogExceptions(DateTime.Now + " " + ex.GetType().Name + ex.Message + ex.StackTrace + "\n" + "\n");
            }
        }
      

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void Button_Cancel_Async(object sender, RoutedEventArgs e)
        {
            this.cts.Cancel();
        }


        // Searching for a celebrity mentioned in the joke and showing the relevant jokes
        private async void Button_Search_Async(object sender, RoutedEventArgs e)
        {
            var input = SearchField.Text;

            SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "donau.hiof.no",
                UserID = "andreot",
                InitialCatalog = "andreot",
                Password = "JGQZn5qV"
            };

            var inputSearch = SearchField.Text;

            try
            {
                //Initialize the Sql-connection 
                using (SqlConnection conn = new SqlConnection(connStringBuilder.ToString()))
                {   
                    if (inputSearch !="")
                    {
                       //Initialize the Sql-command.
                       //Sql-command : Selects the JokeID from the rows who are matching the search
                       SqlCommand cmd = new SqlCommand("SELECT JokeID FROM dbo.Jokes WHERE CelebrityMentioned LIKE'%" + inputSearch + "%'", conn);
                  
                        if (conn.State == ConnectionState.Closed)
                        {
                         
                          //Opens the database connection 
                           conn.Open();

                           // Sends the CommandText to the Connection and builds a SqlDataReader.
                           var reader = cmd.ExecuteReader();
                           CelebrityJokes.Clear();
                    
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                   var jokeID = reader.GetInt32(0);
                                   //Gets relevant jokes from database
                                   var result = await httpCelebrityJokesUri.GetAsync(celebrityJokeUri + "//" + jokeID.ToString());
                                   var celebrityJokeJson = await result.Content.ReadAsStringAsync();
                                   var celebrityJokes = JsonConvert.DeserializeObject<CelebrityJoke>(celebrityJokeJson);

                                  // Add relevant jokes to list
                                 CelebrityJokes.Add(celebrityJokes);
                                }
                            } 
 
                            else
                            {
                              var dialog = new MessageDialog("Sorry we couldn't find a joke containing \"" + inputSearch+ "\"");
                              await dialog.ShowAsync();
                            }
                        
                        }
                    }
                    else
                    {
                        var dialog = new MessageDialog("Please fill out the search field");
                        await dialog.ShowAsync();
                    }
                }

            }
       
            catch (SqlException ex)
            {
                var dialog = new MessageDialog("Something went wrong with the SQL connection.");
                await dialog.ShowAsync();
                FileLogger.LogExceptions(DateTime.Now + " " + ex.GetType().Name + ex.Message + ex.StackTrace + "\n" + "\n");
            }  
        }
    }
}


