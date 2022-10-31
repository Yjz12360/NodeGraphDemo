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
    public class MonsterHpRateNode : BaseNode
    {
        public override Type GetPathType() { return typeof(CustomPath); }

        public int nRefreshId;

        [CustomDrawer(typeof(HpRatePathDrawer))]
        public List<float> tHpRates = new List<float>();
    }

    public class HpRatePathDrawer : BaseCustomDrawer
    {
        public override void DrawAttr(FieldInfo fieldInfo, object data)
        {
            List<float> tHpRates = (List<float>)fieldInfo.GetValue(data);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Path配置：");
                if (GUILayout.Button("添加"))
                    tHpRates.Add(0);
            }
            int removeIndex = -1;
            for (int i = 0; i < tHpRates.Count; ++i)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    float oriRate = tHpRates[i];
                    tHpRates[i] = EditorGUILayout.FloatField($"Path{(i + 1)} Hp Rate >", tHpRates[i]);
                    tHpRates[i] = Mathf.Clamp(tHpRates[i], 0, 1);
                    //if (tHpRates[i] != oriRate)
                    //{
                    //    tHpRates.Sort();
                    //    tHpRates.Reverse();
                    //}
                    if (GUILayout.Button("删除"))
                        removeIndex = i;
                }
            }
            using (new EditorGUILayout.HorizontalScope())
                EditorGUILayout.LabelField($"Path{(tHpRates.Count + 1)} Hp Rate < {tHpRates[tHpRates.Count - 1]}");
            if (removeIndex >= 0)
                tHpRates.RemoveAt(removeIndex);
        }
    }

}
