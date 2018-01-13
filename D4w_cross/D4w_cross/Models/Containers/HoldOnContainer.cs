using D4w_cross.Models.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;

namespace D4w_cross.Models
{
    class HoldOnContainer : INotifyPropertyChanged
    {

        private DateTime date;
        private Timer timer;

        public event EventHandler OnStop;
        public event PropertyChangedEventHandler PropertyChanged;
        public String TimeStr { get; set; }
        public String TimeFormat { get; set; }

        public Booking CurBooking { get; set; }
        public Coworking CurCoworking { get; set; }

        public DateTime AdditionalTime { get; set; } = new DateTime(2017, 01, 01, 0, 0, 0);

        public HoldOnContainer()
        {
        }

        public void Start(DateTime date)
        {
            Stop();
            this.date = date;
            timer = new Timer(1000);
            timer.Elapsed += (sender, e) => Tick();
            timer.Enabled = true;
        }

        public void Stop()
        {
            if (timer != null)
            {
                timer.Enabled = false;
                timer = null;
            }
        }

        public void Tick()
        {
            TimeSpan cur = date - DateTime.Now;
            if (cur.TotalSeconds <= 0)
            {
                timer.Enabled = false;
                timer = null;
                OnStop.Invoke(this, new EventArgs());
            }
            if (PropertyChanged != null)
            {
                TimeStr = cur.ToString(TimeFormat);
                PropertyChanged(this, new PropertyChangedEventArgs("TimeStr"));
            }
        }

        public void UpdateAdditionalTime()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("AdditionalTime"));
            }
        }

        
    }
}
