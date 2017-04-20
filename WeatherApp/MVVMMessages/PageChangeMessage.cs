using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WeatherApp.MVVMMessages
{
    public class PageChangeMessage : MessageBase
    {
        public Page PageToChange
        {
            get
            {
                return (Page)base.FirstObject;
            }
            set
            {
                base.FirstObject = value;
            }
        }
    }
}
