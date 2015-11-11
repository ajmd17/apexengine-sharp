using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModernUISample.metro
{
    /// <summary>
    /// Toolstrip for ModernUI-GUIs
    /// </summary>
    public class MetroToolStrip : System.Windows.Forms.ToolStrip
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        public MetroToolStrip()
            : base()
        {
            Renderer = new metroToolStripRenderer();
            Font = MetroUI.Style.BaseFont;
            ForeColor = MetroUI.Style.ForeColor;
        }

        /// <summary>
        /// OnItemAdded-Event we adjust the font and forecolor of this item
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            e.Item.Font = MetroUI.Style.BaseFont;
            e.Item.ForeColor = MetroUI.Style.ForeColor;

            base.OnItemAdded(e);
        }
    }
}
