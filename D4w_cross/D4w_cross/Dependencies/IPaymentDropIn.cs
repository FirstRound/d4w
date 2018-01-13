using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D4w_cross.Dependencies
{
    public interface IPaymentDropIn
    {
        void ShowInit(string client_token);
        Task<bool> ShowPay(string client_token, int bookingId);

    }
}
