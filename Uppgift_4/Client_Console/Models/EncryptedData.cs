namespace Client_Console.Models;

/// <summary>
/// Class for sending encrypted data to the SignalR hub.
/// The encrypted value is set as the Data and the generated iv is set as the Iv.
/// </summary>
public class EncryptedData
{
    public byte[] Data { get; set; } = null!;
    public byte[] Iv { get; set; } = null!;
}
