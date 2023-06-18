using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace ZR.Utils.Wpf.Common
{
    public class PropertyResolver
    {
        private enum EditorTypeCode
        {
            PlainText,
            SByteNumber,
            ByteNumber,
            Int16Number,
            UInt16Number,
            Int32Number,
            UInt32Number,
            Int64Number,
            UInt64Number,
            SingleNumber,
            DoubleNumber,
            Switch,
            DateTime,
            HorizontalAlignment,
            VerticalAlignment,
            ImageSource
        }

        private static readonly Dictionary<Type, EditorTypeCode> TypeCodeDic = new Dictionary<Type, EditorTypeCode>
        {
            [typeof(string)] = EditorTypeCode.PlainText,
            [typeof(sbyte)] = EditorTypeCode.SByteNumber,
            [typeof(byte)] = EditorTypeCode.ByteNumber,
            [typeof(short)] = EditorTypeCode.Int16Number,
            [typeof(ushort)] = EditorTypeCode.UInt16Number,
            [typeof(int)] = EditorTypeCode.Int32Number,
            [typeof(uint)] = EditorTypeCode.UInt32Number,
            [typeof(long)] = EditorTypeCode.Int64Number,
            [typeof(ulong)] = EditorTypeCode.UInt64Number,
            [typeof(float)] = EditorTypeCode.SingleNumber,
            [typeof(double)] = EditorTypeCode.DoubleNumber,
            [typeof(bool)] = EditorTypeCode.Switch,
            [typeof(DateTime)] = EditorTypeCode.DateTime,
            [typeof(HorizontalAlignment)] = EditorTypeCode.HorizontalAlignment,
            [typeof(VerticalAlignment)] = EditorTypeCode.VerticalAlignment,
            [typeof(ImageSource)] = EditorTypeCode.ImageSource
        };

        public string ResolveCategory(PropertyDescriptor propertyDescriptor)
        {
            CategoryAttribute categoryAttribute = propertyDescriptor.Attributes.OfType<CategoryAttribute>().FirstOrDefault();
            if (categoryAttribute != null)
            {
                if (!string.IsNullOrEmpty(categoryAttribute.Category))
                {
                    return categoryAttribute.Category;
                }

                return Lang.Miscellaneous;
            }

            return Lang.Miscellaneous;
        }

        public string ResolveDisplayName(PropertyDescriptor propertyDescriptor)
        {
            string text = propertyDescriptor.DisplayName;
            if (string.IsNullOrEmpty(text))
            {
                text = propertyDescriptor.Name;
            }

            return text;
        }

        public string ResolveDescription(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.Description;
        }

        public bool ResolveIsBrowsable(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.IsBrowsable;
        }

        public bool ResolveIsDisplay(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.IsLocalizable;
        }

        public bool ResolveIsReadOnly(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.IsReadOnly;
        }

        public object ResolveDefaultValue(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.Attributes.OfType<DefaultValueAttribute>().FirstOrDefault()?.Value;
        }

        public PropertyEditorBase ResolveEditor(PropertyDescriptor propertyDescriptor)
        {
            EditorAttribute editorAttribute = propertyDescriptor.Attributes.OfType<EditorAttribute>().FirstOrDefault();
            if (editorAttribute != null && !string.IsNullOrEmpty(editorAttribute.EditorTypeName))
            {
                return CreateEditor(Type.GetType(editorAttribute.EditorTypeName));
            }

            return CreateDefaultEditor(propertyDescriptor.PropertyType);
        }

        public virtual PropertyEditorBase CreateDefaultEditor(Type type)
        {
            if (TypeCodeDic.TryGetValue(type, out var value))
            {
                return value switch
                {
                    EditorTypeCode.PlainText => new PlainTextPropertyEditor(),
                    EditorTypeCode.SByteNumber => new NumberPropertyEditor(-128.0, 127.0),
                    EditorTypeCode.ByteNumber => new NumberPropertyEditor(0.0, 255.0),
                    EditorTypeCode.Int16Number => new NumberPropertyEditor(-32768.0, 32767.0),
                    EditorTypeCode.UInt16Number => new NumberPropertyEditor(0.0, 65535.0),
                    EditorTypeCode.Int32Number => new NumberPropertyEditor(-2147483648.0, 2147483647.0),
                    EditorTypeCode.UInt32Number => new NumberPropertyEditor(0.0, 4294967295.0),
                    EditorTypeCode.Int64Number => new NumberPropertyEditor(-9.2233720368547758E+18, 9.2233720368547758E+18),
                    EditorTypeCode.UInt64Number => new NumberPropertyEditor(0.0, 1.8446744073709552E+19),
                    EditorTypeCode.SingleNumber => new NumberPropertyEditor(-3.4028234663852886E+38, 3.4028234663852886E+38),
                    EditorTypeCode.DoubleNumber => new NumberPropertyEditor(double.MinValue, double.MaxValue),
                    EditorTypeCode.Switch => new SwitchPropertyEditor(),
                    EditorTypeCode.DateTime => new DateTimePropertyEditor(),
                    EditorTypeCode.HorizontalAlignment => new HorizontalAlignmentPropertyEditor(),
                    EditorTypeCode.VerticalAlignment => new VerticalAlignmentPropertyEditor(),
                    EditorTypeCode.ImageSource => new ImagePropertyEditor(),
                    _ => new ReadOnlyTextPropertyEditor(),
                };
            }

            return type.IsSubclassOf(typeof(Enum)) ? ((PropertyEditorBase)new EnumPropertyEditor()) : ((PropertyEditorBase)new ReadOnlyTextPropertyEditor());
        }

        public virtual PropertyEditorBase CreateEditor(Type type)
        {
            return (Activator.CreateInstance(type) as PropertyEditorBase) ?? new ReadOnlyTextPropertyEditor();
        }
    }
}
