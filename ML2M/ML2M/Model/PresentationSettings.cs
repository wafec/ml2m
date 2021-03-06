﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ML2M.Model
{
    public class PresentationSettings : INotifyPropertyChanged
    {
        private int _fontSize = 30;
        private string _margin = "5,5,5,5";
        private Visibility _tipVisibility = Visibility.Hidden;
        private Visibility _titleVisibility = Visibility.Visible;
        private Visibility _slidesVisibility = Visibility.Collapsed;

        public int FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                if (value < 10)
                    _fontSize = 30;
                else
                    _fontSize = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FontSize"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SmallFontSize"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SubFontSize"));
                }
            }
        }

        public int SmallFontSize
        {
            get
            {
                return (int) Math.Ceiling(FontSize * 0.7);
            }
        }

        public int SubFontSize
        {
            get
            {
                return (int)Math.Ceiling(FontSize * 0.5);
            }
        }

        public string Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                if (value == null || value == "0,0,0,0")
                    _margin = "5,5,5,5";
                else
                    _margin = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Margin"));
            }
        }

        public Visibility TipVisibility
        {
            get
            {
                return _tipVisibility;
            }
            set
            {
                _tipVisibility = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TipVisibility"));
            }
        }

        public Visibility TitleVisibility
        {
            get
            {
                return _titleVisibility;
            }
            set
            {
                _titleVisibility = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TitleVisibility"));
            }
        }

        public Visibility SlidesVisibility
        {
            get
            {
                return _slidesVisibility;
            }
            set
            {
                _slidesVisibility = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SlidesVisibility"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Update(int width, int height)
        {
            FontSize = (int) (Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)) * 0.065);
            int margin = (int)(width * 0.1);
            Margin = string.Format("{0},30,{0},30", margin);
            Console.WriteLine(Margin);
        }

        public void Update(double width, double height)
        {
            Update((int)width, (int)height);
        }
    }
}
