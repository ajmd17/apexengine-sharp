using ApexEngine.Math;
using ApexEngine.Rendering.Animation;
using System.Xml;

namespace ApexEngine.Assets.OgreXml
{
    public class OgreXmlSkeletonLoader : AssetLoader
    {
        private static OgreXmlSkeletonLoader instance = new OgreXmlSkeletonLoader();

        public static OgreXmlSkeletonLoader GetInstance()
        {
            return instance;
        }

        private string keyframeBoneName = "";
        private float keyframeBoneTime = 0f, keyframeBoneAngle = 0f;
        private Vector3f keyframeBoneTrans = new Vector3f(Vector3f.Zero), keyframeBoneAxis = new Vector3f(Vector3f.Zero);
        public Skeleton skeleton = new Skeleton();
        private string lastElement = "";
        private float bindAngle = 0f;
        private Vector3f bindAxis = new Vector3f(Vector3f.Zero);
        private int boneIdx = 0;

        public override void ResetLoader()
        {
            skeleton = new Skeleton();
            bindAxis = new Vector3f(Vector3f.Zero);
            bindAngle = 0f;
            lastElement = "";
            keyframeBoneTrans = new Vector3f(Vector3f.Zero);
            keyframeBoneAxis = new Vector3f(Vector3f.Zero);
            keyframeBoneTime = 0f;
            keyframeBoneAngle = 0f;
            keyframeBoneName = "";
        }

        public override object Load(LoadedAsset asset)
        {
            XmlReader xmlReader = XmlReader.Create(asset.Data);
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "bone")
                    {
                        string name = xmlReader.GetAttribute("name");
                        boneIdx = int.Parse(xmlReader.GetAttribute("id"));
                        skeleton.AddBone(new Bone(name));
                    }
                    else if (xmlReader.Name == "position")
                    {
                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));
                        float z = float.Parse(xmlReader.GetAttribute("z"));
                        Vector3f vec = new Vector3f(x, y, z);
                        skeleton.GetBone(boneIdx).SetBindTranslation(vec);
                    }
                    else if (xmlReader.Name == "rotation")
                    {
                        float angle = float.Parse(xmlReader.GetAttribute("angle"));
                        bindAngle = angle;
                    }
                    else if (xmlReader.Name == "boneparent")
                    {
                        string parent = xmlReader.GetAttribute("parent");
                        string child = xmlReader.GetAttribute("bone");
                        Bone parentBone = null;
                        for (int i = 0; i < skeleton.GetNumBones(); i++)
                            if (skeleton.GetBone(i).Name == parent)
                                parentBone = skeleton.GetBone(i);
                        for (int i = 0; i < skeleton.GetNumBones(); i++)
                        {
                            if (skeleton.GetBone(i).Name == child)
                            {
                                Bone b = skeleton.GetBone(i);
                                parentBone.AddChild(b);
                            }
                        }
                    }
                    else if (xmlReader.Name == "track")
                    {
                        string bone = xmlReader.GetAttribute("bone");
                        keyframeBoneName = bone;
                        Bone b = skeleton.GetBone(keyframeBoneName);
                        if (b != null)
                            skeleton.GetAnimations()[skeleton.GetAnimations().Count - 1].AddTrack(new AnimationTrack(b));
                    }
                    else if (xmlReader.Name == "translate")
                    {
                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));
                        float z = float.Parse(xmlReader.GetAttribute("z"));
                        Vector3f vec = new Vector3f(x, y, z);
                        keyframeBoneTrans = vec;
                    }
                    else if (xmlReader.Name == "rotate")
                    {
                        float angle = float.Parse(xmlReader.GetAttribute("angle"));
                        keyframeBoneAngle = angle;
                    }
                    else if (xmlReader.Name == "axis")
                    {
                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));
                        float z = float.Parse(xmlReader.GetAttribute("z"));
                        Vector3f vec = new Vector3f(x, y, z);
                        string peek = lastElement;
                        if (peek == "rotate") // it is a keyframe
                        {
                            keyframeBoneAxis = vec;
                        }
                        else if (peek == "rotation") // it is a bone bind pose
                        {
                            bindAxis = vec;
                            skeleton.GetBone(boneIdx).SetBindAxisAngle(bindAxis.Normalize(), bindAngle);
                        }
                    }
                    else if (xmlReader.Name == "keyframe")
                    {
                        keyframeBoneTime = float.Parse(xmlReader.GetAttribute("time"));
                    }
                    else if (xmlReader.Name == "animation")
                    {
                        skeleton.GetAnimations().Add(new Animation(xmlReader.GetAttribute("name")));
                    }
                    lastElement = xmlReader.Name;
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    if (xmlReader.Name == "keyframe")
                    {
                        Bone b = skeleton.GetBone(keyframeBoneName);
                        if (b != null)
                        {
                            Keyframe keyf = null;
                            skeleton.GetAnimations()[skeleton.GetAnimations().Count - 1]
                                .GetTrack(skeleton.GetAnimations()[skeleton.GetAnimations().Count - 1]
                                .GetTracks().Count - 1)
                                .AddKeyframe(keyf = new Keyframe(keyframeBoneTime, keyframeBoneTrans, keyframeBoneAxis, keyframeBoneAngle));
                        }
                        keyframeBoneAngle = 0f;
                        keyframeBoneAxis = new Vector3f(Vector3f.Zero);
                        keyframeBoneTime = 0f;
                        keyframeBoneTrans = new Vector3f(Vector3f.Zero);
                    }
                    else if (xmlReader.Name == "track")
                    {
                        keyframeBoneName = "";
                    }
                }
            }
            xmlReader.Close();

            return skeleton;
        }
    }
}