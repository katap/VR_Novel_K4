using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomPropertyDrawer(typeof(CanvasTransition))]
public class ListDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DataDrawarControl draw = new DataDrawarControl();

        //とりあえずlabelなしで半分にした入力フィールドをつくる
        position.width *= 0.15f;
        draw.dataDrawer("transitionNum",     position, property, label);

        position.x += position.width;
        draw.dataDrawer("canvasNum",         position, property, label);

        position.x += position.width;
        draw.dataDrawer("choiceNoSelect",    position, property, label);

        position.x += position.width;
        draw.dataDrawer("choiceLength",      position, property, label);

        position.x += position.width;
        draw.dataDrawer("debugChoiceLength", position, property, label);

        position.x += position.width;
        draw.dataDrawer("fontSize",          position, property, label);

        position.x += position.width;
        draw.dataDrawer("delay",             position, property, label);
    }
}

[CustomPropertyDrawer(typeof(ChoiceTransition))]
public class ChoiceDataDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DataDrawarControl draw = new DataDrawarControl();

        position.width *= 0.15f;
        draw.dataDrawer("debugNo",      position, property, label);

        position.x += position.width;
        draw.dataDrawer("choiceNo",     position, property, label);

        position.x += position.width;
        draw.dataDrawer("canvasNum1",   position, property, label);

        position.x += position.width;
        draw.dataDrawer("FontSize1",   position, property, label);

        position.x += position.width;
        draw.dataDrawer("canvasnum2",   position, property, label);

        position.x += position.width;
        draw.dataDrawer("FontSize2",    position, property, label);

    }
}

[CustomPropertyDrawer(typeof(AnimTransition))]
public class AnimDataDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DataDrawarControl draw = new DataDrawarControl();

        position.width *= 0.15f;
        draw.dataDrawer("animNo",        position, property, label);

        position.x += position.width;
        draw.dataDrawer("animCharacter", position, property, label);

        position.width *= 2f;
        position.x += position.width * 0.5f;
        draw.dataDrawer("animName",      position, property, label);

        position.width *= 0.5f;
        /*
        position.x += position.width;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("secondCanvas"), GUIContent.none);
        */
    }
}

[CustomPropertyDrawer(typeof(ChoiceTextTransition))]
public class ChoiceTextDataDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DataDrawarControl draw = new DataDrawarControl();

        position.width *= 1f;
        draw.dataDrawer("choiceText",   position, property, label);
    }
}

[CustomPropertyDrawer(typeof(SpawnPointTransition))]
public class SpawnPointDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DataDrawarControl draw = new DataDrawarControl();

        position.width *= 0.1f;
        draw.dataDrawer("dataLength",   position, property, label);

        position.x += position.width;
        draw.dataDrawer("eventNum",     position, property, label);

        position.width *= 8f;
        position.x += position.width * 0.13f;
        draw.dataDrawer("spawnPoint",   position, property, label);

    }
}

[CustomPropertyDrawer(typeof(SpawnChoicePointTransition))]
public class SpawnChoicePointDataDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		DataDrawarControl draw = new DataDrawarControl();

		position.width *= 0.1f;
		draw.dataDrawer("dataLength",   position, property, label);

		position.x += position.width;
		draw.dataDrawer("eventNum",     position, property, label);

		position.width *= 8f;
		position.x += position.width * 0.13f;
		draw.dataDrawer("spawnChoicePoint",   position, property, label);

	}
}

public class DataDrawarControl
{
    public void dataDrawer(string str, Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property.FindPropertyRelative(str), GUIContent.none);
    }
    /*
    public void dataName(string str)
    {
        
    }
    */
}