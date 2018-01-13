using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Dependencies
{
    public interface ICustomDIalogAlert
    {
        void Show(string title, string text, string okText);
        bool Show(string title, string text, string okText, string cancelText);

    }
}
