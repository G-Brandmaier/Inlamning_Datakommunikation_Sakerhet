using Api_Hub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;
using System.Text.Json;

namespace Api_Hub.Hubs;

/// <summary>
/// SignalR hub for receiving and broadcast data to users.
/// </summary>
public class WeatherHub : Hub
{
    /// <summary>
    /// Different methods for sending data to subscribed clients.
    /// OnConnectAsync: When connection is made this message will be sent to all clients.
    /// SenMessage: Send messages to all clients that subscribed to ReceiveMessage.
    /// SendWeatherData: Sends weather data to all clients subscribed to ReceiveData, expects to get string to decrypt data from.
    /// SendAdminMessage: Sends messages to all clients subscribed to ReceivMessage but can only be invoked by a logged in user with admin role.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        await SendMessage("Connected");
    }
    public async Task SendMessage(string message)
    {
        if(!string.IsNullOrEmpty(message))
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
    public async Task SendWeatherData(string data)
    {
        if(!string.IsNullOrEmpty(data))
        {
            string decryptedData = Decrypt(data);
            await Clients.All.SendAsync("ReceiveWeatherData", decryptedData);
        }
    }
    [Authorize(Policy = "RequireAdminRole")]
    public async Task SendAdminMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
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


