using GraphApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Костыль, я не знаю пока, как сделать по другому.
namespace GraphApp.Services
{
    public class MessageBuffer : IMessageBuffer
    {
        public string Message { get; set; }
    }
}
