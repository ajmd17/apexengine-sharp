using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;
namespace ApexEngine.Rendering.Util
{
    public class ShaderUtil
    {
        public static string GetValueString(string varName, object val)
        {
            if (val is bool)
            {
                bool bval = (bool)val;
                return (bval ? "true" : "false");
            }
            else if (val is int)
            {
                int ival = (int)val;
                return ival.ToString();
            }
            else if (val is float)
            {
                float fval = (float)val;
                return fval.ToString();
            }
            else if (val is Vector2f)
            {
                Vector2f vval = (Vector2f)val;
                return "vec2(" + vval.x + ", " + vval.y + ")";
            }
            else if (val is Vector3f)
            {
                Vector3f vval = (Vector3f)val;
                return "vec3(" + vval.x + ", " + vval.y + ", " + vval.z + ")";
            }
            else if (val is Vector4f)
            {
                Vector4f vval = (Vector4f)val;
                return "vec4(" + vval.x + ", " + vval.y + ", " + vval.z + ", " + vval.w + ")";
            }
            return "";
        }
        public static string FormatShaderIncludes(string shaderPath, string origCode)
        {
            string res = "";
            string[] lines = origCode.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Trim().StartsWith("#include"))
                {
                    string path = line.Trim().Substring("#include ".Length);
                    path = path.Replace("\"", "");

                    string parentPath = System.IO.Directory.GetParent(shaderPath).ToString();
                    string incPath = parentPath + "\\" + path;

                    line = (string)new Assets.ShaderTextLoader().Load(incPath);
                }
                if (lines[i] != "")
                    res += line + "\n";
            }
            return res;
        }
        public static string FormatShaderProperties(string origCode, ShaderProperties properties)
        {
            string res = "";
            string[] lines = origCode.Split('\n');
            bool inIfStatement = false;
            String ifStatementText = "";
            bool removing = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Trim().StartsWith("#ifdef"))
                {
                    inIfStatement = true;
                    ifStatementText = lines[i].Trim().Substring(7);
                    if (properties.GetBool(ifStatementText) == false)
                        removing = true;
                    else
                        removing = false;
                    lines[i] = "";
                }
                else if (lines[i].Trim().StartsWith("#ifndef"))
                {
                    inIfStatement = true;
                    ifStatementText = lines[i].Trim().Substring(8);
                    if (properties.GetBool(ifStatementText) == true)
                        removing = true;
                    else
                        removing = false;
                    lines[i] = "";
                }
                else if (lines[i].Trim().StartsWith("#endif"))
                {
                    if (inIfStatement)
                    {
                        inIfStatement = false;
                        removing = false;
                    }
                    lines[i] = "";
                }
                if (inIfStatement && removing)
                {
                    lines[i] = "";
                }
                if (lines[i] != "")
                    res += lines[i] + "\n";
            }
            foreach (var val in properties.values)
            {
                res = res.Replace("$" + val.Key, GetValueString(val.Key, val.Value));
            }
            return res;
        }
    }
}
