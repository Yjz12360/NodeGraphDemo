using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(MonsterEditorData))]
    public class MonsterEditor : BaseGameEditor
    {

        protected override void InitTrans()
        {
            base.InitTrans();
            containerTrans = GameEditorHelper.GetOrAddChild(rootConfigTrans, "Monster");
            editorDataTrans = GameEditorHelper.GetOrAddChild(rootEditorDataTrans, "Monster");
        }

        protected override GameObject AddConfig()
        {
            GameObject configObject = base.AddConfig();
            configObject.AddComponent<ConfigId>();
            configObject.AddComponent<PathEditorData>();
            return configObject;
        }

    }
}