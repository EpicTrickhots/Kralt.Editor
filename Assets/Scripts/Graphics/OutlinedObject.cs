// Edited by Aaron Trickhot

//
//  Outline.cs
//  QuickOutline
//
//  Original by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using JetBrains.Annotations;

namespace Kralt.Editor.Graphics.Shaders
{
    [Serializable]
    public struct Outline
    {
        public Color color;
        public float width;
    }
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class OutlinedObject : MonoBehaviour
    {
        protected MeshRenderer objectRenderer;
        protected MeshFilter meshFilter;
        
        private Material outlineMaskMaterial;
        private Material outlineFillMaterial;

        protected virtual void Awake()
        {
            objectRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            
            outlineMaskMaterial = Instantiate(KraltEditor.InstanceData.outlineMaskMaterial);
            outlineFillMaterial = Instantiate(KraltEditor.InstanceData.outlineFillMateria);

            outlineMaskMaterial.name = "OutlineMask (Instance)";
            outlineFillMaterial.name = "OutlineFill (Instance)";
        }
        
        public void SetOutline(in Outline outline)
        {
            List<Material> materials = objectRenderer.sharedMaterials.ToList();
            
            if (materials.Contains(outlineFillMaterial))
            {
                materials.Remove(outlineMaskMaterial);
                materials.Remove(outlineFillMaterial);
            }
            
            UpdateMaterialProperties(outline.color, outline.width);
            
            materials.Add(outlineMaskMaterial);
            materials.Add(outlineFillMaterial);
            
            objectRenderer.materials = materials.ToArray();
        }
        
        public void RemoveOutline()
        {
            List<Material> materials = objectRenderer.sharedMaterials.ToList();
            
            if (materials.Contains(outlineFillMaterial))
            {
                materials.Remove(outlineMaskMaterial);
                materials.Remove(outlineFillMaterial);
            }
            
            objectRenderer.materials = materials.ToArray();
        }
        
        protected virtual void OnDestroy()
        {
            Destroy(outlineMaskMaterial);
            Destroy(outlineFillMaterial);
        }
        
        protected void LoadSmoothNormals()
        {
            var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

            meshFilter.sharedMesh.SetUVs(3, smoothNormals);
            CombineSubmeshes(meshFilter.sharedMesh, objectRenderer.sharedMaterials);
        }

        List<Vector3> SmoothNormals(Mesh mesh)
        {
            var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
            var smoothNormals = new List<Vector3>(mesh.normals);

            foreach (var group in groups)
            {
                if (group.Count() == 1)
                    continue;

                var smoothNormal = Vector3.zero;

                foreach (var pair in group)
                {
                    smoothNormal += smoothNormals[pair.Value];
                }

                smoothNormal.Normalize();

                foreach (var pair in group)
                {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }

        private void CombineSubmeshes(Mesh mesh, Material[] materials)
        {
            if (mesh.subMeshCount == 1 || mesh.subMeshCount > materials.Length)
                return;

            mesh.subMeshCount++;
            mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        }

        private void UpdateMaterialProperties(Color color, float width)
        {
            outlineFillMaterial.SetColor("_OutlineColor", color);
            outlineFillMaterial.SetFloat("_OutlineWidth", width);
            outlineMaskMaterial.SetFloat("_ZTest", (float)CompareFunction.Always);
            outlineFillMaterial.SetFloat("_ZTest", (float)CompareFunction.Always);
        }
    }
}

