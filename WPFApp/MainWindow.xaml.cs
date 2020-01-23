using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            // client.BaseAddress = new Uri("http://localhost:5000/api");
        }

        private async void listCars_Click(object sender, RoutedEventArgs e)
        {
            var response = await client.GetAsync("http://localhost:5000/api/Cars");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<IEnumerable<Car>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var carTable = new CarTable(cars);
            carTable.ShowDialog();
        }

        private async void available_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = datePicker1.SelectedDate.Value;
            Console.WriteLine(date.Millisecond);
            var response = await client.GetAsync("http://localhost:5000/api/Cars/free/" + date.Ticks);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<IEnumerable<Car>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var carTable = new CarTable(cars);
            carTable.ShowDialog();
        }

        private async void book_Click(object sender, RoutedEventArgs e)
        {
            BookedCars request = new BookedCars();
            request.Date = datePicker2.SelectedDate.Value;
            request.CarID = Int32.Parse(car.Text);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5000/api/BookedCars", content);
            Console.WriteLine(response);
        }
    }
}
