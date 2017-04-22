using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.MVVMMessages
{
    class PageChangeFinishedMessage : MessageBase
    {
        public int FrameIndex
        {
            get
            {
                return (int)base.FirstObject;
            }
            set
            {
                base.FirstObject = value;
            }
        }
    }
}
