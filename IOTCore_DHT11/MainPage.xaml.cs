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
        
        private GpioPin dhtPin;
        private Dht11 dht;

        //private DispatcherTimer timer = new DispatcherTimer();


        //private const byte ledPin = 5;
        //private GpioPin pin;

        //private UInt16 duty = 1000;

        public MainPage()
        {
            this.InitializeComponent();

            Setup();
            /*
            Loaded += MainPage_Loaded;
            timer.Tick += Timer_Tick;
            */
            Loop();
        }

        /// <summary>
        /// 타이머를 사용하고 싶었지만 실패했다.
        /// </summary>

        /*
        private async void Timer_Tick(object sender, object e)
        {
            dhtReading = await dht.GetReadingAsync().AsTask();
            //dataBlock.Text = dhtReading.Humidity.ToString() + " : " + dhtReading.Temperature.ToString();
            await Task.Delay(1000);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Loop();
        }
        */
        private void Setup()
        {
            //// 기존의 구문 ////

            GpioController gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                dhtPin = null;
                return;
            }

            dhtPin = gpio.OpenPin(5);
            dhtPin.SetDriveMode(GpioPinDriveMode.Input);
            
            //// 이 부분도 이상함 ////
            
            //dht = new Dht11(dhtPin, GpioPinDriveMode.Input);

            //// 프로젝트가 이상한지 테스트하기 위해 작성한 구문 ////

            /* 정상동작 */

            //var gpio = GpioController.GetDefault();

            //if (gpio == null)
            //{
            //    pin = null;
            //    return;
            //}

            //pin = gpio.OpenPin(ledPin);
            //pin.SetDriveMode(GpioPinDriveMode.Output);
            //pin.Write(GpioPinValue.High);

            /////////////////////////////////////////////////////////
        }

        private async void Loop()
        {
            //// 이 생성자를 생성하는 순간 에러가 났다 ////

            //DhtReading dhtReading;

            //// 기존의 구문 ////

            //while (true)
            //{
            //    dhtReading = await dht.GetReadingAsync().AsTask();

            //    double humidity = dhtReading.Humidity;
            //    double temperature = dhtReading.Temperature;

            //    dataBlock.Text = humidity.ToString() + " : " + temperature.ToString();

            //    await Task.Delay(1000);
            //}

            //// 프로젝트가 이상한지 테스트하기 위해 작성한 구문 ////

            /* 정상동작 */

            //while (true)
            //{
            //    pin.Write(GpioPinValue.Low);
            //    await Task.Delay(duty);
            //    pin.Write(GpioPinValue.High);
            //    await Task.Delay(duty);
            //}
            /////////////////////////////////////////////////////////
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
//// 인터넷에서 찾은 예제코드 ////
/*
 using System;
using System.Collections.Generic;
using System.Linq;
using Sensors.Dht;
using Sensors.OneWire.Common;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Sensors.OneWire
{
	public sealed partial class MainPage : BindablePage
    {
        private DispatcherTimer _timer = new DispatcherTimer();

        GpioPin _pin = null;
        private IDht _dht = null;
        private List<int> _retryCount = new List<int>();
        private DateTimeOffset _startedAt = DateTimeOffset.MinValue;

        public MainPage()
        {
            this.InitializeComponent();

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

			GpioController controller = GpioController.GetDefault();

			if (controller != null)
			{
				_pin = GpioController.GetDefault().OpenPin(17, GpioSharingMode.Exclusive);
				_dht = new Dht22(_pin, GpioPinDriveMode.Input);
				_timer.Start();
				_startedAt = DateTimeOffset.Now;

				// ***
				// *** Uncomment to simulate heavy CPU usage
				// ***
				//CpuKiller.StartEmulation();
			}
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _timer.Stop();

			// ***
			// *** Dispose the pin.
			// ***
			if (_pin != null)
			{
				_pin.Dispose();
				_pin = null;
			}

			// ***
			// *** Set the Dht object reference to null.
			// ***
			_dht = null;

			// ***
			// *** Stop the high CPU usage simulation.
			// ***
			CpuKiller.StopEmulation();

			base.OnNavigatedFrom(e);
        }

        private async void _timer_Tick(object sender, object e)
        {
            DhtReading reading = new DhtReading();
            int val = this.TotalAttempts;
            this.TotalAttempts++;

            reading = await _dht.GetReadingAsync().AsTask();

            _retryCount.Add(reading.RetryCount);
            this.OnPropertyChanged(nameof(AverageRetriesDisplay));
            this.OnPropertyChanged(nameof(TotalAttempts));
            this.OnPropertyChanged(nameof(PercentSuccess));

			if (reading.IsValid)
			{
				this.TotalSuccess++;
				this.Temperature = Convert.ToSingle(reading.Temperature);
				this.Humidity = Convert.ToSingle(reading.Humidity);
				this.LastUpdated = DateTimeOffset.Now;
				this.OnPropertyChanged(nameof(SuccessRate));
			}

            this.OnPropertyChanged(nameof(LastUpdatedDisplay));
        }

        public string PercentSuccess
        {
            get
            {
                string returnValue = string.Empty;

                int attempts = this.TotalAttempts;

                if (attempts > 0)
                {
                    returnValue = string.Format("{0:0.0}%", 100f * (float)this.TotalSuccess / (float)attempts);
                }
                else
                {
                    returnValue = "0.0%";
                }

                return returnValue;
            }
        }

        private int _totalAttempts = 0;
        public int TotalAttempts
        {
            get
            {
                return _totalAttempts;
            }
            set
            {
                this.SetProperty(ref _totalAttempts, value);
                this.OnPropertyChanged(nameof(PercentSuccess));
            }
        }

        private int _totalSuccess = 0;
        public int TotalSuccess
        {
            get
            {
                return _totalSuccess;
            }
            set
            {
                this.SetProperty(ref _totalSuccess, value);
                this.OnPropertyChanged(nameof(PercentSuccess));
            }
        }

        private float _humidity = 0f;
        public float Humidity
        {
            get
            {
                return _humidity;
            }

            set
            {
                this.SetProperty(ref _humidity, value);
                this.OnPropertyChanged(nameof(HumidityDisplay));
            }
        }

        public string HumidityDisplay
        {
            get
            {
                return string.Format("{0:0.0}% RH", this.Humidity);
            }
        }

        private float _temperature = 0f;
        public float Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                this.SetProperty(ref _temperature, value);
                this.OnPropertyChanged(nameof(TemperatureDisplay));
            }
        }

        public string TemperatureDisplay
        {
            get
            {
                return string.Format("{0:0.0} °C", this.Temperature);
            }
        }

        private DateTimeOffset _lastUpdated = DateTimeOffset.MinValue;
        public DateTimeOffset LastUpdated
        {
            get
            {
                return _lastUpdated;
            }
            set
            {
                this.SetProperty(ref _lastUpdated, value);
                this.OnPropertyChanged(nameof(LastUpdatedDisplay));
            }
        }

        public string LastUpdatedDisplay
        {
            get
            {
                string returnValue = string.Empty;

                TimeSpan elapsed = DateTimeOffset.Now.Subtract(this.LastUpdated);

                if (this.LastUpdated == DateTimeOffset.MinValue)
                {
                    returnValue = "never";
                }
                else if (elapsed.TotalSeconds < 60d)
                {
                    int seconds = (int)elapsed.TotalSeconds;

                    if (seconds < 2)
                    {
                        returnValue = "just now";
                    }
                    else
                    {
                        returnValue = string.Format("{0:0} {1} ago", seconds, seconds == 1 ? "second" : "seconds");
                    }
                }
                else if (elapsed.TotalMinutes < 60d)
                {
                    int minutes = (int)elapsed.TotalMinutes == 0 ? 1 : (int)elapsed.TotalMinutes;
                    returnValue = string.Format("{0:0} {1} ago", minutes, minutes == 1 ? "minute" : "minutes");
                }
                else if (elapsed.TotalHours < 24d)
                {
                    int hours = (int)elapsed.TotalHours == 0 ? 1 : (int)elapsed.TotalHours;
                    returnValue = string.Format("{0:0} {1} ago", hours, hours == 1 ? "hour" : "hours");
                }
                else
                {
                    returnValue = "a long time ago";
                }

                return returnValue;
            }
        }

        public int AverageRetries
        {
            get
            {
                int returnValue = 0;

                if (_retryCount.Count() > 0)
                {
                    returnValue = (int)_retryCount.Average();
                }

                return returnValue;
            }
        }

        public string AverageRetriesDisplay
        {
            get
            {
                return string.Format("{0:0}", this.AverageRetries);
            }
        }

        public string SuccessRate
        {
            get
            {
                string returnValue = string.Empty;

                double totalSeconds = DateTimeOffset.Now.Subtract(_startedAt).TotalSeconds;
                double rate = this.TotalSuccess / totalSeconds;

                if (rate < 1)
                {
                    returnValue = string.Format("{0:0.00} seconds/reading", 1d / rate);
                }
                else
                {
                    returnValue = string.Format("{0:0.00} readings/sec", rate);
                }

                return returnValue;
            }
        }
    }
}
*/
