﻿using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Codebelt.Extensions.YamlDotNet.Converters
{
    /// <summary>
    /// Converts an object to and from YAML (YAML ain't markup language).
    /// </summary>
    public abstract class YamlConverter : IYamlTypeConverter
    {
        internal abstract void WriteYamlCore(IEmitter writer, object value, ObjectSerializer serializer);

        internal abstract object ReadYamlCore(IParser reader, Type typeToConvert, ObjectDeserializer deserializer);

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The <seealso cref="Type"/> of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public abstract bool CanConvert(Type typeToConvert);

        /// <summary>
        /// Gets or sets the <see cref="Formatters.YamlFormatter"/> associated with this instance.
        /// </summary>
        /// <value>The <see cref="Formatters.YamlFormatter"/> associated with this instance.</value>
        public Formatters.YamlFormatter Formatter { get; set; }

        bool IYamlTypeConverter.Accepts(Type type)
        {
            return CanConvert(type);
        }

        object IYamlTypeConverter.ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            return ReadYamlCore(parser, type, rootDeserializer);
        }

        void IYamlTypeConverter.WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
        {
            WriteYamlCore(emitter, value, serializer);
        }
    }

    /// <summary>
    /// Converts an object to or from YAML (YAML ain't markup language).
    /// </summary>
    /// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
    /// <seealso cref="YamlConverter" />
    public abstract class YamlConverter<T> : YamlConverter
    {
        /// <summary>
        /// Writes a specified <paramref name="value"/> as YAML.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to YAML.</param>
        public abstract void WriteYaml(IEmitter writer, T value);

        /// <summary>
        /// Reads and converts the YAML to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <returns>The converted value.</returns>
        public abstract T ReadYaml(IParser reader, Type typeToConvert);

        internal override object ReadYamlCore(IParser reader, Type typeToConvert, ObjectDeserializer deserializer)
        {
            return ReadYaml(reader, typeToConvert);
        }

        internal override void WriteYamlCore(IEmitter writer, object value, ObjectSerializer serializer)
        {
            WriteYaml(writer, (T)value);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The <seealso cref="Type" /> of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(T).IsAssignableFrom(typeToConvert);
        }
    }
}
