using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Timers;

namespace App2
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        public int milliseconds;

        public int skore;

        public interface ICloseApplication
        {
            void closeApplication();
        }
        //časovač, který každou vteřinu volá metodu Game
        async void StartTimer()
        {
            await Task.Delay(1000);
            Game();
        }

        //aktualizace labelu cas
        private void Cas()
        {
            lbl_cas.Text = "Čas: " + milliseconds / 1000;
        }
        //obstarává aktualizaci skóre
        private void Skore()
        {
            lbl_skore.Text = "Skóre: " + skore;
        }
        //tato metoda nastaví správný čas a skóre a zapne časovač + tlačítko start uvede do režimu nepovoleno
        private void Start()
        {
            skore = 0;
            milliseconds = 10000;
            Skore();
            Cas();
            StartTimer();
            btn_Start.IsEnabled = false;
        }
        //metoda volaná po stisknutí tlačítka btn_Start
        private void btn_Start_Clicked(object sender, EventArgs e)
        {
            Start();
        }
        //hlavní herní metoda, stará se o rozmístění herního prvku, aktualizuje čas i skóre, pouští časovač a kontroluje, zda hráč má ještě dostatek času
        private void Game()
        {
            milliseconds = milliseconds - 1000;
            //zjišťuje, jestli není konec, kdyžtak vykreslí čas a umístí tlačítko jinam, spustí opět časovač
            if (milliseconds > 0)
            {
                Cas();
                Skore();
                var random = new Random();
                AbsoluteLayout.SetLayoutBounds(btn_kapka_dobra, new Rectangle(random.NextDouble(), random.NextDouble(), .09, .09));
                
                StartTimer();
            }

            else
            {
                Cas();
                Skore();
                Konec();
            }
        }

        //zde se obstarává přičtení času a skóre
        private void btn_kapka_dobra_Clicked(object sender, EventArgs e)
        {
            milliseconds += 2000;
            skore += 1;
            Skore();
            Cas();
        }

        //řeší konec hry

        async void Konec()
        {
            var answer = await DisplayAlert("Konec hry!", "Chcete si zahrát ještě jednou?", "Ano", "Ne");
            if (answer == true)
            {
                Start();
            }
            else
            {
                var closer = DependencyService.Get<ICloseApplication>();
                closer?.closeApplication();
            }
        }
        //tlačítko "About"
        private void btn_About_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("O aplikaci:", "Tato aplikace byla vytvořena Martinem Jodasem v rámci diplomové práce POROVNÁNÍ VYBRANÝCH VÝVOJOVÝCH PROSTŘEDÍ PRO OPERAČNÍ SYSTÉM ANDROID SE ZAMĚŘENÍM NA VÝUKU NA STŘEDNÍCH ŠKOLÁCH ", "OK");
        }
    }
}
