using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Threading.Tasks;

namespace C971_Term_Sch
{
    public partial class App : Application
    {
        public static string FilePath;
        

        public App()
        {
            InitializeComponent();

            
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#002f51"),
                BarTextColor = Color.White,
            };
        }

        public App(string filePath)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#002f51"),
                BarTextColor = Color.White,
            };

            FilePath = filePath;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
