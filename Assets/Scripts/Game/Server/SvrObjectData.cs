
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;

namespace Game
{
    public class SvrObjectData
    {
        public int nGameObjectId;
        public GameObjectType nType;
        public int nStaticId;
        public int nMonsterTid;
        public float nSpeed;
        public Vector3 position = Vector3.zero;
    }
}
