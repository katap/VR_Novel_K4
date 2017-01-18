using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(TransitionBook))]
public class AddressbookInspector : Editor
{
    
    private ReorderableList reorderableList;

    void OnEnable()
    {
        //SerializedObjectからdata(TransitionBook.data)を取得し、ReordableListを作成する
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("data"));
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            // PropertyFieldを使ってよしなにプロパティの描画を行う（PersonDataはPropertyDrawerを使っているのでそちらに移譲されます）
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        };
        reorderableList.drawHeaderCallback += rect =>
        {
            EditorGUI.LabelField(rect, "No./MCanv/MCNoS./CLen/DCLen/Siz/Delay:");
        };
    }

    public override void OnInspectorGUI()
    {
        // 自前で描画している部分。ApplyModifiedPropertiesも忘れずに！
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(TransitionChoice))]
public class AddressChoiceBookInspector : Editor
{

    private ReorderableList reorderableList;

    void OnEnable()
    {
        //SerializedObjectからdata(TransitionBook.data)を取得し、ReordableListを作成する
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("choiceData"));
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            // PropertyFieldを使ってよしなにプロパティの描画を行う（PersonDataはPropertyDrawerを使っているのでそちらに移譲されます）
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        };
        reorderableList.drawHeaderCallback += rect =>
        {
            EditorGUI.LabelField(rect, "DNo./CTextNo./CNum1/FSiz1/NcNo1./Cnum2/FSiz2/NcNo2.:");
        };
    }

    public override void OnInspectorGUI()
    {
        // 自前で描画している部分。ApplyModifiedPropertiesも忘れずに！
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(TransitionAnimBook))]
public class AddressAnimBookInspector : Editor
{

    private ReorderableList reorderableList;

    void OnEnable()
    {
        //SerializedObjectからdata(TransitionBook.data)を取得し、ReordableListを作成する
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("animData"));
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            // PropertyFieldを使ってよしなにプロパティの描画を行う（PersonDataはPropertyDrawerを使っているのでそちらに移譲されます）
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        };
        reorderableList.drawHeaderCallback += rect =>
        {
            EditorGUI.LabelField(rect, "No./Chara/AnimeName:");
        };
    }

    public override void OnInspectorGUI()
    {
        // 自前で描画している部分。ApplyModifiedPropertiesも忘れずに！
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}


[CustomEditor(typeof(TransitionChoiceText))]
public class AddressChoiceTextInspector : Editor
{

    private ReorderableList reorderableList;

    void OnEnable()
    {
        //SerializedObjectからdata(TransitionBook.data)を取得し、ReordableListを作成する
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("choiceTextData"));
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            // PropertyFieldを使ってよしなにプロパティの描画を行う（PersonDataはPropertyDrawerを使っているのでそちらに移譲されます）
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        };
        reorderableList.drawHeaderCallback += rect =>
        {
            EditorGUI.LabelField(rect, "ChoiceText");
        };
    }

    public override void OnInspectorGUI()
    {
        // 自前で描画している部分。ApplyModifiedPropertiesも忘れずに！
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(TransitionSpawnPoint))]
public class AddressSpawnPointInspector : Editor
{

    private ReorderableList reorderableList;

    void OnEnable()
    {
        //SerializedObjectからdata(TransitionBook.data)を取得し、ReordableListを作成する
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("spawnPointData"));
        reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
        {
            SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            // PropertyFieldを使ってよしなにプロパティの描画を行う（PersonDataはPropertyDrawerを使っているのでそちらに移譲されます）
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        };
        reorderableList.drawHeaderCallback += rect =>
        {
            EditorGUI.LabelField(rect, "No./EventNo./AActive/SpawnPoint");
        };
    }
    
    
    public override void OnInspectorGUI()
    {
        // 自前で描画している部分。ApplyModifiedPropertiesも忘れずに！
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(TransitionSpawnChoicePoint))]
public class AddressSpawnChoicePointInspector : Editor
{

	private ReorderableList reorderableList;

	void OnEnable()
	{
		//SerializedObjectからdata(TransitionBook.data)を取得し、ReordableListを作成する
		reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("spawnChoicePointData"));
		reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
		{
			SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
			// PropertyFieldを使ってよしなにプロパティの描画を行う（PersonDataはPropertyDrawerを使っているのでそちらに移譲されます）
			EditorGUI.PropertyField(rect, property, GUIContent.none);
		};
		reorderableList.drawHeaderCallback += rect =>
		{
			EditorGUI.LabelField(rect, "No./EventNo./SpawnChoicePoint");
		};
	}


	public override void OnInspectorGUI()
	{
		// 自前で描画している部分。ApplyModifiedPropertiesも忘れずに！
		reorderableList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}
}
