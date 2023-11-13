using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WatchIT.WebAPI.Helpers
{
    public static class StringHelpers
    {
        public static string CreateRandom(int length) => CreateRandom(length, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890`~!@#$%^&*()-_=+[{]};:'\"\\|,<.>/?");
        public static string CreateRandom(int length, IEnumerable<char> characters) => new string(Enumerable.Repeat(characters, length).Select(s => s.ElementAt(Random.Shared.Next(s.Count()))).ToArray());
    }
}
