using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    static Matrix4x4 CreateBasis(Vector3 n, Vector3 t, Vector3 r)
    {
        return new Matrix4x4(n, t, r, new Vector4(0, 0, 0, 1));
    }
    // devolver las coordenadas del vector v en la base {n, t, r} (vectores columna)
    static Vector3 BasisChangeWorldToLocal(Vector3 v, Matrix4x4 basis)
    {
        Matrix4x4 inv = basis.inverse;
        return inv.MultiplyPoint3x4(v);
    }
    static Vector3 BasisChangeLocalToWorld(Vector3 v, Matrix4x4 basis)
    {
        return basis.MultiplyPoint3x4(v);
    }
}
