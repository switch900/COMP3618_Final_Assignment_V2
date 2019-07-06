using IMDbDotNetAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for EditTitleBasicForm.xaml
    /// </summary>
    public partial class EditTitleBasicForm : Window
    {
        public ObservableCollection<titlebasic> ObserveTitleBasicList { get; set; }
        public static List<titlebasic> TitleBasicList;
        private string titleBasicID = "";

        public EditTitleBasicForm()
        {
            InitializeComponent();
            Run();
        }

        //starts UI and populates it with current EmployeeID.  Default is 0 for none.  
        public void Run()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:2105/")
            };
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        //starts UI and populates it with a selected EmployeeID. 
        public void GetTitleBasicByID(string id)
        {
            titleBasicID = id;
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:2105/")
            };
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
            var url = "api/titlebasics/" + titleBasicID + "?startindex=0&pagesize=1";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var titlebasics = response.Content.ReadAsAsync<IEnumerable<titlebasic>>().Result;
                foreach (titlebasic temp in titlebasics)
                {
                    ObserveTitleBasicList = new ObservableCollection<titlebasic>(titlebasics);
                }
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
            DataContext = ObserveTitleBasicList;
            Show();
        }

        private async void SaveClose_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (titleBasicID == "")
            {
                NewTitleBasic();
            }
            else
            {
                await UpdateTitleBasicByIdAsync(titleBasicID);
            }
            Hide();
        }

        //saves changes but UI remains active.
        private async void Save_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (titleBasicID == "")
            {
                NewTitleBasic();
            }
            else
            {
                await UpdateTitleBasicByIdAsync(titleBasicID);
            }
        }

        //reloads page with original information from the last save.
        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            GetTitleBasicByID(titleBasicID);
        }

        //creates new employee and saves it to databse from input from user.
        private async void NewTitleBasic()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:2105/")
            };
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/xml"));

            bool ifSuccessStartYear = short.TryParse(startYearTextBox.Text, out short tempStartYear);
            bool ifSuccessEndYear = short.TryParse(endYearTextBox.Text, out short tempEndYear);
            bool ifSuccessRunTimeMinutes = short.TryParse(runTimeMinutesTextBox.Text, out short tempRunTimeMinutes);

            var titlebasic = new titlebasic
            {
                tconst = tconstTextBox.Text,
                titleType = titleTypeTextBox.Text,
                primaryTitle = primaryTitleTextBox.Text,
                originalTitle = originaltitleTextBox.Text,
                isAdult = Convert.ToBoolean(isAdultComboBox.Text),
                startYear = tempStartYear,
                endYear = tempEndYear,
                runtimeMinutes = tempRunTimeMinutes,
                genres = genreTextBox.Text
            };
            var response = await client.PostAsXmlAsync("api/titlebasics", titlebasic);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Titlebasic Added");
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        //updates employee information in database on selected employee
        private async Task UpdateTitleBasicByIdAsync(string id)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:2105/")
            };
            var url = "api/titlebasics/" + id;

            bool ifSuccessStartYear = short.TryParse(startYearTextBox.Text, out short tempStartYear);
            bool ifSuccessEndYear = short.TryParse(endYearTextBox.Text, out short tempEndYear);
            bool ifSuccessRunTimeMinutes = int.TryParse(runTimeMinutesTextBox.Text, out int tempRunTimeMinutes);

            var titlebasic = new titlebasic
            {
                tconst = tconstTextBox.Text,
                titleType = titleTypeTextBox.Text,
                primaryTitle = primaryTitleTextBox.Text,
                originalTitle = originaltitleTextBox.Text,
                isAdult = Convert.ToBoolean(isAdultComboBox.Text),
                startYear = tempStartYear,
                endYear = tempEndYear,
                runtimeMinutes = tempRunTimeMinutes,
                genres = genreTextBox.Text
            };

            HttpResponseMessage response = await client.PutAsXmlAsync(url, titlebasic);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Titlebasic updated");
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        //deletes selected employee from database
        private async void DeleteTitleBasic(string id)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://localhostED:2105/")
            };
            var url = "api/titlebasics/" + id;
            HttpResponseMessage response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("User Deleted");
                BindTitlebasicsList();
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void BindTitlebasicsList()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:2105/")
            };
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/xml"));
        }
    }
}