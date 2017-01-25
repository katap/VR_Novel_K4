using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

[RequireComponent(typeof( TextController))]
public class ScenarioManager : SingletonMonoBehaviourFast<ScenarioManager> {

	public string LoadFileName;

    [System.NonSerialized]
	public string[] m_scenarios;
    [System.NonSerialized]
    public int m_currentLine = 0;
    [System.NonSerialized]
    public bool m_isCallPreload = false;

    [System.NonSerialized]
	public TextController m_textController;
    [System.NonSerialized]
    public CommandController m_commandController;

	public void RequestNextLine ()
	{
		var currentText = m_scenarios[m_currentLine];

		m_textController.SetNextLine(CommandProcess(currentText));
		m_currentLine ++;
		m_isCallPreload = false;
	}

	public void UpdateLines(string fileName)
	{
		var scenarioText = Resources.Load<TextAsset>("Scenario/" + fileName);
		
		if( scenarioText == null ){
			Debug.LogError("シナリオファイルが見つかりませんでした");
			Debug.LogError("ScenarioManagerを無効化します");
			enabled = false;
			return;
		}
		m_scenarios = scenarioText.text.Split(new string[]{"@br"}, System.StringSplitOptions.None);
		m_currentLine = 0;

		Resources.UnloadAsset(scenarioText);
	}

	private string CommandProcess(string line)
	{
		var lineReader = new StringReader(line);
		var lineBuilder = new StringBuilder();
		var text = string.Empty;
		while( (text = lineReader.ReadLine()) != null )
		{
			var commentCharacterCount = text.IndexOf("//");
			if( commentCharacterCount != -1 ){
				text = text.Substring(0, commentCharacterCount );
			}

			if(! string.IsNullOrEmpty( text )  ){
				if( text[0] == '@' &&  m_commandController.LoadCommand(text))
					continue;
				lineBuilder.AppendLine(text);
			}
		}
		
		return lineBuilder.ToString();
	}
	
#region UNITY_CALLBACK

	void Start () {
		m_textController = GetComponent<TextController>();
		m_commandController = GetComponent<CommandController>();

		UpdateLines(LoadFileName);
		RequestNextLine();
	}

    void Update () 
	{
        if ( m_textController.IsCompleteDisplayText  ){
			if( m_currentLine < m_scenarios.Length)
			{
				if( !m_isCallPreload )
				{
					m_commandController.PreloadCommand(m_scenarios[m_currentLine]);
					m_isCallPreload = true;
				}
						
				if( Input.GetMouseButtonUp(0)){
					RequestNextLine();
				}

                if(m_textController.maneger != 1)
                {
                    if (Input.GetMouseButtonUp(1))
                    {
                        m_textController.maneger -= 2;
                        m_currentLine -= 2;
                        RequestNextLine();
                    }
                }
			}
		}else{
			if(Input.GetMouseButtonUp(0)){
				m_textController.ForceCompleteDisplayText();
			}
        }
	}

#endregion
}
