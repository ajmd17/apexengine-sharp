using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernUISample.metro
{
    /// <summary>
    /// Static Layer for all MetroUI related settings and helper-methods
    /// </summary>
    public static class MetroUI
    {
        private static MetroStyle _style;
        private static bool _designmode;
        private static bool _initialized;

        /// <summary>
        /// Gives true if the application runs in the Designer and false if application runs in normal runtime environment. 
        /// Use this method to encapsulate Code that should only be running at normal runtime.
        /// </summary>
        public static bool DesignMode
        {
            get { if (_initialized == false) _initialize(); return _designmode; }
        }

        private static void _initialize()
        {
            _designmode = (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");

            _initialized = true;
        }

        /// <summary>
        /// Definition of the MetroStyle
        /// 
        /// Configures parameters which affects the visual presentation.
        /// </summary>
        public static MetroStyle Style
        {
            get
            {
                if (_style == null)
                    _style = new MetroStyle();

                return _style;
            }
        }
    }
}
