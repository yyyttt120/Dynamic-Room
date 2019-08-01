using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Functions
{
    static class SomeFunctions
    {

        public static double AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)//return the angle between two vectors
        {
            return Mathf.Atan2(
                Vector3.Dot(n, Vector3.Cross(v1, v2)),
                Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }

        public static List<object> CopyList(List<object> original_list)
        {
            List<object> copid_list = new List<object>();
            foreach (Object obj in original_list)
                copid_list.Add(obj);
            return copid_list;
        }

    }

    
}