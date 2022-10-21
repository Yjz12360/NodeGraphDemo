using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Game
{
    public class GameConfigConverter : JsonConverter
    {
        public static GameConfigConverter converter = new GameConfigConverter();
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GameObject) || objectType.IsSubclassOf(typeof(GameObject)); ;
        }

        private int CompareTrans(Transform x, Transform y)
        {
            int.TryParse(x.name, out int numX);
            int.TryParse(y.name, out int numY);
            return numX.CompareTo(numY);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            GameObject configObject = (GameObject)value;
            writer.WriteStartObject();

            writer.WritePropertyName("tRefreshMonsters");
            writer.WriteStartObject();
            Transform monsterTrans = configObject.transform.Find("Monster");
            if(monsterTrans != null)
            {
                List<Transform> monsterChilds = new List<Transform>();
                for (int i = 0; i < monsterTrans.childCount; ++i)
                    monsterChilds.Add(monsterTrans.GetChild(i));
                monsterChilds.Sort(CompareTrans);
                foreach (Transform child in monsterChilds)
                {
                    string refreshId = child.name;
                    ConfigId configId = child.GetComponent<ConfigId>();
                    if (configId != null)
                    {
                        writer.WritePropertyName(refreshId);
                        writer.WriteStartObject();
                        writer.WritePropertyName("sRefreshId");
                        writer.WriteValue(refreshId);

                        writer.WritePropertyName("nMonsterCfgId");
                        writer.WriteValue(configId.nConfigId);
                        
                        writer.WritePropertyName("tPos");
                        writer.WriteStartObject();
                        writer.WritePropertyName("x");
                        writer.WriteValue(child.position.x);
                        writer.WritePropertyName("y");
                        writer.WriteValue(child.position.y);
                        writer.WritePropertyName("z");
                        writer.WriteValue(child.position.z);
                        writer.WriteEndObject();

                        Transform pathTrans = child.Find("Path");
                        if (pathTrans != null)
                        {
                            writer.WritePropertyName("tPath");
                            writer.WriteStartObject();
                            List<Transform> pathChilds = new List<Transform>();
                            for (int i = 0; i < pathTrans.childCount; ++i)
                                pathChilds.Add(pathTrans.GetChild(i));
                            pathChilds.Sort(CompareTrans);
                            for (int i = 0; i < pathChilds.Count; ++i)
                            {
                                Transform pathChild = pathChilds[i];
                                writer.WritePropertyName((i + 1).ToString());
                                writer.WriteStartObject();
                                writer.WritePropertyName("x");
                                writer.WriteValue(pathChild.position.x);
                                writer.WritePropertyName("y");
                                writer.WriteValue(pathChild.position.y);
                                writer.WritePropertyName("z");
                                writer.WriteValue(pathChild.position.z);
                                writer.WriteEndObject();
                            }
                            writer.WriteEndObject();
                        }
                        writer.WriteEndObject();
                    }
                    
                }
                writer.WriteEndObject();
            }

            Transform monsterGroupTrans = configObject.transform.Find("MonsterGroup");
            if (monsterGroupTrans != null)
            {
                List<Transform> monsterGroupChilds = new List<Transform>();
                for (int i = 0; i < monsterGroupTrans.childCount; ++i)
                    monsterGroupChilds.Add(monsterGroupTrans.GetChild(i));
                monsterGroupChilds.Sort(CompareTrans);
                writer.WritePropertyName("tRefreshMonsterGroups");
                writer.WriteStartObject();
                foreach(Transform child in monsterGroupChilds)
                {
                    string groupId = child.name;
                    MonsterGroupConfig monsterGroupConfig = child.GetComponent<MonsterGroupConfig>();
                    if(monsterGroupConfig != null)
                    {
                        writer.WritePropertyName(groupId);
                        writer.WriteStartObject();
                        writer.WritePropertyName("sGroupId");
                        writer.WriteValue(groupId);

                        writer.WritePropertyName("tRefreshMonsters");
                        writer.WriteStartArray();
                        foreach (GameObject refreshObject in monsterGroupConfig.monsters)
                        {
                            string refreshId = refreshObject.name;
                            writer.WriteValue(refreshId);
                        }
                        writer.WriteEndArray();
                        writer.WriteEndObject();
                    }

                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jsonObject = (JObject)JToken.ReadFrom(reader);

            GameObject configObject = new GameObject("Config");
            GameObject monsterRoot = new GameObject("Monster");
            monsterRoot.transform.SetParent(configObject.transform);
            List<Transform> monsterChilds = new List<Transform>();
            if (jsonObject.TryGetValue("tRefreshMonsters", out JToken refreshMonsterToken))
            {
                foreach (JProperty property in refreshMonsterToken.Children())
                {
                    string refreshId = property.Value["sRefreshId"].Value<string>();
                    GameObject child = new GameObject(refreshId);
                    monsterChilds.Add(child.transform);
                    int monsterCfgId = property.Value["nMonsterCfgId"].Value<int>();
                    ConfigId configId = child.AddComponent<ConfigId>();
                    configId.nConfigId = monsterCfgId;
                    JToken positionToken = property.Value["tPos"];
                    if(positionToken != null)
                    {
                        Vector3 position = Vector3.zero;
                        foreach(JProperty posProp in positionToken.Children())
                        {
                            if (posProp.Name == "x")
                                position.x = posProp.Value.Value<float>();
                            if (posProp.Name == "y")
                                position.y = posProp.Value.Value<float>();
                            if (posProp.Name == "z")
                                position.z = posProp.Value.Value<float>();
                        }
                        child.transform.position = position;
                    }

                    JToken pathsToken = property.Value["tPath"];
                    if(pathsToken != null)
                    {
                        GameObject pathObject = new GameObject("Path");
                        pathObject.transform.parent = child.transform;
                        List<Transform> pathChilds = new List<Transform>();
                        foreach (JProperty pathsProp in pathsToken.Children())
                        {
                            Vector3 pathPos = Vector3.zero;
                            foreach (JProperty pathProp in pathsProp.Value.Children())
                            {
                                if (pathProp.Name == "x")
                                    pathPos.x = pathProp.Value.Value<float>();
                                if (pathProp.Name == "y")
                                    pathPos.y = pathProp.Value.Value<float>();
                                if (pathProp.Name == "z")
                                    pathPos.z = pathProp.Value.Value<float>();
                            }
                            GameObject pathChildObj = new GameObject(pathsProp.Name);
                            pathChildObj.transform.position = pathPos;
                            pathChilds.Add(pathChildObj.transform);
                        }
                        pathChilds.Sort(CompareTrans);
                        foreach (Transform pathChild in pathChilds)
                            pathChild.parent = pathObject.transform;
                    }
                }
            }
            monsterChilds.Sort(CompareTrans);
            foreach (Transform child in monsterChilds)
                child.parent = monsterRoot.transform;

            GameObject monsterGroupRoot = new GameObject("MonsterGroup");
            monsterGroupRoot.transform.SetParent(configObject.transform);
            List<Transform> monsterGroupChilds = new List<Transform>();
            if (jsonObject.TryGetValue("tRefreshMonsterGroups", out JToken refreshMonsterGroupToken))
            {
                foreach (JProperty property in refreshMonsterGroupToken.Children())
                {
                    string groupId = property.Value["sGroupId"].Value<string>();
                    GameObject child = new GameObject(groupId);
                    monsterGroupChilds.Add(child.transform);
                    MonsterGroupConfig monsterGroupConfig = child.AddComponent<MonsterGroupConfig>();

                    JToken monstersToken = property.Value["tRefreshMonsters"];
                    foreach (JToken monsterToken in monstersToken.Children())
                    {
                        string refreshId = monsterToken.Value<string>();
                        Transform monsterTrans = monsterRoot.transform.Find(refreshId);
                        if(monsterTrans != null)
                        {
                            monsterGroupConfig.monsters.Add(monsterTrans.gameObject);
                        }
                    }
                }
            }
            monsterGroupChilds.Sort(CompareTrans);
            foreach (Transform child in monsterGroupChilds)
                child.parent = monsterGroupRoot.transform;

            return configObject;
        }
    }
}
