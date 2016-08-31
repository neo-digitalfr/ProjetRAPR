using UnityEngine;
using System.Collections;


/// <summary>
/// User interface tween translate.
/// </summary>
[AddComponentMenu( "Tweener/UITweenTranslate", 10 )]
public class UI_TweenTranslate : UI_Tweener {

	public Vector3 m_vTranslate;

	protected override void Init ()
	{
		base.Init ();
		if(m_curRectTransfrom)
			m_vFrom = m_curRectTransfrom.anchoredPosition;
		else
			m_vFrom =m_trsfCur.position;
		
		m_vTo = m_vFrom + m_vTranslate;
	}
	
	protected override void ValueUpdated (float _fValue)
	{
		base.ValueUpdated (_fValue);
		if(m_curRectTransfrom)
			m_curRectTransfrom.anchoredPosition = Vector2.Lerp (m_vFrom, m_vTo, _fValue);
		else
			m_trsfCur.position = Vector3.Lerp (m_vFrom, m_vTo, _fValue);
	}
	
	private Vector3 m_vFrom;
	private Vector3 m_vTo;
}

