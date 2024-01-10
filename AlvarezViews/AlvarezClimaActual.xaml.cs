using Alvarez_ApiPublica.AlvarezModelo;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Alvarez_ApiPublica.AlvarezViews;

public partial class AlvarezClimaActual : ContentPage
{
	public AlvarezClimaActual()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		string AlvarezLatitud = AlvarezLat.Text;
        string AlvarezLongitud = AlvarezLon.Text;

		if(Connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			using (var client = new HttpClient())
			{
				string url = $"https://api.openweathermap.org/data/2.5/weather?lat=" + AlvarezLatitud + "&lon=" + AlvarezLongitud + "&appid=c4768f4a366be89b9561a91784c546ce";
				var response = await client.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					var clima = JsonConvert.DeserializeObject<Rootobject>(json);

                    AlvarezWeatherLabel.Text = clima.weather[0].main;
					AlvarezCityLabel.Text = clima.name;
					AlvarezPais.Text = clima.sys.country;


                    double tempMaxFahrenheit = clima.main.temp_max;
                    double tempMaxCelsius = AlvarezConvertKelvinToCelsius(tempMaxFahrenheit);

                    AlvarezTempMaxima.Text = $"{tempMaxCelsius:F2} °C";

                    double tempMinFahrenheit = clima.main.temp_min;
                    double tempMinCelsius = AlvarezConvertKelvinToCelsius(tempMinFahrenheit);

                    AlvarezTempMinima.Text = $"{tempMinCelsius:F2} °C";
                }
			}
		}
    }

    public double AlvarezConvertKelvinToCelsius(double kelvin)
    {
        return kelvin - 273.15;
    }
}