using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [CustomEditor(typeof(TransparentWallEditorData))]
    public class TransparentWallEditor : BaseGameEditor
    {

        protected override void InitTrans()
        {
            base.InitTrans();
            containerTrans = GameEditorHelper.GetOrAddChild(rootSceneDataTrans, "TransparentWall");
            editorDataTrans = GameEditorHelper.GetOrAddChild(rootEditorDataTrans, "TransparentWall");
        }

        protected override GameObject AddConfig()
        {
            GameObject configObject = base.AddConfig();
            BoxCollider collider = configObject.GetComponent<BoxCollider>();
            if (collider == null)
                collider = configObject.AddComponent<BoxCollider>();
            collider.isTrigger = false;
            collider.center = Vector3.zero;
            return configObject;
        }
    }
}