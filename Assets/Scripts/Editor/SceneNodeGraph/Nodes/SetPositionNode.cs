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

        //[CustomDrawer(typeof(Vector3Drawer))]
        //[CustomConverter(typeof(Vector3Converter))]
        //public Vector3 vecPos;
    }

    //public class Vector3Drawer : BaseCustomDrawer
    //{
    //    public override void DrawAttr(FieldInfo fieldInfo, object data)
    //    {
    //        SetPositionNode setPositionNode = (SetPositionNode)data;
    //        EditorGUILayout.LabelField("vecPos: ");
    //        setPositionNode.vecPos.x = EditorGUILayout.FloatField("x", setPositionNode.vecPos.x);
    //        setPositionNode.vecPos.y = EditorGUILayout.FloatField("y", setPositionNode.vecPos.y);
    //        setPositionNode.vecPos.z = EditorGUILayout.FloatField("z", setPositionNode.vecPos.z);
    //    }
    //}

    //public class Vector3Converter : BaseNodeAttrConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return objectType == typeof(Vector3);
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        Vector3 vector = (Vector3)value;
    //        writer.WriteStartObject();
    //        writer.WritePropertyName("x");
    //        writer.WriteValue(vector.x);
    //        writer.WritePropertyName("y");
    //        writer.WriteValue(vector.y);
    //        writer.WritePropertyName("z");
    //        writer.WriteValue(vector.z);
    //        writer.WriteEndObject();
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null)
    //            return null;
    //        var obj = JObject.Load(reader);
    //        Vector3 vector = new Vector3(0, 0, 0);
    //        if (obj.TryGetValue("x", out JToken xToken))
    //            vector.x = xToken.Value<float>();
    //        if (obj.TryGetValue("y", out JToken yToken))
    //            vector.y = yToken.Value<float>();
    //        if (obj.TryGetValue("z", out JToken zToken))
    //            vector.z = zToken.Value<float>();
    //        return vector;
    //    }
    //}
}
