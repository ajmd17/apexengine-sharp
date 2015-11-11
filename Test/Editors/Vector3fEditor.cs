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
    public class Vector3fEditor : System.Drawing.Design.UITypeEditor
    {
        private IWindowsFormsEditorService edSvc = null;
        public Vector3f vector3fValue;
        public Vector3f Vector3fValue
        {
            get { return vector3fValue; }
            set { vector3fValue = value; }
        }
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
                    Vector3f vec = (Vector3f)value;
                    Vector3fEditForm editForm = new Vector3fEditForm(vec.x, vec.y, vec.z);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        return new Vector3f(editForm.x, editForm.y, editForm.z);
                    }
                    else
                    {
                        return vec;
                    }
                }
            }
            return new Vector3f();
        }
    }
}
