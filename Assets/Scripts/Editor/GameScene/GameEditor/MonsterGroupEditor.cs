using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(MonsterGroupEditorData))]
    public class MonsterGroupEditor : BaseGameEditor
    {

        protected override void InitTrans()
        {
            base.InitTrans();
            containerTrans = GameEditorHelper.GetOrAddChild(rootConfigTrans, "MonsterGroup");
            editorDataTrans = GameEditorHelper.GetOrAddChild(rootEditorDataTrans, "MonsterGroup");
        }

        protected override GameObject AddConfig()
        {
            GameObject configObject = base.AddConfig();
            configObject.AddComponent<MonsterGroupConfig>();
            return configObject;
        }

    }
}