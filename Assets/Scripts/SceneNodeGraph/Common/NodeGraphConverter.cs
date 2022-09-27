using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace SceneNodeGraph
{
    public class SceneNodeGraphConverter : JsonConverter
    {
        public static SceneNodeGraphConverter converter = new SceneNodeGraphConverter();
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(NodeGraphData) || objectType.IsSubclassOf(typeof(NodeGraphData)); ;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            NodeGraphData nodeGraphData = (NodeGraphData)value;
            writer.WriteStartObject();

            writer.WritePropertyName("tNodeMap");
            writer.WriteStartObject();
            foreach (KeyValuePair<string, BaseNodeData> pair in nodeGraphData.tNodeMap)
            {
                writer.WritePropertyName(pair.Key);
                Type type = pair.Value.GetType();
                PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                writer.WriteStartObject();
                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    Type fieldType = fieldInfo.GetType();
                    if(fieldType.IsPrimitive || fieldType.IsEnum || fieldType.Equals(typeof(string)))
                    {
                        writer.WritePropertyName(fieldInfo.Name);
                        writer.WriteValue(fieldInfo.GetValue(pair.Value));
                    }
                    else
                    {
                        Debug.LogError($"NodeGraphConverter: not support type: {fieldType}");
                    }
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();


            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
