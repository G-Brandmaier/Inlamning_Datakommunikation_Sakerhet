using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace WebSocket_Server_Side.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebSocketsController : ControllerBase
{
    /// <summary>
    /// Setting up endpoint for websocket connection.
    /// When connection is made and accepted data is received and read.
    /// Then a message with the data received is sent back to the connected client.
    /// If connection is closed from client the socket connection will also close.
    /// </summary>
    /// <returns></returns>
    [HttpGet("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            var serverMessage = Encoding.UTF8.GetBytes($"Data received from client: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");

            await webSocket.SendAsync( new ArraySegment<byte>(serverMessage, 0, serverMessage.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

            buffer = new byte[1024 * 4];
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}
