using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Messageboxのテキストを制御する
public class TextController : MonoBehaviour 
{

	[SerializeField][Range(0.001f, 0.3f)]
	public float intervalForCharacterDisplay = 0.05f;
    [System.NonSerialized]
    public int maneger;
    [System.NonSerialized]
    public bool NoDeley;

    public string currentText = string.Empty;

	private float timeUntilDisplay = 0;
	private float timeElapsed = 1;
	private int lastUpdateCharacter = -1;

	[SerializeField]
	public Text _uiText;

	public bool IsCompleteDisplayText 
	{
		get { return  Time.time > timeElapsed + timeUntilDisplay; }
	}

	// 強制的に全文表示する
	public void ForceCompleteDisplayText ()
	{
		timeUntilDisplay = 0;
	}

	// 次に表示する文字列をセットする
	public void SetNextLine(string text)
	{
		currentText = text;
		timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
		timeElapsed = Time.time;
		lastUpdateCharacter = -1;
        maneger ++;
	}

#region UNITY_CALLBACK	

	void Update () 
	{
        currentText = currentText.TrimEnd();
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
		if (NoDeley) {
			_uiText.text = currentText;
            timeUntilDisplay = 0;
        } else {
			if (displayCharacterCount != lastUpdateCharacter) {
				_uiText.text = currentText.Substring (0, displayCharacterCount);
				lastUpdateCharacter = displayCharacterCount;
			}
		}
	}
#endregion
}