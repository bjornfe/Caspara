using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Caspara.Values
{
    [DataContract]
    public class Value : IValue
    {
        private object _obj = "";

        [DataMember]
        private object ObjectValue
        {
            get => _obj;
            set
            {
                if (_obj != null)
                {
                    if (!_obj.Equals(value))
                    {
                        _obj = value;
                        Updated?.Invoke(this, value);
                    }
                }
                else
                    _obj = value;

            }
        }

        [DataMember]
        public String ValueType { get; set; } = "System.Object";

        public event EventHandler<object> Updated;

        public object GetValue()
        {
            try
            {
                var newType = Type.GetType(ValueType);
                if (newType != null)
                {
                    return Convert.ChangeType(ObjectValue, Type.GetType(ValueType));
                }
                else
                    return ObjectValue;
            }
            catch
            {
                return ObjectValue;
            }
        }

        public byte[] GetBytes()
        {
            switch (ObjectValue)
            {
                case bool v:
                    return BitConverter.GetBytes(v);
                case char v:
                    return BitConverter.GetBytes(v);
                case short v:
                    return BitConverter.GetBytes(v);
                case int v:
                    return BitConverter.GetBytes(v);
                case long v:
                    return BitConverter.GetBytes(v);
                case ushort v:
                    return BitConverter.GetBytes(v);
                case uint v:
                    return BitConverter.GetBytes(v);
                case ulong v:
                    return BitConverter.GetBytes(v);
                case float v:
                    return BitConverter.GetBytes(v);
                case double v:
                    return BitConverter.GetBytes(v);
                default:
                    return null;
            }

        }

        public void SetFromBytes(byte[] bytes)
        {
            switch (ObjectValue)
            {
                case bool v:
                    ObjectValue = BitConverter.ToBoolean(bytes, 0);
                    break;
                case char v:
                    ObjectValue = BitConverter.ToChar(bytes, 0);
                    break;
                case short v:
                    ObjectValue = BitConverter.ToUInt16(bytes, 0);
                    break;
                case int v:
                    ObjectValue = BitConverter.ToBoolean(bytes, 0);
                    break;
                case long v:
                    ObjectValue = BitConverter.ToBoolean(bytes, 0);
                    break;
                case ushort v:
                    ObjectValue = BitConverter.ToBoolean(bytes, 0);
                    break;
                case uint v:
                    ObjectValue = BitConverter.ToBoolean(bytes, 0);
                    break;
                case ulong v:
                    ObjectValue = BitConverter.ToBoolean(bytes, 0);
                    break;
                case float v:
                    ObjectValue = BitConverter.ToBoolean(bytes, 0);
                    break;
                case double v:
                    ObjectValue = BitConverter.ToBoolean(bytes, 0);
                    break;
            }
        }

        public virtual void SetBit(int bitNr, bool value)
        {
            byte[] originalBytes = GetBytes();
            BitArray ba = new BitArray(originalBytes);
            ba.Set(bitNr, value);
            ba.CopyTo(originalBytes, 0);
            SetValue(originalBytes);
        }

        public virtual bool GetBit(int bitNr)
        {
            byte[] originalBytes = GetBytes();
            BitArray ba = new BitArray(originalBytes);
            return ba.Get(bitNr);
        }

        public Value()
        {

        }

        public Value(object value)
        {
            SetValue(value);
        }

        public IValue SetValue(object Value)
        {
            if (Value != null)
            {
                this.ObjectValue = Value;
                this.ValueType = Value.GetType().ToString();
            }
            return this;
        }

    }
}
