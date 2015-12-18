using ApexEngine.Math;
using System;

namespace ApexEngine.Networking
{
    public class PlayerInfo
    {
        private string screenName = "";
        private string id = "";
        private Transform transf = new Transform();

        public PlayerInfo(String id, String screenName, Vector3f pos)
        {
            this.id = id;
            this.screenName = screenName;
            this.transf.SetTranslation(pos);
        }

        public String GetID()
        {
            return id;
        }

        public String GetScreenName()
        {
            return screenName;
        }

        public Transform GetTransform()
        {
            return transf;
        }

        public Vector3f GetPosition()
        {
            return transf.GetTranslation();
        }
    }
}