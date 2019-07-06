using System;
using System.Collections.Generic;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;
using IMDbDotNetAPI.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<titlebasic> ObserveTitleBasicList { get; set; }
        public static List<titlebasic> titleBasicList;
        private string titleBasicID = null;
        private int pageSize = 12;
        private int pageIndex = 12;
        public int ienumerableCount;
        public List<titlebasic> asList;
        public List<titlebasic> pageList = new List<titlebasic>();
        static HttpClient Client = new HttpClient();

        public MainWindow()
        {
            Run();
        }

        public void Run()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Client.BaseAddress = new Uri("http://localhost:2105/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            EditTitleBasicForm edit = new EditTitleBasicForm();
            edit.Show();
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (titleBasicID != null)
            {
                EditTitleBasicForm edit = new EditTitleBasicForm();
                edit.GetTitleBasicByID(titleBasicID);
                edit.Show();
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This button is deprecate. Feature moved to Edit form.");
        }

        private async void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            await Search();
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var id = titleBasicID;
            var url = "api/titlebasics/" + id;

            HttpResponseMessage response = await Client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("You have deleted " + titleBasicID);
                await Search();
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private async void BtnSearch_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Search();
        }

        private async Task Search()
        {
            var id = m_txtTest.Text.Trim();
            var url = "api/titlebasics/" + id + "?startindex=" + pageIndex + "&pagesize=" + pageSize;
            HttpResponseMessage response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var titlebasics = await response.Content.ReadAsAsync<IEnumerable<titlebasic>>();
                 ienumerableCount = titlebasics.Count();
            //    GetCount(id);
                TotalCount.Content = ienumerableCount;
                asList = titlebasics.ToList();
                ObserveTitleBasicList = new ObservableCollection<titlebasic>(asList);
                DataContext = ObserveTitleBasicList;
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        //private async void GetCount(string id)
        //{
        //    var url = "api/titlebasics/" + id;
        //    HttpResponseMessage response = await Client.GetAsync(url);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var titlebasics = await response.Content.ReadAsAsync<IEnumerable<titlebasic>>();
        //        ienumerableCount = titlebasics.Count();
        //        TotalCount.Content = ienumerableCount;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
        //    }
        //}

        private void ListView1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (titlebasic item in e.AddedItems)
            {
                titleBasicID = item.tconst;
            }
        }

        private void SearchBar_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (m_txtTest.Text == "")
            {
                m_txtTest.Text = "Search";
            }
        }

        private void SearchBar_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (m_txtTest.Text.Equals("Search"))
            {
                m_txtTest.Text = "";
            }
        }

        private async void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (pageIndex >= pageSize)
            {
                pageIndex -= pageSize;
            }
            await Search();
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            pageIndex += pageSize;
            DataContext = null;
            await Search();
        }

        private async void M_txtTest_MouseDoubleClickAsync(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await Search();
        }

        private async void M_txtTest_KeyDownAsync(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                await Search();
            }
        }
    }
}
