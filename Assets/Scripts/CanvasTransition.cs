using System;
using UnityEngine;

[Serializable]
public class CanvasTransition
{
    public int transitionNum;

    public int canvasNum;

    public int choiceNoSelect;

    public int choiceLength;

    public int debugChoiceLength;

    public int fontSize;

    public bool delay;
}

[Serializable]
public class ChoiceTransition
{
    public int debugNo;

    public int choiceNo;

    public int canvasNum1;

    public int FontSize1;

    public int nextChoiceNo1;

    public int canvasnum2;

    public int FontSize2;

    public int nextChoiceNo2;
}

[Serializable]
public class AnimTransition
{
    public int animNo;

    public int animCharacter;

    public string animName;

}

[Serializable]
public class ChoiceTextTransition
{
    public string choiceText;

}

[Serializable]
public class SpawnPointTransition
{
    public int dataLength;

    public int eventNum;

    public Vector3 spawnPoint;

    public bool arrowActive;

}

[Serializable]
public class SpawnChoicePointTransition
{
	public int dataLength;

	public int eventNum;

	public Vector3 spawnChoicePoint;

}