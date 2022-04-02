using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{

    public int dimention = 10;
    public Octave[] octaves;
    public float UVScale = 2f;
    public Transform offset;

    protected MeshFilter meshFilter;
    protected Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.name = gameObject.name;

        mesh.vertices = generateVerts();
        mesh.triangles = generateTris();
        mesh.uv = GenerateUVs();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private Vector3[] generateVerts(){
        var verts = new Vector3[(dimention + 1) * (dimention + 1)];

        for (int x = 0; x <= dimention; x ++){
            for (int z = 0; z <= dimention; z ++){
                 verts[index(x,z)] = new Vector3(x,0,z);
            }
        }

        return verts;
    }

    private int index(int x, int z){
        return x * (dimention + 1) + z;
    }

    private int[] generateTris(){
        var tris = new int[mesh.vertices.Length * 6];

        for (int x = 0; x < dimention; x ++){
            for (int z = 0; z < dimention; z ++){
                tris[index(x,z) * 6 + 0] = index(x,z);
                tris[index(x,z) * 6 + 1] = index(x + 1,z + 1);
                tris[index(x,z) * 6 + 2] = index(x + 1,z);
                tris[index(x,z) * 6 + 3] = index(x,z);
                tris[index(x,z) * 6 + 4] = index(x,z + 1);
                tris[index(x,z) * 6 + 5] = index(x + 1,z + 1);
            }
        }

        return tris;
    }

    private Vector2[] GenerateUVs(){
        var uvs = new Vector2[mesh.vertices.Length];

        for(int x = 0; x <= dimention; x++){
            for(int z = 0; z <= dimention; z++){
                var vec = new Vector2((x/ UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x,z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    // Update is called once per frame
    void Update()
    {
        var verts = mesh.vertices;
        for (int x = 0; x <= dimention; x ++){
            for (int z = 0; z <= dimention; z ++){
                var y = 0f;
                for(int o = 0; o < octaves.Length; o++){
                    if(octaves[o].alternate){
                        var perl = Mathf.PerlinNoise(((offset.position.x + x) * octaves[o].scale.x) / dimention, ((offset.position.z + z)* octaves[o].scale.y) / dimention) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + octaves[o].speed.magnitude * Time.time) * octaves[o].height;
                    }else{
                        var perl = Mathf.PerlinNoise(((offset.position.x + x) * octaves[o].scale.x + Time.time * octaves[o].speed.x) / dimention, ((offset.position.z + z) * octaves[o].scale.y + Time.time * octaves[o].speed.y) / dimention) - 0.5f;
                        y += perl * octaves[o].height;
                    }
                }
                verts[index(x,z)] = new Vector3(x,y,z);
            }
        }
        mesh.vertices = verts;
        mesh.RecalculateNormals();
    }

    [Serializable]
    public struct Octave{
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
