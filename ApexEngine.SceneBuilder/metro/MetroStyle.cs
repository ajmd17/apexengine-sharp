using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ModernUISample.metro
{
    /// <summary>
    /// MetroStyle-Konfiguration
    /// </summary>
    public class MetroStyle : INotifyPropertyChanged
    {
        private Font _baseFont;
        private Font _boldFont;
        private Font _lightFont;
        private Color _backColor;
        private Color _foreColor;
        private Color _accentColor;
        private Color _accentFrontColor;
        private Color _disabledColor;
        private bool _darkStyle;

        /// <summary>
        /// Font for Standard-Output
        /// </summary>
        public Font BaseFont
        {
            get { return _baseFont; }
            set
            {
                if (value.Equals(_baseFont) == false)
                {
                    _baseFont = value;
                    OnPropertyChanged("BaseFont");
                }
            }
        }
        /// <summary>
        /// Font for Bold-Output
        /// </summary>
        public Font BoldFont
        {
            get { return _boldFont; }
            set
            {
                if (value.Equals(_boldFont) == false)
                {
                    _boldFont = value;
                    OnPropertyChanged("BoldFont");
                }
            }
        }
        /// <summary>
        /// Font for Light-Output (thinner)
        /// </summary>
        public Font LightFont
        {
            get { return _lightFont; }
            set
            {
                if (value.Equals(_lightFont) == false)
                {
                    _lightFont = value;
                    OnPropertyChanged("LightFont");
                }
            }
        }

        /// <summary>
        /// Backgroundcolor
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (value.Equals(_backColor) == false)
                {
                    _backColor = value;
                    OnPropertyChanged("BackColor");
                }
            }
        }
        /// <summary>
        /// ForeColor
        /// </summary>
        public Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                if (value.Equals(_foreColor) == false)
                {
                    _foreColor = value;
                    OnPropertyChanged("ForeColor");
                }
            }
        }
        /// <summary>
        /// Accented Elements
        /// </summary>
        public Color AccentColor
        {
            get { return _accentColor; }
            set
            {
                if (value.Equals(_accentColor) == false)
                {
                    _accentColor = value;
                    OnPropertyChanged("AccentColor");
                }
            }
        }
        /// <summary>
        /// Accented Elements ForeColor
        /// </summary>
        public Color AccentFrontColor
        {
            get { return _accentFrontColor; }
            set
            {
                if (value.Equals(_accentFrontColor) == false)
                {
                    _accentFrontColor = value;
                    OnPropertyChanged("AccentFrontColor");
                }
            }
        }
        /// <summary>
        /// Disabled Elements
        /// </summary>
        public Color DisabledColor
        {
            get { return _disabledColor; }
            set
            {
                if (value.Equals(_disabledColor) == false)
                {
                    _disabledColor = value;
                    OnPropertyChanged("DisabledColor");
                }
            }
        }

        /// <summary>
        /// true if the application uses the 'Dark'-Style
        /// </summary>
        public bool DarkStyle
        {
            get { return _darkStyle; }
            set
            {
                if (value != _darkStyle)
                {
                    _darkStyle = value;
                    if (_darkStyle)
                    {
                        BackColor = Color.FromArgb(51, 51, 51);
                        ForeColor = Color.White;
                        AccentColor = Color.DarkOrange;
                        AccentFrontColor = Color.FromArgb(51, 51, 51);
                        DisabledColor = Color.DimGray;
                    }
                    else
                    {
                        BackColor = Color.White;
                        ForeColor = Color.FromArgb(51, 51, 51);
                        AccentColor = Color.FromArgb(0, 114, 198);
                        AccentFrontColor = Color.White;
                        DisabledColor = Color.DimGray;
                    }

                    OnPropertyChanged("DarkStyle");
                }
            }
        }
 
        /// <summary>
        /// support the INotifyPropertyChanged Interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
       
        /// <summary>
        /// support the INotifyPropertyChanged Interface
        /// </summary>
        /// <param name="property">property name</param>
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
       
        /// <summary>
        /// Initializes the visual properties
        /// </summary>
        public MetroStyle()
        {
            BaseFont = new Font("Arial", 10f);
            BoldFont = new Font("Arial", 10f, FontStyle.Bold);
            LightFont = new Font("Arial", 8f, FontStyle.Regular);

            DarkStyle = false;
        }
    }
}
