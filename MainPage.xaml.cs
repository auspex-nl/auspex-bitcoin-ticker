using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Auspex.Bitcoin.Ticker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer _timer;
        private PriceData _lastPrice;

        public MainPage()
        {
            this.InitializeComponent();

            // kick off loading the pricedata one time, then set up a timer to do it every minute
            LoadCurrentPrice();


            _timer = new DispatcherTimer();

            _timer.Tick += dispatcherTimer_Tick;
            _timer.Interval = new TimeSpan(0, 1, 0);

            _timer.Start();
        }

        private async void dispatcherTimer_Tick(object sender, object e)
        {
            await LoadCurrentPrice();
        }

        public async Task LoadCurrentPrice()
        {
            try
            {
                HttpClient client = new HttpClient();
                string jsonData = await client.GetStringAsync(new Uri("https://api.coindesk.com/v1/bpi/currentprice.json"));

                var price = JsonConvert.DeserializeObject<PriceData>(jsonData);

                if (_lastPrice != null)
                {
                    if (price.BPI.EUR.RateFloat > _lastPrice.BPI.EUR.RateFloat)
                    {
                        _textBlockArrow.Text = "▲";
                        _textBlockArrow.Foreground = new SolidColorBrush(Colors.DarkGreen);
                    }
                    else if (price.BPI.EUR.RateFloat < _lastPrice.BPI.EUR.RateFloat)
                    {
                        _textBlockArrow.Text = "▼";
                        _textBlockArrow.Foreground = new SolidColorBrush(Colors.DarkRed);
                    }
                    else
                    {
                        _textBlockArrow.Text = "◆";
                        _textBlockArrow.Foreground = new SolidColorBrush(Colors.White);
                    }
                }

                _textBlockPrice.Text = $"{Math.Round(price.BPI.EUR.RateFloat,2)}";

                _lastPrice = price;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to fetch price data: {ex.Message}");
            }
        }
    }
}
