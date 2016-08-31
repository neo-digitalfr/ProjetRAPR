using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// Localised text. Herits from text mesh pro and add a localization layer on it
/// </summary>
public class UI_LocalisedText : TextMeshProUGUI 
{
	[SerializeField]
	public string m_textId = "";

	[SerializeField]
	public bool m_bUpper=false;
	/// <summary>
	/// Gets or sets the text ID and update the localization.
	/// </summary>
	/// <value>The text ID</value>
	public string TextID {
		get {
			return m_textId;
		}
		set {
			m_textId = value;
			UpdateLocalization();
		}
	}


	#region Main Function
	protected override void Awake ()
	{
		base.Awake ();
		UpdateLocalization();
	}
	#endregion

	
	private void UpdateLocalization()
	{
		if(Application.isPlaying && m_textId != "")
		{
			this.text = m_bUpper?Language.Get(m_textId).ToUpper():Language.Get(m_textId) ; 
		}
	}
}
