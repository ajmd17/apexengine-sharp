using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModernUISample.metro
{
    /// <summary>
    /// Renderer for the ModernUI-Toolstrip
    /// </summary>
    class metroToolStripRenderer : ToolStripProfessionalRenderer
    {
        public metroToolStripRenderer()
            : base(new metrocolorscheme())
        {
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Item.Selected && e.Item.Pressed == false)
                e.TextColor = MetroUI.Style.AccentFrontColor;
            else
                e.TextColor = MetroUI.Style.ForeColor;

            base.OnRenderItemText(e);
        }

        /// <summary>
        /// Render an item
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            // automatic adjust the color of attached images...
            if (e.Item.Selected && e.Item.Pressed == false)
                e.Graphics.DrawImageUnscaled(e.Item.Image.AdjustRGBGamma(1f, 1f, 1f, 0.01f), e.ImageRectangle);
            else
            {
                if (MetroUI.Style.DarkStyle)
                    e.Graphics.DrawImageUnscaled(e.Item.Image.AdjustRGBGamma(1f, 1f, 1f, 0.01f), e.ImageRectangle);
                else
                    base.OnRenderItemImage(e);
            }
        }

        /// <summary>
        /// ColorTable for the Renderer
        /// </summary>
        class metrocolorscheme : ProfessionalColorTable
        {
            public override Color ButtonSelectedHighlight
            {
                get { return ButtonSelectedGradientMiddle; }
            }
            public override Color ButtonSelectedHighlightBorder
            {
                get { return ButtonSelectedBorder; }
            }
            public override Color ButtonPressedHighlight
            {
                get { return ButtonPressedGradientMiddle; }
            }
            public override Color ButtonPressedHighlightBorder
            {
                get { return ButtonPressedBorder; }
            }
            public override Color ButtonCheckedHighlight
            {
                get { return ButtonCheckedGradientMiddle; }
            }
            public override Color ButtonCheckedHighlightBorder
            {
                get { return ButtonSelectedBorder; }
            }
            public override Color ButtonPressedBorder
            {
                get { return ButtonSelectedBorder; }
            }
            public override Color ButtonSelectedBorder
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color ButtonCheckedGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ButtonCheckedGradientMiddle
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ButtonCheckedGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ButtonSelectedGradientBegin
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color ButtonSelectedGradientMiddle
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color ButtonSelectedGradientEnd
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color ButtonPressedGradientBegin
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color ButtonPressedGradientMiddle
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color ButtonPressedGradientEnd
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color CheckBackground
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color CheckSelectedBackground
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color CheckPressedBackground
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color GripDark
            {
                get { return MetroUI.Style.ForeColor; }
            }
            public override Color GripLight
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ImageMarginGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ImageMarginGradientMiddle
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ImageMarginGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ImageMarginRevealedGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ImageMarginRevealedGradientMiddle
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ImageMarginRevealedGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color MenuStripGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color MenuStripGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color MenuItemSelected
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color MenuItemBorder
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color MenuBorder
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color MenuItemPressedGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color MenuItemPressedGradientMiddle
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color MenuItemPressedGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color RaftingContainerGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color RaftingContainerGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color SeparatorDark
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color SeparatorLight
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color StatusStripGradientBegin
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color StatusStripGradientEnd
            {
                get { return MetroUI.Style.AccentColor; }
            }
            public override Color ToolStripBorder
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ToolStripGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ToolStripGradientMiddle
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ToolStripGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ToolStripContentPanelGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ToolStripContentPanelGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ToolStripPanelGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color ToolStripPanelGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color OverflowButtonGradientBegin
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color OverflowButtonGradientMiddle
            {
                get { return MetroUI.Style.BackColor; }
            }
            public override Color OverflowButtonGradientEnd
            {
                get { return MetroUI.Style.BackColor; }
            }
        }
    }
}
