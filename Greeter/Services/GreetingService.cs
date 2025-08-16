using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greeter.Services
{
    public class GreetingService : IGreetingService
    {
        public Task<string> GreetAsync(string name)
        {
            return Task.FromResult($"Hello, {name}!");
        }

        public Task<string> GetGreetingAsync(string name, string language)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetGreetingAsync(string name, string language, DateTimeOffset dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
