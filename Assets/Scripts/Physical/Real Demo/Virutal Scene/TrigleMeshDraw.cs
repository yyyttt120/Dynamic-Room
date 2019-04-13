using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MeshDrawTriangle : MonoBehaviour
{
    public float sideLength = 2;
    public float angleDegree = 100;
    private static readonly int ANGLE_DEGREE_PRECISION = 1000;
    private static readonly int SIDE_LENGTH_PRECISION = 1000;

    private MeshFilter meshFilter;

    private TriangleMeshCreator creator = new TriangleMeshCreator();

    [ExecuteInEditMode]
    private void Awake()
    {

        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        meshFilter.mesh = creator.CreateMesh(sideLength, angleDegree);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        DrawMesh();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        DrawMesh();
    }

    private void DrawMesh()
    {
        Mesh mesh = creator.CreateMesh(sideLength, angleDegree);
        int[] tris = mesh.triangles;
        Gizmos.DrawLine(transformToWorld(mesh.vertices[tris[0]]), transformToWorld(mesh.vertices[tris[1]]));
        Gizmos.DrawLine(transformToWorld(mesh.vertices[tris[0]]), transformToWorld(mesh.vertices[tris[2]]));
        Gizmos.DrawLine(transformToWorld(mesh.vertices[tris[1]]), transformToWorld(mesh.vertices[tris[2]]));
    }

    private Vector3 transformToWorld(Vector3 src)
    {
        return transform.TransformPoint(src);
    }

    private class TriangleMeshCreator
    {
        private float _sideLength;
        private float _angleDegree;

        private Mesh _cacheMesh;
        public Mesh CreateMesh(float sideLength, float angleDegree)
        {
            if (checkDiff(sideLength, angleDegree))
            {
                Mesh newMesh = Create(sideLength, angleDegree);
                if (newMesh != null)
                {
                    _cacheMesh = newMesh;
                    this._sideLength = sideLength;
                    this._angleDegree = angleDegree;
                }
            }
            return _cacheMesh;
        }

        private Mesh Create(float sideLength, float angleDegree)
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[3];

            float angle = Mathf.Deg2Rad * angleDegree;
            float halfAngle = angle / 2;
            vertices[0] = Vector3.zero;
            float cosA = Mathf.Cos(halfAngle);
            float sinA = Mathf.Sin(halfAngle);
            vertices[1] = new Vector3(cosA * sideLength, 0, sinA * sideLength);
            vertices[2] = new Vector3(cosA * sideLength, 0, -sinA * sideLength);

            int[] triangles = new int[3];
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            Vector2[] uvs = new Vector2[vertices.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = Vector2.zero;
            }
            mesh.uv = uvs;

            return mesh;
        }

        private bool checkDiff(float sideLength, float angleDegree)
        {
            return (int)((sideLength - this._sideLength) * SIDE_LENGTH_PRECISION) != 0 ||
                (int)((angleDegree - this._angleDegree) * ANGLE_DEGREE_PRECISION) != 0;
        }
    }

}