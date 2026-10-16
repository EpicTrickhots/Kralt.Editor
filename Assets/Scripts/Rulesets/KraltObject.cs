using System.Collections.Generic;
using UnityEngine;
using Kralt.Editor.Graphics.Shaders;

namespace Kralt.Editor.Rulesets
{
    [RequireComponent(typeof(MeshCollider))]
    public class KraltObject : OutlinedObject
    {
        public HashSet<Vector3Int> voxels;
        
        private readonly List<Vector3> vertices = new();
        private readonly List<int> triangles = new();
        private readonly List<Vector2> uvs = new();
        
        private Mesh objectMesh;
        private MeshCollider collision;
        
        protected override void Awake()
        {
            base.Awake();
            collision = GetComponent<MeshCollider>();
            objectMesh = new();
            meshFilter.mesh = objectMesh;
        }

        public void BuildMesh()
        {
            ClearMesh();

            foreach (Vector3Int block in voxels)
            {
                foreach (FaceData face in faces)
                {
                    if (voxels.Contains(block + face.Direction)) // Block exists on this side, skip the face
                        continue;

                    int start = vertices.Count;

                    for (int i = 0; i < 4; i++)
                    {
                        vertices.Add(block + face.Vertices[i]);
                    }

                    triangles.Add(start + 0);
                    triangles.Add(start + 1);
                    triangles.Add(start + 2);

                    triangles.Add(start + 0);
                    triangles.Add(start + 2);
                    triangles.Add(start + 3);

                    uvs.Add(new(0, 0));
                    uvs.Add(new(0, 1));
                    uvs.Add(new(1, 1));
                    uvs.Add(new(1, 0));
                }
            }

            objectMesh.SetVertices(vertices);
            objectMesh.SetTriangles(triangles, 0);
            objectMesh.SetUVs(0, uvs);
            objectMesh.RecalculateNormals();
            
            // collision.sharedMesh = null;
            collision.sharedMesh = objectMesh;
            
            if (objectRenderer.sharedMaterials[0] == null)
                objectRenderer.sharedMaterial = KraltEditor.InstanceData.defaultEditorObjectMaterial;
            
            LoadSmoothNormals();
        }

        private void ClearMesh()
        {
            objectMesh.Clear();
            vertices.Clear();
            triangles.Clear();
            uvs.Clear();
        }
        
        public void AddVoxel(Vector3Int voxel)
        {
            voxels.Add(voxel);
            BuildMesh();
        }

        public void AddVoxels(Vector3Int[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                voxels.Add(v[i]);
            }

            BuildMesh();
        }

        public void RemoveVoxel(Vector3Int v)
        {
            voxels.Remove(v);
            BuildMesh();
        }

        public void RemoveVoxels(Vector3Int[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                voxels.Remove(v[i]);
            }

            BuildMesh();
        }
        
#pragma warning disable IDE0090
        
        private static readonly FaceData[] faces = new FaceData[6]
        {
            new(Vector3Int.right, new Vector3[4]
            {
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f)
            }),
            new(Vector3Int.left, new Vector3[4]
            {
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f)
            }),
            new(Vector3Int.up, new Vector3[4]
            {
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f)
            }),
            new(Vector3Int.down, new Vector3[4]
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f)
            }),
            new(Vector3Int.forward, new Vector3[4]
            {
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f)
            }),
            new(Vector3Int.back, new Vector3[4]
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f)
            })
        };

#pragma warning restore IDE0090

        public readonly struct FaceData
        {
            public Vector3Int Direction { get; }
            public Vector3[] Vertices { get; }

            public FaceData(Vector3Int direction, Vector3[] verticies)
            {
                Direction = direction;
                Vertices = verticies;
            }
        }
    }
}
