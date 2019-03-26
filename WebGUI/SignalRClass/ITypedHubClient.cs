using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGUI.SignalRClass
{
    public interface ITypedHubClient
    {
        Task SendAsync(string topic, string name, string message);
    }
}
