using System;
using System.Reflection;

namespace WebSocketPlayground.Router.WebSocketRouter
{
    public class WsControllerInfo
    {
        public string FullPath { get; set; }
        public Type ControllerType { get; set; }
        public MethodInfo HandleRequestMethod { get; set; }
    }
}