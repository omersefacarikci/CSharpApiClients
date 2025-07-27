using Newtonsoft.Json.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        private const string apiKey = "Bu projeyi çalýþtýrmak için kendi OpenWeatherMap API anahtarýnýzý Form1.cs içindeki `apiKey` sabitine giriniz.\r\n";


        public Form1()
        {
            InitializeComponent();

        }

        private void guna2CircleButton5_MouseClick(object sender, MouseEventArgs e)
        {
            guna2CircleButton5.FillColor = Color.FromArgb(119, 152, 171);
            guna2CircleButton4.FillColor = Color.FromArgb(30, 30, 30);

        }

        private void guna2CircleButton4_MouseClick(object sender, MouseEventArgs e)
        {
            guna2CircleButton5.FillColor = Color.FromArgb(30, 30, 30);
            guna2CircleButton4.FillColor = Color.FromArgb(119, 152, 171);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            label1.Text = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("tr-TR"));
            guna2ComboBox1.Items.AddRange(new string[] {
                "Adana", "Adýyaman", "Afyonkarahisar", "Aksaray", "Aðrý", "Amasya", "Ankara", "Antalya", "Ardahan", "Artvin",
                "Aydýn", "Balýkesir", "Bartýn", "Batman", "Bayburt", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa",
                "Çanakkale", "Çankýrý", "Çorum", "Denizli", "Diyarbakýr", "Düzce", "Edirne", "Elazýð", "Erzincan", "Erzurum",
                "Eskiþehir", "Gaziantep", "Giresun", "Gümüþhane", "Hakkâri", "Hatay", "Iðdýr", "Isparta", "Ýstanbul", "Ýzmir",
                "Kahramanmaraþ", "Karabük", "Karaman", "Kars", "Kastamonu", "Kayseri", "Kilis", "Kocaeli", "Konya", "Kütahya",
                "Kýrklareli", "Kýrýkkale", "Kýrþehir", "Malatya", "Manisa", "Mardin", "Mersin", "Muðla", "Muþ", "Nevþehir", "Niðde",
                "Ordu", "Osmaniye", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Þanlýurfa", "Þýrnak", "Tekirdað",
                "Tokat", "Trabzon", "Tunceli", "Uþak", "Van", "Yalova", "Yozgat", "Zonguldak"
            });
            guna2ComboBox1.DropDownHeight = 350;
            guna2ComboBox1.SelectedIndex = 0;
            await GetWeatherDataAsync("Adana");
        }

        private async Task GetWeatherDataAsync(string cityName)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string currentUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric&lang=tr";
                    string currentResponse = await client.GetStringAsync(currentUrl);
                    JObject currentJson = JObject.Parse(currentResponse);

                    string desc = (string)currentJson["weather"]?[0]?["description"] ?? "-";
                    string icon = (string)currentJson["weather"]?[0]?["icon"] ?? "01d";
                    double temp = (double)currentJson["main"]["temp"];
                    double wind = (double)currentJson["wind"]["speed"];
                    int humidity = (int)currentJson["main"]["humidity"];
                    //1.gün
                    label1.Text = GetTurkishDayName(DateTime.Now.DayOfWeek);
                    label11.Text = $"{Math.Round(temp)}°C";
                    label12.Text = desc;
                    label13.Text = $"Rüzgar: {wind} m/s";
                    label14.Text = $"Nem: %{humidity}";
                    pictureBox1.Load($"https://openweathermap.org/img/wn/{icon}@2x.png");
                    string forecastUrl = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid={apiKey}&units=metric&lang=tr";
                    string forecastResponse = await client.GetStringAsync(forecastUrl);
                    JObject forecastJson = JObject.Parse(forecastResponse);
                    JArray list = (JArray)forecastJson["list"];

                    List<(DateTime date, double temp, string description, string icon, double windSpeed, int humidity)> forecastData = new();

                    foreach (var item in list)
                    {
                        DateTime dt = DateTime.Parse(item["dt_txt"].ToString());

                        if (dt.Hour == 12 && forecastData.Count < 4)
                        {
                            double fTemp = (double)item["main"]["temp"];
                            string fDesc = (string)item["weather"]?[0]?["description"] ?? "-";
                            string fIcon = (string)item["weather"]?[0]?["icon"] ?? "01d";
                            double fWind = (double)item["wind"]["speed"];
                            int fHumidity = (int)item["main"]["humidity"];

                            forecastData.Add((dt, fTemp, fDesc, fIcon, fWind, fHumidity));
                        }
                    }

                    if (forecastData.Count == 4)

                    {
                        //2.gün
                        label4.Text = GetTurkishDayName(forecastData[0].date.DayOfWeek);
                        label16.Text = $"{Math.Round(forecastData[0].temp)}°C";
                        label15.Text = forecastData[0].description;
                        label9.Text = $"Rüzgar: {forecastData[0].windSpeed} m/s";
                        label8.Text = $"Nem: %{forecastData[0].humidity}";
                        pictureBox2.Load($"https://openweathermap.org/img/wn/{forecastData[0].icon}@2x.png");
                        //3gün
                        label5.Text = GetTurkishDayName(forecastData[1].date.DayOfWeek);
                        label20.Text = $"{Math.Round(forecastData[1].temp)}°C";
                        label19.Text = forecastData[1].description;
                        label18.Text = $"Rüzgar: {forecastData[1].windSpeed} m/s";
                        label17.Text = $"Nem: %{forecastData[1].humidity}";
                        pictureBox3.Load($"https://openweathermap.org/img/wn/{forecastData[1].icon}@2x.png");
                        //4.gün
                        label6.Text = GetTurkishDayName(forecastData[2].date.DayOfWeek);
                        label24.Text = $"{Math.Round(forecastData[2].temp)}°C";
                        label23.Text = forecastData[2].description;
                        label22.Text = $"Rüzgar: {forecastData[2].windSpeed} m/s";
                        label21.Text = $"Nem: %{forecastData[2].humidity}";
                        pictureBox4.Load($"https://openweathermap.org/img/wn/{forecastData[2].icon}@2x.png");
                        //5.gün
                        label29.Text = GetTurkishDayName(forecastData[3].date.DayOfWeek);
                        label28.Text = $"{Math.Round(forecastData[3].temp)}°C";
                        label27.Text = forecastData[3].description;
                        label26.Text = $"Rüzgar: {forecastData[3].windSpeed} m/s";
                        label25.Text = $"Nem: %{forecastData[3].humidity}";
                        pictureBox5.Load($"https://openweathermap.org/img/wn/{forecastData[3].icon}@2x.png");

                        string washingtonUrl = $"https://api.openweathermap.org/data/2.5/weather?q=Washington,US&appid={apiKey}&units=metric&lang=tr";
                        string washingtonResponse = await client.GetStringAsync(washingtonUrl);
                        JObject washingtonJson = JObject.Parse(washingtonResponse);
                        double wTemp = (double)washingtonJson["main"]["temp"];
                        string wIcon = (string)washingtonJson["weather"]?[0]?["icon"] ?? "01d";

                        label33.Text = $"{Math.Round(wTemp)}°C";
                        pictureBox6.Load($"https://openweathermap.org/img/wn/{wIcon}@2x.png");
                        //--------------------------------\\
                        string beijingUrl = $"https://api.openweathermap.org/data/2.5/weather?q=Beijing,CN&appid={apiKey}&units=metric&lang=tr";
                        string beijingResponse = await client.GetStringAsync(beijingUrl);
                        JObject pekinJson = JObject.Parse(beijingResponse);
                        double wTempp = (double)pekinJson["main"]["temp"];
                        string wIconn = (string)pekinJson["weather"]?[0]?["icon"] ?? "01d";

                        label34.Text = $"{Math.Round(wTempp)}°C";
                        pictureBox7.Load($"https://openweathermap.org/img/wn/{wIconn}@2x.png");
                        //-----------------------------------\\
                        string moscowUrl = $"https://api.openweathermap.org/data/2.5/weather?q=Moscow,RU&appid={apiKey}&units=metric&lang=tr";
                        string moscowResponse = await client.GetStringAsync(moscowUrl);
                        JObject moscowJson = JObject.Parse(moscowResponse);
                        double wTemppp = (double)moscowJson["main"]["temp"];
                        string wIconnn = (string)moscowJson["weather"]?[0]?["icon"] ?? "01d";

                        label35.Text = $"{Math.Round(wTemppp)}°C";
                        pictureBox8.Load($"https://openweathermap.org/img/wn/{wIconnn}@2x.png");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hava durumu alýnamadý: " + ex.Message);
                }

            }
        }

        private string GetTurkishDayName(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return "Pazartesi";
                case DayOfWeek.Tuesday: return "Salý";
                case DayOfWeek.Wednesday: return "Çarþamba";
                case DayOfWeek.Thursday: return "Perþembe";
                case DayOfWeek.Friday: return "Cuma";
                case DayOfWeek.Saturday: return "Cumartesi";
                case DayOfWeek.Sunday: return "Pazar";
                default: return "";
            }
        }

        private void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(20, 20, 20);
            label2.ForeColor = Color.White;
            label10.ForeColor = Color.White;
            label39.ForeColor = Color.White;
        }

        private void guna2CircleButton5_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            label2.ForeColor = Color.Black;
            label10.ForeColor = Color.Black;
            label39.ForeColor = Color.Black;
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            string selectedCity = guna2ComboBox1.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedCity))
            {
                await GetWeatherDataAsync(selectedCity);
            }
        }

        private async void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCity = guna2ComboBox1.SelectedItem.ToString();
            label2.Text = selectedCity;
            await GetWeatherDataAsync(selectedCity);
        }

        private void label39_MouseEnter(object sender, EventArgs e)
        {
            label39.ForeColor = Color.FromArgb(119, 152, 171);
        }

        private void label39_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void label39_MouseLeave(object sender, EventArgs e)
        {
            label39.ForeColor = Color.White;

        }
    }
}
