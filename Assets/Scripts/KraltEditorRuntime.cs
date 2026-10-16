using System;
using UnityEngine;
using UnityEngine.Events;
using Kralt.Editor.Graphics.Shaders;

namespace Kralt.Editor
{
    public sealed class KraltEditor : MonoBehaviour
    {
        public const int OBJECT_MODE = 0;
        public const int EDIT_MODE = 1;
        
        [Header("Materials")]
        public Material defaultEditorObjectMaterial;
        public Material outlineMaskMaterial;
        public Material outlineFillMateria;
        // public KraltMaterial[] kraltMaterials;
        
        [Header("Layers")]
        public LayerMask kraltObjectLayer;
        public LayerMask kraltGizmoLayer;
        
        [Header("Outline Colors")]
        public Outline objectModeSelectionOutline;
        public Outline editModeSelectionOutline;
        
        [Header("Events")]
        public UnityEvent OnEditModeChanged;
        
        public static int ActiveEditorMode { get; private set; } = OBJECT_MODE;
        
        public static bool IsEditMode => ActiveEditorMode == EDIT_MODE;
        public static bool IsObjectMode => ActiveEditorMode == OBJECT_MODE;

        public static KraltEditor InstanceData { get; private set; }
        private void Awake() => InstanceData = this;

        public void ToggleEditorMode()
        {
            ActiveEditorMode = 1 - ActiveEditorMode;
            OnEditModeChanged.Invoke();
        }
    }
}
