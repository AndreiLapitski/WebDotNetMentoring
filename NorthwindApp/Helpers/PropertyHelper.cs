using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NorthwindApp.Attributes;

namespace NorthwindApp.Helpers
{
    public static class PropertyHelper
    {
        public static IList<string> GetDisplayablePropertyNames(Type type)
        {
            return (from propertyInfo in 
                    type
                        .GetProperties()
                        .Where(p => p.GetCustomAttribute<NotDisplayAttribute>() == null)
                        .ToArray()
                let displayName = propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                select string.IsNullOrEmpty(displayName)
                    ? propertyInfo.Name
                    : displayName)
                .ToList();
        }
    }
}
