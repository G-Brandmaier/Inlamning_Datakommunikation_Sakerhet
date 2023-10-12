using Client_Console.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Client_Console.Services;

/// <summary>
/// Service class for managing connection, timer intervals, sending and encrypting data to the SignalR hub application.
/// </summary>
public class HubConnectionService
{
    public HubConnection _hubConnection;
    public bool IsConnected = false;
    private readonly string _url = "https://localhost:7235/weatherhub";
    private readonly System.Timers.Timer _timer;

    public HubConnectionService()
    {
        _hubConnection = new HubConnectionBuilder().WithUrl(_url).Build();
        _timer = new System.Timers.Timer();
    }

    /// <summary>
    /// Starts the setups for connection to the SignalR hub application.
    /// </summary>

    public async Task Start()
    {
        //Configure timer settings
        _timer.Interval = 5000;
        _timer.Elapsed += OnTimedEvent!;
        _timer.AutoReset = true;

        /// <summary>
        /// Starts the connection and when connection is established it will send data to the hub and then start the timer 
        /// as long as the connection is true.
        /// </summary>
        try
        {
            await StartConnection();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _hubConnection.DisposeAsync();
        }

        if (IsConnected)
        {
            await SendData();

            while (IsConnected)
            {
                _timer.Start();
            }
        }
        Console.ReadKey();
    }

    /// <summary>
    /// Events that will be executed when the timer is triggered
    /// </summary>
    public async void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            await SendData();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Starts connection the the SignalR hub application.
    /// </summary>
    public async Task StartConnection()
    {
        try
        {
            await _hubConnection.StartAsync();
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                IsConnected = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Sends data to the SignalR hub application.
    /// Generates a WeatherData object and serializes it to a json string, the string is then sent to the Encrpt 
    /// method which will encrypt the information. The return value will be an object which then also gets serialized into a jsonstring 
    /// and then sent to the hub.
    /// </summary>
    public async Task SendData()
    {
        var weatherData = new WeatherData();
        var weatherDataJsonString = JsonSerializer.Serialize(weatherData);
        var encryptedDataJsonString = JsonSerializer.Serialize(Encrypt(weatherDataJsonString));

        await _hubConnection.InvokeAsync("SendWeatherData", encryptedDataJsonString);

    }

    /// <summary>
    /// Encrypting method.
    /// An instance of EncryptedData will be made which have the parameters byte[] data and byte[] iv.
    /// The received string will be encrypted with a key and a generated iv. 
    /// The parameters of the EncryptedData object will be set with the encrypted string and the generated iv.
    /// Returns the EncryptedData object.
    /// </summary>
    public EncryptedData Encrypt(string data)
    {
        EncryptedData encryptedData = new EncryptedData();
        byte[] encrypted;

        using (Aes aes = Aes.Create())
        {
            byte[] key =
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
            };

            aes.Key = key;
            aes.GenerateIV();
            byte[] iv = aes.IV;
            encryptedData.Iv = iv;
            ICryptoTransform cryptoTransform = aes.CreateEncryptor(key, iv);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(Encoding.UTF8.GetBytes(data));
                }
                encrypted = memoryStream.ToArray();
            }
        }
        encryptedData.Data = encrypted;
        return encryptedData;
    }
}
