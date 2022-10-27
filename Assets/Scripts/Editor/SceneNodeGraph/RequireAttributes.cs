using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace SceneNodeGraph
{
    public class RequireBoolAttribute : Attribute
    {
        public string attrName;
        public bool value;
        public RequireBoolAttribute(string attrName, bool value)
        {
            this.attrName = attrName;
            this.value = value;
        }
    }

    public class RequireIntAttribute : Attribute
    {
        public string attrName;
        public int value;

        public RequireIntAttribute(string attrName, int value)
        {
            this.attrName = attrName;
            this.value = value;
        }
    }

    public class RequireEnumAttribute : Attribute
    {
        public string attrName;
        public object value;
        public RequireEnumAttribute(string attrName, object value)
        {
            this.attrName = attrName;
            this.value = value;
        }
    }

}