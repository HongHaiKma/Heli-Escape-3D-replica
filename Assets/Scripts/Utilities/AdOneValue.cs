using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace AdOne
{
    [TypeConverter(typeof(AbiValueTypeConverter))]
    [System.Serializable]
    public struct AbiValue
    {
        public string Value;

        public AbiValue(string v)
        {
            Value = v;
        }

        public AbiValue(float v)
        {
            Value = v.ToString("0.000");
        }
        public AbiValue(double v)
        {
            Value = v.ToString("0.000");
        }

        public AbiValue(int v)
        {
            Value = v.ToString();
        }

        public AbiValue(Vector3 v)
        {
            Value = $"{v.x};{v.y};{v.z}";
        }

        public Vector3 ToVector3()
        {
            List<float> l = Value.ToListFloat();
            return new Vector3(l[0], l[1], l[2]);
        }

        public AbiValue(List<float> v)
        {
            Value = v.ToStringLine();
        }
        public AbiValue(List<int> v)
        {
            Value = v.ToStringLine();
        }
        public AbiValue(List<string> v)
        {
            Value = v.ToStringLine();
        }

        public static implicit operator AbiValue(string v) => new AbiValue(v);
        public static implicit operator string(AbiValue g) => g.Value;

        public static implicit operator AbiValue(double v) => new AbiValue(v);
        public static implicit operator double(AbiValue g) => double.Parse(g.Value);

        public static implicit operator AbiValue(float v) => new AbiValue(v);
        public static implicit operator float(AbiValue g) => float.Parse(g.Value);

        public static implicit operator AbiValue(int v) => new AbiValue(v);
        public static implicit operator int(AbiValue g) => int.Parse(g.Value);

        public static implicit operator AbiValue(Vector3 v) => new AbiValue(v);
        public static implicit operator Vector3(AbiValue g) => g.ToVector3();

        public static implicit operator AbiValue(List<float> v) => new AbiValue(v);
        public static implicit operator List<float>(AbiValue g) => g.Value.ToListFloat();

        public static implicit operator AbiValue(List<int> v) => new AbiValue(v);
        public static implicit operator List<int>(AbiValue g) => g.Value.ToListInt();

        public static implicit operator AbiValue(List<string> v) => new AbiValue(v);
        public static implicit operator List<string>(AbiValue g) => g.Value.ToListString();
        public override string ToString()
        {
            return Value;
        }
    }

    public class AbiValueTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string)
                || sourceType == typeof(float)
                || sourceType == typeof(int)
                || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string)
                || destinationType == typeof(float)
                || destinationType == typeof(int)
                || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var casted = value as string;
            return casted != null
                ? new AbiValue(casted)
                : base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            var casted = (AbiValue)value;
            return destinationType == typeof(string)
                ? casted
                : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
