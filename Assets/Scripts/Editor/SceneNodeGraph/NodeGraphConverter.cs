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
                writer.WriteStartObject();

                writer.WritePropertyName("nNodeType");
                writer.WriteValue(pair.Value.GetNodeType());

                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    if (fieldInfo.GetCustomAttribute<NodeOutputAttribute>() != null)
                        continue;
                    if (!RequireAttrChecker.Check(fieldInfo, pair.Value))
                        continue;
                    Type fieldType = fieldInfo.FieldType;
                    CustomConverterAttribute customConverter = fieldInfo.GetCustomAttribute<CustomConverterAttribute>();
                    if(customConverter != null)
                    {
                        writer.WritePropertyName(fieldInfo.Name);
                        writer.WriteRawValue(JsonConvert.SerializeObject(fieldInfo.GetValue(pair.Value), Formatting.None, customConverter.converter));
                    }
                    else if (fieldType.IsPrimitive || fieldType.IsEnum || fieldType.Equals(typeof(string)))
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

            writer.WritePropertyName("tNodeInputSource");
            writer.WriteStartObject();
            foreach (var pair in nodeGraphData.inputData)
            {
                int outputNodeId = pair.Key;
                writer.WritePropertyName(outputNodeId.ToString());
                writer.WriteStartObject();
                foreach(var data in pair.Value)
                {
                    string outputNodeAttr = data.Key;
                    if (string.IsNullOrEmpty(outputNodeAttr)) continue;
                    NodeInputData nodeInputData = data.Value;
                    int inputNodeId = nodeInputData.nodeId;
                    if (inputNodeId <= 0) continue;
                    string inputNodeAttr = nodeInputData.attrName;
                    if (string.IsNullOrEmpty(inputNodeAttr)) continue;
                    writer.WritePropertyName(outputNodeAttr);
                    writer.WriteStartObject();
                    writer.WritePropertyName("nInputNodeId");
                    writer.WriteValue(inputNodeId);
                    writer.WritePropertyName("sInputNodeAttr");
                    writer.WriteValue(inputNodeAttr);
                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();

            Dictionary<int, List<string>> requireDic = new Dictionary<int, List<string>>();
            foreach (var pair in nodeGraphData.inputData)
            {
                foreach (NodeInputData nodeInputData in pair.Value.Values)
                {
                    int nodeId = nodeInputData.nodeId;
                    if (!requireDic.ContainsKey(nodeId))
                        requireDic[nodeId] = new List<string>();
                    requireDic[nodeId].Add(nodeInputData.attrName);
                }
            }
            writer.WritePropertyName("tRequiredOutput");
            writer.WriteStartObject();
            foreach(var pair in requireDic)
            {
                writer.WritePropertyName(pair.Key.ToString());
                writer.WriteStartObject();
                foreach(string attr in pair.Value)
                {
                    writer.WritePropertyName(attr);
                    writer.WriteValue(true);
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
                        JToken valueToken = property.Value[fieldInfo.Name];
                        if (valueToken != null)
                        {
                            Type fieldType = fieldInfo.FieldType;
                            CustomConverterAttribute customConverter = fieldInfo.GetCustomAttribute<CustomConverterAttribute>();
                            if (customConverter != null)
                            {
                                fieldInfo.SetValue(nodeData, JsonConvert.DeserializeObject(valueToken.ToString(), fieldType, customConverter.converter));
                            }
                            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                string rawValue = valueToken.ToString();
                                SetListValue(fieldInfo, nodeData, rawValue);
                            }
                            else
                            {
                                fieldInfo.SetValue(nodeData, valueToken.ToObject(fieldType));
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

            if (jsonObject.TryGetValue("tNodeInputSource", out JToken inputDataToken))
            {
                foreach (JProperty inputDataProp in inputDataToken.Children())
                {
                    if (!int.TryParse(inputDataProp.Name, out int outputNodeId))
                        continue;
                    var inputData = nodeGraphData.inputData;
                    if (!inputData.ContainsKey(outputNodeId))
                        inputData[outputNodeId] = new Dictionary<string, NodeInputData>();
                    foreach (JProperty outputAttrProp in inputDataProp.Value.Children())
                    {
                        string outputAttr = outputAttrProp.Name;
                        if (!inputData[outputNodeId].ContainsKey(outputAttr))
                            inputData[outputNodeId][outputAttr] = new NodeInputData();
                        foreach(JProperty inputProp in outputAttrProp.Value.Children())
                        {
                            if (inputProp.Name == "nInputNodeId")
                                inputData[outputNodeId][outputAttr].nodeId = inputProp.Value.Value<int>();
                            if (inputProp.Name == "sInputNodeAttr")
                                inputData[outputNodeId][outputAttr].attrName = inputProp.Value.Value<string>();
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
