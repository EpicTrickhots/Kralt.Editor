using System.Collections.Generic;
using Kralt.Editor.Rulesets;
using UnityEngine;

namespace Kralt.Editor.Graphics.Gizmos
{
    public class KraltGizmoManager : MonoBehaviour
    {
        [Header("Refernces")]
        [SerializeField] GameObject transformGizmo;
        
        public static KraltGizmoManager InstanceData { get; private set; }
        private void Awake() => InstanceData = this;
        
        public void UpdateTransformGizmo()
        {
            transformGizmo.SetActive(!(ObjectManager.SelectedObject == null || KraltEditor.ActiveEditorMode == KraltEditor.EDIT_MODE));
        }
    }
}
