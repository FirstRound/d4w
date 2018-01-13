using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Helpers
{
    public interface IPermissionRequester
    {
        bool IsGPSEnabled();
        bool AskForGPS();
    }
}
