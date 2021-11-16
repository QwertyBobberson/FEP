using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int size;

    public int octaves;
    public float scale;
    public float lacunarity;
    public float persistance;

    public float maxHeight;
    public float minHeight;

    public float height;

    public int seed;

    public Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        maxHeight = float.MinValue;
        minHeight = float.MaxValue;

        mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;

        GenerateChunk(offset.x + transform.position.x, offset.y + transform.position.z);
        UpdateMesh();
        Debug.Log(minHeight);
    }

    int i = 0;

    void Update()
    {
        if(i >= 600)
        {
            GenerateChunk(offset.x + transform.position.x, offset.y + transform.position.z);
            UpdateMesh();
            i = 0;
        }
        i++;
    }

    // Update is called once per frame
    void GenerateChunk(float _x, float _z)
    {
        vertices = new Vector3[(size + 1) * (size + 1)];

        for(int i = 0, z = 0; z <= size; z++)
        {
            for(int x = 0; x <= size; x++)
            {
                float y = 0;

                float amplitude = 1;
                float frequency = 1;

                for(int j = 0; j < octaves; j++)
                {
                    y += Mathf.PerlinNoise((float)(x + _x) / scale * frequency, (float)(z + _z) / scale * frequency) * amplitude * height;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                y = Mathf.Pow(y, 2);

                if(y > maxHeight)
                {
                    maxHeight = y;
                }
                if(y < minHeight)
                {
                    minHeight = y;
                }
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[size * size * 6];

        int vert = 0;
        int triNum = 0;
        for(int z = 0; z < size; z++)
        {
            for(int x = 0; x < size; x++)
            {
                triangles[triNum] = vert + 0;
                triangles[triNum + 1] = vert + size + 1;
                triangles[triNum + 2] = vert + 1;
                triangles[triNum + 3] = vert + 1;
                triangles[triNum + 4] = vert + size + 1;
                triangles[triNum + 5] = vert + size + 2;
                
                vert++;
                triNum += 6;
            }
            vert++;
        }

        for(int i = 0, z = 0; z <= size; z++)
        {
            for(int x = 0; x <= size; x++)
            {
                vertices[i].y -= minHeight;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
