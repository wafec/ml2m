using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;

namespace ML2M.Model
{
    public class MainProgressControl : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _progressText;
        private Visibility _visibility = Visibility.Hidden;

        public string ProgressText
        {
            get
            {
                return _progressText;
            }
            set
            {
                _progressText = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProgressText"));
            }
        }

        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Visibility"));
            }
        }

        public void Hide(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Visibility = Visibility.Hidden;
            }
            else
            {
                ProgressText = text;
                Timer timer = new Timer(2000);
                timer.Elapsed += OnTimedEvent;
                timer.AutoReset = false;
                timer.Enabled = true;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Visibility = Visibility.Hidden;          
        }

        public void Show(string text)
        {      
            ProgressText = text;
            Visibility = Visibility.Visible;
        }
    }
}
