using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [CustomEditor(typeof(TriggerEditorData))]
    public class TriggerEditor: BaseGameEditor
    {

        protected override void InitTrans()
        {
            base.InitTrans();
            containerTrans = GameEditorHelper.GetOrAddChild(rootSceneDataTrans, "Triggers");
            editorDataTrans = GameEditorHelper.GetOrAddChild(rootEditorDataTrans, "Trigger");
        }

        protected override GameObject AddConfig()
        {
            GameObject prefab = Resources.Load<GameObject>("GamePrefabs/Trigger/Trigger");
            GameObject configObject = Instantiate(prefab);
            int genId = GameEditorHelper.GenChildId(containerTrans);
            configObject.name = genId.ToString();
            LuaOnTrigger luaOnTrigger = configObject.GetComponent<LuaOnTrigger>();
            if (luaOnTrigger != null)
                luaOnTrigger.triggerId = genId;
            configObject.transform.SetParent(containerTrans);
            configObject.transform.position = Vector3.zero;
            BoxCollider collider = configObject.GetComponent<BoxCollider>();
            if (collider == null)
                collider = configObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.center = Vector3.zero;
            return configObject;
        }

    }
}