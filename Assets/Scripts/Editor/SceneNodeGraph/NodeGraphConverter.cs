using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace SceneNodeGraph
{
    public class NodeGraphConverter : JsonConverter
    {
        public static NodeGraphConverter converter = new NodeGraphConverter();
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
            foreach (KeyValuePair<string, BaseNode> pair in nodeGraphData.tNodeMap)
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
            foreach(var pair in nodeGraphData.tTransitions)
            {
                string fromNodeId = pair.Key;
                writer.WritePropertyName(fromNodeId);
                writer.WriteStartArray();
                List<NodeTransitionData> transitions = pair.Value;
                foreach(NodeTransitionData transition in transitions)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("sToNodeId");
                    writer.WriteValue(transition.sToNodeId);
                    writer.WritePropertyName("nPath");
                    writer.WriteValue(transition.nPath);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
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
                    Type type = BaseNode.GetType(nodeType);
                    if (type == null)
                        type = typeof(BaseNode);
                    BaseNode nodeData = (BaseNode)Activator.CreateInstance(type);
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
                    string fromNodeId = property.Name;
                    if (!nodeGraphData.tTransitions.ContainsKey(fromNodeId))
                        nodeGraphData.tTransitions[fromNodeId] = new List<NodeTransitionData>();
                    JArray nodeTransitionsToken = property.Value as JArray;
                    foreach(JToken nodeTransitionToken in nodeTransitionsToken.Children())
                    {
                        NodeTransitionData transition = new NodeTransitionData();
                        transition.sToNodeId = nodeTransitionToken["sToNodeId"].Value<string>();
                        transition.nPath = nodeTransitionToken["nPath"].Value<int>();
                        nodeGraphData.tTransitions[fromNodeId].Add(transition);
                    }
                }
            }

            if (jsonObject.TryGetValue("sStartNodeId", out _))
                nodeGraphData.sStartNodeId = jsonObject["sStartNodeId"].Value<string>();

            return nodeGraphData;
        }
    }
}
