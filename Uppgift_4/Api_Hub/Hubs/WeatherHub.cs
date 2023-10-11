using Api_Hub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;
using System.Text.Json;

namespace Api_Hub.Hubs;

/// <summary>
/// Hub class for receiving data and broadcast it to the clients/subscribers.
/// SendData method is for receivig data from the Iot-client that provides weather data and broadcast it to all clients.
/// SendMessage method can only be called from those with an admin role from the web-application where you login and broadcasts to all clients.
/// Decrypt method is for decrypting the data from the Iot-client that sends the weather data. It receives a jsonstring object that contains the data and the iv then converts it in to an EncryptedDataDto object where we can select the needed data for the decryption. Lastly we send only the decrypted  weather data back to the SendData method which gets broadcasted.
/// </summary>
public class WeatherHub : Hub
{
    //When connection is made this message will be sent to all clients
    public override async Task OnConnectedAsync()
    {
        await SendHubMessage("Connected");
    }
    //Send messages for the hub to all clients that subscribes in ReceiveMessage
    public async Task SendHubMessage(string message)
    {
        if(!string.IsNullOrEmpty(message))
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
    //Sends weather data to all clients subscribed to ReceiveData, expects to get string to decrypt data from
    public async Task SendWeatherData(string data)
    {
        if(!string.IsNullOrEmpty(data))
        {
            string decryptedData = Decrypt(data);
            await Clients.All.SendAsync("ReceiveWeatherData", decryptedData);
        }
    }

    //Sends messages to all clients subscribed to ReceivMessage but can only be invoked by a logged in user with admin role.
    [Authorize(Policy = "RequireAdminRole")]
    public async Task SendMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }

    /// <summary>
    /// Decrypts data, expect to receive jsonstring to convert to EncryptedDataDto object with Data and Iv as its parameters.
    /// Decrypts with the key and provided iv.
    /// Returns the decrypted data as a string.
    /// </summary>

    public string Decrypt(string data)
    {
        var encryptedData = JsonSerializer.Deserialize<EncryptedDataDto>(data);
        var encryptedString = encryptedData!.Data;

        string decrypted = string.Empty;

        using (Aes aes = Aes.Create())
        {
            byte[] key =
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
            };

            aes.Key = key;
            ICryptoTransform cryptoTransform = aes.CreateDecryptor(key, encryptedData.Iv);

            using (MemoryStream memoryStream = new MemoryStream(encryptedString))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        decrypted = streamReader.ReadToEnd();
                    }
                }
            }

            return decrypted;
        }
    }
}


