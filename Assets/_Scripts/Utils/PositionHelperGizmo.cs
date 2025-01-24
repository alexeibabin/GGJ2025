using UnityEngine;

namespace _Scripts.Utils
{
    [ExecuteInEditMode] 
    public class PositionHelperGizmo : MonoBehaviour
    {
        [Header("Helper Properties")]
        public Color gizmoColor = Color.green;
        public float gizmoSize = 0.5f;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoSize);
#if UNITY_EDITOR
            var image = UnityEditor.EditorGUIUtility.IconContent("d_P4_AddedLocal");
            UnityEditor.Handles.Label(transform.position, image, UnityEditor.EditorStyles.boldLabel);
#endif
        }
    }

}