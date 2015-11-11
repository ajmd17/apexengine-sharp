using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel;
namespace ApexEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TypeDescriptor.AddAttributes(typeof(ApexEngine.Math.Vector3f),
               new EditorAttribute(typeof(Editors.Vector3fEditor), typeof(UITypeEditor)));
            TypeDescriptor.AddAttributes(typeof(ApexEngine.Math.Quaternion),
               new EditorAttribute(typeof(Editors.QuaternionEditor), typeof(UITypeEditor)));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
