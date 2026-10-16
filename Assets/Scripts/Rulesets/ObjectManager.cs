using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Kralt.Editor.Extensions;
using Kralt.Editor.Graphics.Gizmos;

namespace Kralt.Editor.Rulesets
{
    public class ObjectManager : MonoBehaviour
    {
        public static KraltObject SelectedObject { get; private set; }
        
        public static ObjectManager InstanceData { get; private set; }
        private void Awake() => InstanceData = this;
        
        [Header("Events")]
        [SerializeField] UnityEvent OnObjectSelected;
        
        [SerializeField]
        private Vector3Int[] startBlocks =
        {
            new(0, 0, 0),
            new(1, 0, 0),
            new(0, 0, 1),
            new(-1, 0, 0),
            new(0, 0, -1),
            new(1, 0, 1),
            new(-1, 0, 1),
            new(-1, 0, -1),
            new(1, 0, -1),
        };
        
        private void Start()
        {
            CreateNew(Vector3.zero, startBlocks);
        }

        public void UpdateSelectedObjectOutline()
        {
            // SelectedObject should never actually be null here, but null check just to be safe
            if(SelectedObject == null)
                return;
            
            bool isObjectMode = KraltEditor.ActiveEditorMode == KraltEditor.OBJECT_MODE;
            SelectedObject.SetOutline(isObjectMode ? KraltEditor.InstanceData.objectModeSelectionOutline : KraltEditor.InstanceData.editModeSelectionOutline);
        }
        
        public bool TrySelectObject(Transform obj)
        {
            if(obj.TryGetComponent(out KraltObject c))
            {
                SelectObject(c);
                return true;
            }
            
            return false;
        }
        
        public void SelectObject(KraltObject obj)
        {
            if(ReferenceEquals(obj, SelectedObject))
                return;
            
            if(SelectedObject != null)
                SelectedObject.RemoveOutline();
            
            SelectedObject = obj;
            obj.SetOutline(KraltEditor.InstanceData.objectModeSelectionOutline);

            OnObjectSelected.Invoke();
        }
        
        public void DeselectAll()
        {
            if(SelectedObject == null)
                return;
            
            SelectedObject.RemoveOutline();
            SelectedObject = null;

            OnObjectSelected.Invoke();
        }
        
        public static void CreateNew()
        {
            CreateNew(Vector3.zero);
        }
        
        public static void CreateNew(Vector3 position)
        {
            Vector3Int[] voxels = { Vector3Int.zero };
            CreateNew(position, voxels);
        }
        
        public static void CreateNew(Vector3 position, in Vector3Int[] voxels)
        {
            GameObject newObject = new("Kralt Object");
            
            newObject.transform.parent = InstanceData.transform;
            newObject.layer = KraltEditor.InstanceData.kraltObjectLayer.LayerIndex();
            
            KraltObject objectData = newObject.AddComponent<KraltObject>();
            objectData.voxels = new HashSet<Vector3Int>(voxels);
            objectData.BuildMesh();
        }
        
        public void TransformSelection(Vector3 position)
        {
            SelectedObject.transform.position = position;
        }
    }
}
