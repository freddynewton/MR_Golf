using UnityEngine;
using System.Collections.Generic;

public class SQEMColliderApproximator : MonoBehaviour
{
    public MeshFilter targetMesh;
    public int numSpheres = 10;
    public float maxRadius = 10f;

    void Start()
    {
        ApproximateColliders();
    }

    void ApproximateColliders()
    {
        if (targetMesh == null || targetMesh.sharedMesh == null)
        {
            Debug.LogError("Target mesh is not assigned or invalid.");
            return;
        }

        Mesh mesh = targetMesh.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        List<SQEM> sqems = new List<SQEM>();
        for (int i = 0; i < numSpheres; i++)
        {
            sqems.Add(new SQEM());
        }

        // Step 1: Initialize SQEMs with mesh triangles
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v1 = vertices[triangles[i]];
            Vector3 v2 = vertices[triangles[i + 1]];
            Vector3 v3 = vertices[triangles[i + 2]];

            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).normalized;
            Vector3 center = (v1 + v2 + v3) / 3f;

            int closestSQEM = 0;
            float minDist = float.MaxValue;
            for (int j = 0; j < numSpheres; j++)
            {
                float dist = Vector3.Distance(center, sqems[j].GetCenter());
                if (dist < minDist)
                {
                    minDist = dist;
                    closestSQEM = j;
                }
            }

            sqems[closestSQEM].AddPlane(center, normal);
        }

        // Step 2: Optimize spheres
        for (int i = 0; i < numSpheres; i++)
        {
            Vector3 sphereCenter;
            float sphereRadius;
            Vector3 min = Vector3.one * -maxRadius;
            Vector3 max = Vector3.one * maxRadius;

            sqems[i].Minimize(out sphereCenter, out sphereRadius, min, max);

            // Create sphere collider
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = sphereCenter;
            sphere.transform.localScale = Vector3.one * (sphereRadius * 2);
            sphere.transform.SetParent(transform);

            // Remove mesh renderer and keep only the collider
            Destroy(sphere.GetComponent<MeshRenderer>());
            Destroy(sphere.GetComponent<MeshFilter>());
        }
    }
}

public class SQEM
{
    private Matrix4x4 A;
    private Vector4 b;
    private float c;

    public SQEM()
    {
        A = Matrix4x4.zero;
        b = Vector4.zero;
        c = 0;
    }

    public void AddPlane(Vector3 point, Vector3 normal)
    {
        Vector4 p = new Vector4(point.x, point.y, point.z, 1);
        Vector4 n = new Vector4(normal.x, normal.y, normal.z, 0);

        A = AddMatrices(A, OuterProduct(n, n));
        b += Vector4.Dot(p, n) * n;
        c += Vector4.Dot(p, n) * Vector4.Dot(p, n);
    }

    private Matrix4x4 AddMatrices(Matrix4x4 a, Matrix4x4 b)
    {
        Matrix4x4 result = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result[i, j] = a[i, j] + b[i, j];
            }
        }
        return result;
    }

    public void Minimize(out Vector3 center, out float radius, Vector3 min, Vector3 max)
    {
        Matrix4x4 invA = A.inverse;
        Vector4 x = invA * b;

        center = new Vector3(x.x, x.y, x.z);
        radius = Mathf.Sqrt(c - Vector4.Dot(b, x));

        // Clamp center within bounds
        center.x = Mathf.Clamp(center.x, min.x, max.x);
        center.y = Mathf.Clamp(center.y, min.y, max.y);
        center.z = Mathf.Clamp(center.z, min.z, max.z);
    }

    public Vector3 GetCenter()
    {
        Vector4 x = A.inverse * b;
        return new Vector3(x.x, x.y, x.z);
    }

    private Matrix4x4 OuterProduct(Vector4 a, Vector4 b)
    {
        Matrix4x4 result = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result[i, j] = a[i] * b[j];
            }
        }
        return result;
    }
}
