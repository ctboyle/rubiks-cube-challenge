using System;
using System.Reflection;

namespace System
{
    static class EnumHelper
    {
        public static Array GetValues(Type enumType)
        {
            if (enumType.BaseType == typeof(System.Enum))
            {
                FieldInfo[] infos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
                Enum[] values = new Enum[infos.Length];

                for (int i = 0; i < infos.Length; ++i)
                    values[i] = (Enum)infos[i].GetValue(null);

                return values;
            }
            else
            {
                throw new ArgumentException("enumType parameter is not a System.Enum");
            }
        }
    }
}