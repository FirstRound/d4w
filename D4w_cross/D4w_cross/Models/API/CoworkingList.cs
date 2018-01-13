using System;
using System.Collections.Generic;
using System.Text;

namespace D4w_cross.Models.API
{
    class CoworkingList : APIResponse
    {
        public List <Coworking> Coworkings { get; set; }
    }
}
