using D4w_cross.Models.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Models.CustomEventArgs
{
    class CheckBoxEventArgs : EventArgs
    {
        public CheckBoxImage CheckBox { get; set; }
    }
}
