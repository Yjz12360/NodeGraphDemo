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

    public static class RequireAttrChecker
    {
        public static bool Check(FieldInfo fieldInfo, object data)
        {
            Type type = data.GetType();
            RequireBoolAttribute requireBool = fieldInfo.GetCustomAttribute<RequireBoolAttribute>();
            if (requireBool != null)
            {
                string requireAttr = requireBool.attrName;
                FieldInfo requireField = type.GetField(requireAttr);
                if (requireField == null)
                    return false;
                object value = requireField.GetValue(data);
                if (value == null || value.GetType() != typeof(bool))
                    return false;
                if ((bool)value != requireBool.value)
                    return false;
            }
            RequireIntAttribute requireInt = fieldInfo.GetCustomAttribute<RequireIntAttribute>();
            if (requireInt != null)
            {
                string requireAttr = requireInt.attrName;
                FieldInfo requireField = type.GetField(requireAttr);
                if (requireField == null)
                    return false;
                object value = requireField.GetValue(data);
                if (value == null || value.GetType() != typeof(int))
                    return false;
                if ((int)value != 0)
                    return false;
            }
            RequireEnumAttribute requireEnum = fieldInfo.GetCustomAttribute<RequireEnumAttribute>();
            if (requireEnum != null)
            {
                string requireAttr = requireEnum.attrName;
                FieldInfo requireField = type.GetField(requireAttr);
                if (requireField == null)
                    return false;
                object value = requireField.GetValue(data);
                if (value == null || !value.GetType().IsEnum)
                    return false;
                if ((int)value != (int)requireEnum.value)
                    return false;
            }
            return true;
        }
    }

}