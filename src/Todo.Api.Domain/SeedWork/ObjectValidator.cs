using System.Reflection;

namespace Todo.Api.Domain.SeedWork;
public class ObjectValidator
{
    protected virtual bool AllPropertiesAreFilled(object? obj)
    {
        if (obj == null)
            return false;

        PropertyInfo[] properties = obj.GetType().GetProperties();

        foreach (PropertyInfo property in properties)
        {
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                object? value = property.GetValue(obj);

                if (!AllPropertiesAreFilled(value))
                    return false;
            }
            else
            {
                object? value = property.GetValue(obj);

                if (value == null || value is string && string.IsNullOrEmpty((string)value))
                    return false;
            }
        }

        return true;
    }
}

