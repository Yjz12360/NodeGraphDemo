using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SceneNodeGraph
{
    public class CustomConverterAttribute : Attribute
    {
        public BaseNodeAttrConverter converter;
        public CustomConverterAttribute(Type converterType)
        {
            if (!converterType.IsSubclassOf(typeof(BaseNodeAttrConverter)))
            {
                Debug.LogError("CustomDrawerAttribute Error: not a subtype of JsonConverter");
                return;
            }
            this.converter = (BaseNodeAttrConverter)Activator.CreateInstance(converterType);
        }
    }

    public abstract class BaseNodeAttrConverter : JsonConverter
    {

    }
}