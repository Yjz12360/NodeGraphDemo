using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

                writer.WritePropertyName("nNodeType");
                writer.WriteValue(pair.Value.GetNodeType());

                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    Type fieldType = fieldInfo.FieldType;
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

            writer.WritePropertyName("tTransitions");
            writer.WriteStartObject();
            //writer.WriteStartArray();
            //foreach(NodeTransitionData transition in nodeGraphData.tTransitions)
            for(int i = 0; i < nodeGraphData.tTransitions.Count; ++i)
            {
                NodeTransitionData transition = nodeGraphData.tTransitions[i];
                writer.WritePropertyName(i.ToString());
                writer.WriteStartObject();
                writer.WritePropertyName("sFromNodeId");
                writer.WriteValue(transition.sFromNodeId);
                writer.WritePropertyName("sToNodeId");
                writer.WriteValue(transition.sToNodeId);
                writer.WritePropertyName("nPath");
                writer.WriteValue(transition.nPath);
                writer.WriteEndObject();
            }
            //writer.WriteEndArray();
            writer.WriteEndObject();

            writer.WritePropertyName("sStartNodeId");
            writer.WriteValue(nodeGraphData.sStartNodeId);

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jsonObject = (JObject)JToken.ReadFrom(reader);

            NodeGraphData nodeGraphData = new NodeGraphData();

            if(jsonObject.TryGetValue("tNodeMap", out JToken nodeMapToken))
            {
                foreach(JProperty property in nodeMapToken.Children())
                {
                    NodeType nodeType = (NodeType)property.Value["nNodeType"].Value<int>();
                    Type type = BaseNodeData.GetType(nodeType);
                    if (type == null)
                        type = typeof(BaseNodeData);
                    BaseNodeData nodeData = (BaseNodeData)Activator.CreateInstance(type);
                    FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                    foreach(FieldInfo fieldInfo in fieldInfos)
                    {
                        if(property.Value[fieldInfo.Name] != null)
                        {
                            fieldInfo.SetValue(nodeData, property.Value[fieldInfo.Name].ToObject(fieldInfo.FieldType));
                        }
                    }
                    nodeGraphData.tNodeMap[nodeData.sNodeId] = nodeData;
                }
            }

            if(jsonObject.TryGetValue("tTransitions", out JToken transitionToken))
            {
                foreach (JProperty property in transitionToken.Children())
                {
                    NodeTransitionData transition = new NodeTransitionData();
                    transition.sFromNodeId = property.Value["sFromNodeId"].Value<string>();
                    transition.sToNodeId = property.Value["sToNodeId"].Value<string>();
                    transition.nPath = property.Value["nPath"].Value<int>();
                    nodeGraphData.tTransitions.Add(transition);
                }
            }

            if (jsonObject.TryGetValue("sStartNodeId", out _))
                nodeGraphData.sStartNodeId = jsonObject["sStartNodeId"].Value<string>();

            //var obj = JObject.Load(reader);

            //NodeGraphData nodeGraphData = new NodeGraphData();

            //JToken nodeMapToken;
            //if (obj.TryGetValue("tNodeMap", out nodeMapToken))
            //{
            //    nodeMapToken
            //}

            //if (obj.TryGetValue("sStartNodeId", out _))
            //    nodeGraphData.sStartNodeId = obj["sStartNodeId"].Value<string>();


            return nodeGraphData;
        }
    }
}
