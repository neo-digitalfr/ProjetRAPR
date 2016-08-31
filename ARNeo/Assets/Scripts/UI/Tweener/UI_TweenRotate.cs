using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


/// <summary>
/// User interface tween rotate.
/// </summary>
[AddComponentMenu( "Tweener/UITweenRotate", 10 )]
public class UI_TweenRotate : UI_Tweener
{
	public float m_fFrom;
	public float m_fTo;
	public bool m_bLocalSpace;
	public bool m_bRelative;

	protected override void Init ()
	{
		base.Init ();
		if(m_bLocalSpace && m_curRectTransfrom)
		{
			m_trsf = m_curRectTransfrom.transform;
		}
	}

	protected override void StartTween (bool _bResume=false)
	{
		base.StartTween (_bResume);
		if(m_bRelative)
		{
			float fDiff = m_fTo - m_fFrom;
			if(!m_bLocalSpace)
				m_fFrom = m_curRectTransfrom.rotation.eulerAngles.z;
			else
				m_fFrom = m_trsf.localRotation.eulerAngles.z;
				
			m_fTo += fDiff;
		}
	}
	
	protected override void ValueUpdated (float _fValue)
	{
		base.ValueUpdated (_fValue);
		if(!m_bLocalSpace)
			m_curRectTransfrom.rotation = Quaternion.Euler(0,0, Mathf.Lerp (m_fFrom, m_fTo, _fValue));
		else
			m_trsf.localRotation = Quaternion.Euler(0,0, Mathf.Lerp (m_fFrom, m_fTo, _fValue));
	}

	private Transform m_trsf;
}