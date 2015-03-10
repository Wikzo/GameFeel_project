using UnityEditor;
using UnityEngine;
//[CustomEditor(typeof(Rigidbody))]

// http://answers.unity3d.com/questions/310097/is-there-a-way-to-draw-center-of-mass-on-the-scree.html
/*public class RigidbodyEditor : Editor
{
	void OnSceneGUI()
	{
		Rigidbody rb = target as Rigidbody;
		Handles.color = Color.red;
		Handles.SphereCap(1, rb.transform.TransformPoint(rb.centerOfMass), rb.rotation, 1f);
	}
	public override void OnInspectorGUI()
	{
		GUI.skin = EditorGUIUtility.GetBuiltinSkin(UnityEditor.EditorSkin.Inspector);
		DrawDefaultInspector();
	}
}*/

public class RigidbodyEditor : MonoBehaviour
{
	
}