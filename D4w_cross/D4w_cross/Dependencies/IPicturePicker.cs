using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace D4w_cross.Helpers
{
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync();
    }

}
