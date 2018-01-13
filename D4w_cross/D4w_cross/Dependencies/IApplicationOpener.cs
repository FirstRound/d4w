using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Helpers
{
    public interface IApplicationOpener
    {
        void OpenMapApp(double latTo, double lngTo);
        void OpenMapApp(string address);
    }
}
