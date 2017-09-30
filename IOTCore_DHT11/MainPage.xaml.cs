using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace IOTCore_DHT11
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GpioPin dhtPin = null;
        private IDht dht = null;
        private DhtReading dhtReading = new DhtReading();

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;



        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Setup();

            Loop();
        }

        private void Setup()
        {
            GpioController gpio = GpioController.GetDefault();

            if(gpio == null)
            {
                dhtPin = null;
                return;
            }

            dhtPin = gpio.OpenPin(5);
            dht = new Dht11(dhtPin, GpioPinDriveMode.Input);

        }

        private async void Loop()
        {
            while (true)
            {
                dhtReading = await dht.GetReadingAsync().AsTask();
                dataBlock.Text = dhtReading.Humidity.ToString() + " : " + dhtReading.Temperature.ToString();
                //await Task.Delay(1000);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
