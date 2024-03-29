﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Draw.src.Model
{
    [Serializable]
    class CustomMatrix : ISerializable
    {
        public CustomMatrix() {}

        public CustomMatrix(Matrix matrix)
        {
            this.matrix = matrix;
        }

        [NonSerialized]
        public Matrix matrix = new Matrix();

        public CustomMatrix(SerializationInfo info, StreamingContext context)
        {
            float[] m = (float[])info.GetValue("matrix", typeof(float[]));
            matrix = new Matrix(m[0], m[1], m[2], m[3], m[4], m[5]);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("matrix", matrix.Elements, typeof(float[]));
        }
    }
}
