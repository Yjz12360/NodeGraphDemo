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
                    int.TryParse(child.name, out int refreshId);
                    ConfigId configId = child.GetComponent<ConfigId>();
                    if (configId != null)
                    {
                        writer.WritePropertyName(refreshId.ToString());
                        writer.WriteStartObject();
                        writer.WritePropertyName("nRefreshId");
                        writer.WriteValue(refreshId);

                        writer.WritePropertyName("nMonsterCfgId");
                        writer.WriteValue(configId.ID);
                        
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
                    int.TryParse(child.name, out int groupId);
                    MonsterGroupConfig monsterGroupConfig = child.GetComponent<MonsterGroupConfig>();
                    if(monsterGroupConfig != null)
                    {
                        writer.WritePropertyName(groupId.ToString());
                        writer.WriteStartObject();
                        writer.WritePropertyName("nGroupId");
                        writer.WriteValue(groupId);

                        writer.WritePropertyName("tRefreshMonsters");
                        writer.WriteStartArray();
                        foreach (GameObject refreshObject in monsterGroupConfig.monsters)
                        {
                            int.TryParse(refreshObject.name, out int refreshId);
                            writer.WriteValue(refreshId);
                        }
                        writer.WriteEndArray();
                        writer.WriteEndObject();
                    }

                }
                writer.WriteEndObject();
            }

            Transform positionTrans = configObject.transform.Find("Position");
            if(positionTrans != null)
            {
                List<Transform> positionChilds = new List<Transform>();
                for (int i = 0; i < positionTrans.childCount; ++i)
                    positionChilds.Add(positionTrans.GetChild(i));
                positionChilds.Sort(CompareTrans);
                writer.WritePropertyName("tPositions");
                writer.WriteStartObject();
                for (int i = 0; i < positionChilds.Count; ++i)
                {
                    Transform positionChild = positionChilds[i];
                    writer.WritePropertyName((i + 1).ToString());
                    writer.WriteStartObject();
                    writer.WritePropertyName("x");
                    writer.WriteValue(positionChild.position.x);
                    writer.WritePropertyName("y");
                    writer.WriteValue(positionChild.position.y);
                    writer.WritePropertyName("z");
                    writer.WriteValue(positionChild.position.z);
                    writer.WriteEndObject();
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
                    int refreshId = property.Value["nRefreshId"].Value<int>();
                    GameObject child = new GameObject(refreshId.ToString());
                    monsterChilds.Add(child.transform);
                    int monsterCfgId = property.Value["nMonsterCfgId"].Value<int>();
                    ConfigId configId = child.AddComponent<ConfigId>();
                    configId.ID = monsterCfgId;
                    child.AddComponent<PathEditorData>();
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
                        pathObject.transform.localPosition = Vector3.zero;
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
                    int groupId = property.Value["nGroupId"].Value<int>();
                    GameObject child = new GameObject(groupId.ToString());
                    monsterGroupChilds.Add(child.transform);
                    MonsterGroupConfig monsterGroupConfig = child.AddComponent<MonsterGroupConfig>();

                    JToken monstersToken = property.Value["tRefreshMonsters"];
                    foreach (JProperty monsterToken in monstersToken.Children())
                    {
                        int refreshId = monsterToken.Value.Value<int>();
                        Transform monsterTrans = monsterRoot.transform.Find(refreshId.ToString());
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


            GameObject positionRoot = new GameObject("Position");
            positionRoot.transform.SetParent(configObject.transform);
            List<Transform> positionChilds = new List<Transform>();
            if (jsonObject.TryGetValue("tPositions", out JToken positionsToken))
            {
                foreach (JProperty property in positionsToken.Children())
                {
                    string positionId = property.Name;
                    GameObject child = new GameObject(positionId);
                    Vector3 position = Vector3.zero;
                    foreach (JProperty posProp in property.Value.Children())
                    {
                        if (posProp.Name == "x")
                            position.x = posProp.Value.Value<float>();
                        if (posProp.Name == "y")
                            position.y = posProp.Value.Value<float>();
                        if (posProp.Name == "z")
                            position.z = posProp.Value.Value<float>();
                    }
                    child.transform.position = position;
                    positionChilds.Add(child.transform);
                }
            }
            positionChilds.Sort(CompareTrans);
            foreach (Transform child in positionChilds)
                child.parent = positionRoot.transform;


            return configObject;
        }
    }
}
