using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private InputField input;

    RainController weatherRain;

    private string city;

    private void Start()
    {
        weatherRain = GetComponent<RainController>();
    }

    public void Change()
    {
        City();

        StartCoroutine(GetRequest("https://goweather.herokuapp.com/weather/" + city.Replace(" " , "_")));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        yield return webRequest.SendWebRequest();

        string resultado = webRequest.downloadHandler.text;

        Debug.Log(resultado);

        WeatherInfo temperature = JsonUtility.FromJson<WeatherInfo>(resultado);

        Debug.Log(temperature.temperature);

        ChangeValues(temperature);

        ChangeScenes();
    }

    private void ChangeValues(WeatherInfo weather)
    {
        weatherRain.masterIntensity = 1f;

        if (weather.description.Contains("rain"))
        {
            weatherRain.rainIntensity = 1f;
        }

        switch (weather.description)
        {
            case "Sunny":
                weatherRain.lightningIntensity = 1f;
                break;
            case "Partly cloudy":
                weatherRain.fogIntensity = 0.5f;
                break;
            case "Clear":
                break;
            default:
                break;
        }
    }

    private void ChangeScenes()
    {
        SceneManager.LoadScene(1);
    }

    private void City()
    {
        city = input.text;

        Debug.Log(city);
    }
}

class Forecast
{
    public int day;
    public string temperature;
    public string wind;
}

class WeatherInfo
{
    public string temperature;
    public string wind;
    public string description;
    public IList<Forecast> forecast;
}
