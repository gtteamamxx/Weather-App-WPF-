using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.MVVMMessages
{
    public abstract class MessageBase
    {
        protected object FirstObject { get; set; }
        protected object SecondObject { get; set; }
    }
}
