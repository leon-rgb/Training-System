using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexGenerator : MonoBehaviour
{
    Mesh mesh;

    // Use this for initialization
    public ConvexGenerator(Mesh mesh)
    {
        this.mesh = mesh;
    }

    /**
    public Mesh BuildSimplifiedConvexMesh()
    {
        Debug.Log(mesh.triangles.Length / 3 + " tris");

        //SplitMeshBuilder builder = new SplitMeshBuilder();

        for (int i = 0; i < 64; i++)
        {
            int index = Random.Range(0, mesh.triangles.Length / 3) * 3;

            Vector3[] triangle = new Vector3[] { mesh.vertices[mesh.triangles[index]], mesh.vertices[mesh.triangles[index + 1]], mesh.vertices[mesh.triangles[index + 2]] };
            Vector2[] uvs = new Vector2[] { mesh.uv[mesh.triangles[index]], mesh.uv[mesh.triangles[index + 1]], mesh.uv[mesh.triangles[index + 2]] };

            //builder.AddTriangleToMesh(triangle, uvs);
        }

        //Mesh polygonSoup = builder.Build();
        //Debug.Log(polygonSoup.triangles.Length / 3 + " tris");

        return polygonSoup;
    }
    */
}
