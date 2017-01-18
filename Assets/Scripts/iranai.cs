using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;      //!< デプロイ時にEditorスクリプトが入るとエラーになるので UNITY_EDITOR で括ってね！
using UnityEditorInternal;
#endif

public class iranai : MonoBehaviour {

    /*
#if UNITY_EDITOR
/*
[CustomEditor(typeof(Animation_Command))]               //!< 拡張するときのお決まりとして書いてね
//複数選択有効
[CanEditMultipleObjects]

public class Animation_CommandEditor : Editor           //!< Editorを継承するよ！
{
   public override void OnInspectorGUI()
   {
       Animation_Command anim = target as Animation_Command;

       anim.size = EditorGUILayout.IntField("シーン偏移数", anim.size);
       EditorGUILayout.LabelField("体力(現在/最大)");
       int i = 0;
       int x = anim.moveScene[i];
       int y = anim.moveText[i];
       EditorGUILayout.BeginHorizontal();
       //anim.moveScene[i] = EditorGUILayout.IntField(anim.moveScene[i], GUILayout.Width(48));
       //anim.moveText[i] = EditorGUILayout.IntField(anim.moveText[i], GUILayout.Width(48));
       x = EditorGUILayout.IntField("シーン変更", x);
       y = EditorGUILayout.IntField("シーン変更", y);
       EditorGUILayout.EndHorizontal();
   }


}
//*/
    //#endif

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
