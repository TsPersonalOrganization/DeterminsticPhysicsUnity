using UnityEngine;

public class PrefabFixer : MonoBehaviour
{
	void OnEnable ()
	{
#if UNITY_EDITOR
		UnityEditor.PrefabUtility.prefabInstanceUpdated = OnPrefabUpdate;
		Debug.Log ("OnEnable");
#endif
	}

#if UNITY_EDITOR
	void OnPrefabUpdate (GameObject go)
	{
		Debug.Log (go.name, go);
		UnityEditor.PrefabUtility.DisconnectPrefabInstance (go);
	}
#endif
}
