using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public static class GameEditorHelper
    {
        private static int CompareTrans(Transform x, Transform y)
        {
            int.TryParse(x.name, out int numX);
            int.TryParse(y.name, out int numY);
            return numX.CompareTo(numY);
        }

        public static int GenChildId(Transform container)
        {
            for (int i = 1; i < 10000; ++i)
            {
                Transform child = container.Find(i.ToString());
                if (child == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public static GameObject AddConfig(Transform container)
        {
            int genId = GameEditorHelper.GenChildId(container);
            GameObject configObject = new GameObject(genId.ToString());
            configObject.transform.SetParent(container);
            configObject.transform.position = Vector3.zero;
            return configObject;
        }

        public static Transform GetOrAddChild(Transform container, string childName)
        {
            Transform child = container.Find(childName);
            if (child == null)
            {
                GameObject newObject = new GameObject(childName);
                newObject.transform.parent = container;
                newObject.transform.localPosition = Vector3.zero;
                child = newObject.transform;
            }
            return child;
        }

        public static void TransferTo(Transform child, Transform container)
        {
            if (container.Find(child.name) != null)
            {
                int id = GameEditorHelper.GenChildId(container);
                child.name = id.ToString();
            }
            child.parent = container;
        }

        public static void ReorderChildById(Transform container)
        {
            List<Transform> childs = new List<Transform>();
            for(int i = 0; i < container.childCount; ++i)
            {
                Transform child = container.GetChild(i);
                childs.Add(child);
            }
            foreach (Transform child in childs)
                child.transform.parent = null;
            childs.Sort(CompareTrans);
            foreach (Transform child in childs)
                child.parent = container;
        }
    }
}
