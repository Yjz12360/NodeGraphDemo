using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(PositionEditorData))]
    public class PositionEditor : BaseGameEditor
    {

        protected override void InitTrans()
        {
            base.InitTrans();
            containerTrans = GameEditorHelper.GetOrAddChild(rootConfigTrans, "Position");
            editorDataTrans = GameEditorHelper.GetOrAddChild(rootEditorDataTrans, "Position");
        }
    }
}