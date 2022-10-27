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
            foreach (KeyValuePair<int, BaseNode> pair in nodeGraphData.nodeMap)
            {
                writer.WritePropertyName(pair.Key.ToString());
                Type type = pair.Value.GetType();
                PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                writer.WriteStartObject();

                writer.WritePropertyName("nNodeType");
                writer.WriteValue(pair.Value.GetNodeType());

                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    if (!RequireAttrChecker.Check(fieldInfo, pair.Value))
                        continue;
                    Type fieldType = fieldInfo.FieldType;
                    if(fieldType.IsPrimitive || fieldType.IsEnum || fieldType.Equals(typeof(string)))
                    {
                        writer.WritePropertyName(fieldInfo.Name);
                        writer.WriteValue(fieldInfo.GetValue(pair.Value));
                    }
                    else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        writer.WritePropertyName(fieldInfo.Name);
                        writer.WriteRawValue(JsonConvert.SerializeObject(fieldInfo.GetValue(pair.Value)));
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
            foreach (var pair in nodeGraphData.transitions)
            {
                int fromNodeId = pair.Key;
                writer.WritePropertyName(fromNodeId.ToString());
                writer.WriteStartObject();
                foreach(var nodeTransitions in pair.Value)
                {
                    int path = nodeTransitions.Key;
                    writer.WritePropertyName(path.ToString());
                    writer.WriteStartArray();
                    foreach (int toNodeId in nodeTransitions.Value)
                    {
                        writer.WriteValue(toNodeId);
                    }
                    writer.WriteEndArray();
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();

            writer.WritePropertyName("nStartNodeId");
            writer.WriteValue(nodeGraphData.startNodeId);

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
                            Type fieldType = fieldInfo.FieldType;
                            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                string rawValue = property.Value[fieldInfo.Name].ToString();
                                SetListValue(fieldInfo, nodeData, rawValue);
                            }
                            else
                            {
                                fieldInfo.SetValue(nodeData, property.Value[fieldInfo.Name].ToObject(fieldInfo.FieldType));
                            }
                        }
                    }
                    nodeGraphData.nodeMap[nodeData.nNodeId] = nodeData;
                }
            }

            if(jsonObject.TryGetValue("tTransitions", out JToken transitionToken))
            {
                foreach (JProperty property in transitionToken.Children())
                {
                    int.TryParse(property.Name, out int fromNodeId);
                    var transitions = nodeGraphData.transitions;
                    if (!transitions.ContainsKey(fromNodeId))
                        transitions[fromNodeId] = new Dictionary<int, List<int>>();
                    foreach (JProperty pathProp in property.Value.Children())
                    {
                        int path;
                        int.TryParse(pathProp.Name, out path);
                        var nodeTransitions = transitions[fromNodeId];
                        if (!nodeTransitions.ContainsKey(path))
                            nodeTransitions[path] = new List<int>();
                        foreach(JProperty pathTransitionProp in pathProp.Value.Children())
                        {
                            int toNodeId = pathTransitionProp.Value.Value<int>();
                            nodeTransitions[path].Add(toNodeId);
                        }
                    }
                }
            }

            if (jsonObject.TryGetValue("nStartNodeId", out _))
                nodeGraphData.startNodeId = jsonObject["nStartNodeId"].Value<int>();

            return nodeGraphData;
        }

        private void SetListValue(FieldInfo fieldInfo, object nodeData, string rawValue)
        {
            Type fieldType = fieldInfo.FieldType;
            Type argumentType = fieldType.GetGenericArguments()[0];
            if (argumentType == typeof(int))
                SetListValue<int>(fieldInfo, nodeData, rawValue);
            else if (argumentType == typeof(float))
                SetListValue<float>(fieldInfo, nodeData, rawValue);
            else if (argumentType == typeof(string))
                SetListValue<string>(fieldInfo, nodeData, rawValue);

        }

        private void SetListValue<T>(FieldInfo fieldInfo, object nodeData, string rawValue)
        {
            var dicValue = JsonConvert.DeserializeObject<Dictionary<string, T>>(rawValue);
            List<string> keys = new List<string>();
            foreach (string key in dicValue.Keys)
                keys.Add(key);
            keys.Sort();
            List<T> value = new List<T>();
            foreach (string key in keys)
                value.Add(dicValue[key]);
            fieldInfo.SetValue(nodeData, value);
        }
    }
}
