using UnityEngine;
using UnityEditor;

namespace Kralt.Editor.Graphics.UserInterface
{
    [CustomEditor(typeof(CameraViewport))]
    public class ChunkGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Update Fixed Map"))
            {
                ((CameraViewport)target).UpdateViewportRect();
            }
        }
    }
}
