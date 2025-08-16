using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greeter.Services
{
    public interface IGreetingService
    {
        Task<string> GreetAsync(string name);
        Task<string> GetGreetingAsync(string name, string language);
        Task<string> GetGreetingAsync(string name, string language, DateTimeOffset dateTime);
    }
}
