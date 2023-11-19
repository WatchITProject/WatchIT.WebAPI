using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchIT.WebAPI.Services.Common
{
    public class Check<T>
    {
        public Predicate<T> CheckAction { get; set; }
        public string Message { get; set; }
    }
}
