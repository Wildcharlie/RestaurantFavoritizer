using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RestaurantFavoritizer
{
    public static class EnumNinja
    {
        public static string DisplayName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DisplayAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute))
                        as DisplayAttribute;

            return attribute == null ? value.ToString() : attribute.Name;
        }
    }
}