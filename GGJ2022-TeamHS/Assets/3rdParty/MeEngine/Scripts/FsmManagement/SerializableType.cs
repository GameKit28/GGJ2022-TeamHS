// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using UnityEngine;

namespace TypeReferences
{
    /// <summary>
    /// Reference to a class <see cref="System.Type"/> with support for Unity serialization.
    /// </summary>
    [Serializable]
    public sealed class SerializableType : ISerializationCallbackReceiver
    {

        public static string GetTypeRef(Type type)
        {
            return type != null
                ? type.FullName + ", " + type.Assembly.GetName().Name
                : "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableType"/> class.
        /// </summary>
        public SerializableType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableType"/> class.
        /// </summary>
        /// <param name="assemblyQualifiedClassName">Assembly qualified class name.</param>
        public SerializableType(string assemblyQualifiedClassName)
        {
            Type = !string.IsNullOrEmpty(assemblyQualifiedClassName)
                ? Type.GetType(assemblyQualifiedClassName)
                : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableType"/> class.
        /// </summary>
        /// <param name="type">Class type.</param>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="type"/> is not a class type.
        /// </exception>
        public SerializableType(Type type)
        {
            Type = type;
        }

        [SerializeField]
        private string _typeRef;

        #region ISerializationCallbackReceiver Members

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(_typeRef))
            {
                _type = System.Type.GetType(_typeRef);

                if (_type == null)
                    Debug.LogWarning(string.Format("'{0}' was referenced but class type was not found.", _typeRef));
            }
            else {
                _type = null;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        #endregion

        private Type _type;

        /// <summary>
        /// Gets or sets type of class reference.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="value"/> is not a class type.
        /// </exception>
        public Type Type
        {
            get { return _type; }
            set
            {
                if (value != null && !value.IsClass)
                    throw new ArgumentException(string.Format("'{0}' is not a class type.", value.FullName), "value");

                _type = value;
                _typeRef = GetTypeRef(value);
            }
        }

        public static implicit operator string(SerializableType typeReference)
        {
            return typeReference._typeRef;
        }

        public static implicit operator Type(SerializableType typeReference)
        {
            return typeReference.Type;
        }

        public static implicit operator SerializableType(Type type)
        {
            return new SerializableType(type);
        }

        public override string ToString()
        {
            return Type != null ? Type.FullName : "(None)";
        }
    }
}