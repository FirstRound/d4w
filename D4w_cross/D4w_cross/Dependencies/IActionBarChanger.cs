using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Dependencies
{
    public interface IActionBarChanger
    { 
        void InitClick(EventHandler func);    
        void ChangeTitle(string title);
    }
}
