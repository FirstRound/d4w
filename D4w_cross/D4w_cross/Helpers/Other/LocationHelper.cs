using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D4w_cross.Helpers
{
    class LocationHelper
    {

        private const double DEFAULT_LAT = 55.77580f;
        private const double DEFAULT_LNG = 49.12201f;

        public static async Task<Position> WaitLoc()
        {
            var locator = CrossGeolocator.Current;
            Position res = null;
            try
            {
                res = await locator.GetPositionAsync(System.TimeSpan.FromSeconds(2));
            }catch(Exception ex)
            {
                
            }
            var ret = new Position();
            if (res == null)
            {
                ret.Latitude = DEFAULT_LAT;
                ret.Longitude = DEFAULT_LNG;
            }
            else
            {
                ret.Latitude = res.Latitude;
                ret.Longitude = res.Longitude;
            }
            return ret;    
        }

        public static Position GetLocation()
        {
            var task = Task.Run<Position>(async () => await WaitLoc());
           
            return task.Result;
        }
    }
}
