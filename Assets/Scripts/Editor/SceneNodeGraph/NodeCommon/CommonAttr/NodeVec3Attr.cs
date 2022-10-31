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
    public class Vector3Drawer : BaseCustomDrawer
    {
        public override void DrawAttr(FieldInfo fieldInfo, object data)
        {
            Vector3 vector = (Vector3)fieldInfo.GetValue(data);
            Vector3 oriVector = vector;
            EditorGUILayout.LabelField(fieldInfo.Name);
            vector.x = EditorGUILayout.FloatField("x", vector.x);
            vector.y = EditorGUILayout.FloatField("y", vector.y);
            vector.z = EditorGUILayout.FloatField("z", vector.z);
            if(vector != oriVector)
            {
                fieldInfo.SetValue(data, vector);
            }
        }
    }

    public class Vector3Converter : BaseNodeAttrConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector3);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector3 vector = (Vector3)value;
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(vector.x);
            writer.WritePropertyName("y");
            writer.WriteValue(vector.y);
            writer.WritePropertyName("z");
            writer.WriteValue(vector.z);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            var obj = JObject.Load(reader);
            Vector3 vector = new Vector3(0, 0, 0);
            if (obj.TryGetValue("x", out JToken xToken))
                vector.x = xToken.Value<float>();
            if (obj.TryGetValue("y", out JToken yToken))
                vector.y = yToken.Value<float>();
            if (obj.TryGetValue("z", out JToken zToken))
                vector.z = zToken.Value<float>();
            return vector;
        }
    }
}
