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
namespace ApexEditor.Editors
{
    public class QuaternionEditor : System.Drawing.Design.UITypeEditor
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
                    Quaternion quat = (Quaternion)value;
                    QuaternionEditForm editForm = new QuaternionEditForm(quat.x, quat.y, quat.z, quat.w);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        return new Quaternion(editForm.x, editForm.y, editForm.z, editForm.w);
                    }
                    else
                    {
                        return quat;
                    }
                }
            }
            return new Quaternion();
        }
    }
}
