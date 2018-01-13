using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using D4w_cross.Droid.Dependencies;
using D4w_cross.Dependencies;
using Com.Braintreepayments.Api.Dropin;
using Xamarin.Forms;
using System.Threading.Tasks;
using D4w_cross.Helpers.API;
using D4w_cross.Models.API;

[assembly: Xamarin.Forms.Dependency(typeof(PaymentDropInImplementation))]
namespace D4w_cross.Droid.Dependencies
{
    class PaymentDropInImplementation : IPaymentDropIn
    {
        public PaymentDropInImplementation() { }

        public void ShowInit(string clientToken)
        {
            var activity = (MainActivity)Forms.Context;
            var listener = new ActivityResultListener(activity, null);
            var request = new DropInRequest().ClientToken(clientToken);
            activity.StartActivityForResult(request.GetIntent(Forms.Context), 1);
        }

        public Task<bool> ShowPay(string clientToken, int bookingId)
        {
            var activity = (MainActivity)Forms.Context;
            var listener = new ActivityResultListener(activity, bookingId);
            var request = new DropInRequest().ClientToken(clientToken);
            activity.StartActivityForResult(request.GetIntent(Forms.Context), 1);
            return listener.Task;
        }

        private class ActivityResultListener
        {

            private int? bookingId = null;

            private TaskCompletionSource<bool> Complete = new TaskCompletionSource<bool>();
            public Task<bool> Task { get { return this.Complete.Task; } }

            public ActivityResultListener(MainActivity activity, int? bookingId)
            {
                activity.ActivityResult += OnActivityResult;
                this.bookingId = bookingId;
            }

            private void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                var context = Forms.Context;
                var activity = (MainActivity)context;
                activity.ActivityResult -= OnActivityResult;

                // process result
                if (resultCode != Result.Ok)
                {
                    Complete.TrySetResult(true);
                }
                   // this.Complete.TrySetResult(false);
                else
                {
                    var result = data.GetParcelableExtra(DropInResult.ExtraDropInResult) as DropInResult;
                    if (bookingId != null)
                    {
                        var res = new PaymentsHelper().PayForBooking(result.PaymentMethodNonce.Nonce, bookingId.Value);
                        if (res.Status == System.Net.HttpStatusCode.OK)
                        {
                            Complete.TrySetResult(true);
                        }
                        else
                        {
                            Complete.TrySetResult(false);
                        }
                    }
                    else
                    {
                        new PaymentsHelper().InitPayments(result.PaymentMethodNonce.Nonce);
                    }

                    // this.Complete.TrySetResult(true);
                }
            }
        }
    }
}