using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CommonServices.Utilities
{
    public static class EnumUtility
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string GetEnumDescription<T>(T value)
        {
            FieldInfo fieldInfo = typeof(T).GetField(value.ToString());

            if (fieldInfo.GetCustomAttribute(typeof(DisplayAttribute), false) is
                DisplayAttribute descriptionAttribute)
                return descriptionAttribute.Description;
            else
                return value.ToString();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static T GetValueFromDescription<T>(this string description, bool ignoreCase = true, bool ignoreSpace = true)
        {
            if (!string.IsNullOrEmpty(description))
            {
                foreach (var field in typeof(T).GetFields())
                {
                    if (field.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute attribute)
                    {
                        string attributeDescription = ignoreCase ?
                            ignoreSpace ?
                            attribute.Description.Replace(" ", string.Empty).ToLower() :
                            attribute.Description.ToLower() :
                            ignoreSpace ?
                            attribute.Description.Replace(" ", string.Empty) :
                            attribute.Description;

                        description = ignoreCase ?
                            ignoreSpace ?
                            description.Replace(" ", string.Empty).ToLower() :
                            description.ToLower() :
                            ignoreSpace ?
                            description.Replace(" ", string.Empty) :
                            description;

                        if (attributeDescription == description)
                            return (T)field.GetValue(null);
                    }
                    else
                    {
                        if (field.Name == description)
                            return (T)field.GetValue(null);
                    }
                }
            }
            return default(T);
        }

        public static IEnumerable<string> GetDescriptions(Type type)
        {
            List<string> descriptions = new List<string>();
            string[] names = Enum.GetNames(type);
            foreach (string name in names)
            {
                FieldInfo field = type.GetField(name);
                IEnumerable<Attribute> descriptionAttributes = field.GetCustomAttributes(typeof(DisplayAttribute), true);
                foreach (DisplayAttribute descriptionAttribute in descriptionAttributes)
                {
                    descriptions.Add(descriptionAttribute.Description);
                }
            }
            return descriptions;
        }

        public static IEnumerable<string> GetDescriptionsList(Type type)
        {
            List<string> descriptions = new List<string>();
            string[] names = Enum.GetNames(type);
            foreach (var name in names)
            {
                FieldInfo field = type.GetField(name);
                IEnumerable<Attribute> descriptionAttributes = field.GetCustomAttributes(typeof(DisplayAttribute), true);
                foreach (DisplayAttribute fd in descriptionAttributes)
                {
                    descriptions.Add(fd.Description);
                }
            }
            return descriptions;
        }
    }
}
