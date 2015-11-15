using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;
using ApexEngine.Math;
using ApexEngine.Rendering;
namespace ApexEditor.Editors
{
    public class MaterialEditor : System.Drawing.Design.UITypeEditor
    {
        private IWindowsFormsEditorService edSvc = null;
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            if (context != null
                && context.Instance != null
                && provider != null)
            {

                edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (edSvc != null)
                {
                    frmMatEditor matEdit = new frmMatEditor();
                    matEdit.Init();
                    matEdit.Material = (Material)value;
                    matEdit.ShowDialog();
                    if (matEdit.DialogResult == DialogResult.OK)
                    {
                        return matEdit.Material;
                    }
                }
            }
            return value;
        }
    }
}
