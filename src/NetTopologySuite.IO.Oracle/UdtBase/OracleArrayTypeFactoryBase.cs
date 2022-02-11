using System;
using Oracle.ManagedDataAccess.Types;

namespace NetTopologySuite.IO.UdtBase
{
#pragma warning disable 1591
    public abstract class OracleArrayTypeFactoryBase<T> : IOracleArrayTypeFactory
    {
        public Array CreateArray(int numElems) => new T[numElems];

        public Array CreateStatusArray(int numElems) => null;
    }
#pragma warning restore 1591
}
