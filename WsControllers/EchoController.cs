using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebSocketPlayground.Router.WebSocketRouter;

namespace WebSocketPlayground.WsControllers
{
    [WebSocketController(Path = "echo")]
    public class EchoController
    {
        private readonly ILogger<EchoController> _logger;
        public EchoController(ILogger<EchoController> logger)
        {
            _logger = logger;
        }

        [SubPath(Path = "repeater")]
        public async Task RepeaterHandler(HttpContext context, WebSocket socket)
        {
            var recv = await WebSocketMessage.GetMessageAsync(socket);
            
            while (!recv.Item3.CloseStatus.HasValue)
             {
                (string message, _, WebSocketReceiveResult result)  = recv;
                _logger.LogInformation($"Receive message: {message}");
                
                var response = Encoding.UTF8.GetBytes($"{message} -- OK!");
                await socket.SendAsync(new System.ArraySegment<byte>(response), result.MessageType, result.EndOfMessage, CancellationToken.None);

                recv = await WebSocketMessage.GetMessageAsync(socket);
            }
            await socket.CloseAsync(recv.Item3.CloseStatus.Value, recv.Item3.CloseStatusDescription, CancellationToken.None);
        }

        [SubPath(Path = "hello")]
        public async Task SayHello(HttpContext context, WebSocket socket)
        {
            var recv = await WebSocketMessage.GetMessageAsync(socket);
            
            while (!recv.Item3.CloseStatus.HasValue)
             {
                (string message, _, WebSocketReceiveResult result)  = recv;
                _logger.LogInformation($"Receive message: {message}");
                
                var response = Encoding.UTF8.GetBytes($"Hello! {message}");
                await socket.SendAsync(new System.ArraySegment<byte>(response), result.MessageType, result.EndOfMessage, CancellationToken.None);
                
                recv = await WebSocketMessage.GetMessageAsync(socket);
            }
            await socket.CloseAsync(recv.Item3.CloseStatus.Value, recv.Item3.CloseStatusDescription, CancellationToken.None);
        }
    }
}