using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace SceneNodeGraph
{
    public class SetPositionNode : BaseNode
    {
        public bool bSetPlayer;
        public int nRefreshId;
        public int nPosId;
        [RequireInt("nPosId", 0)]
        public float nPosX;
        [RequireInt("nPosId", 0)]
        public float nPosY;
        [RequireInt("nPosId", 0)]
        public float nPosZ;

        [CustomDrawer(typeof(Vector3Drawer))]
        [CustomConverter(typeof(Vector3Converter))]
        [RequireInt("nPosId", 0)]
        public Vector3 tPos;
    }

}
