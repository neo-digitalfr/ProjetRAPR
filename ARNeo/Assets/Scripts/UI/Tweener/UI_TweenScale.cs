using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


/// <summary>
/// User interface tween scale.
/// </summary>
[AddComponentMenu( "Tweener/UITweenScale", 10 )]
public class UI_TweenScale : UI_Tweener
{
	private delegate void UpdateValuePointer(float _fValue);
	private UpdateValuePointer updateValuePointer;
	public bool bKeepZToOne=true;
	public Vector3 m_vFrom;
	public Vector3 m_vTo;
	public bool m_bRelative = false;  

	protected override void Init()
	{
		base.Init();
		if(m_curRectTransfrom)
			updateValuePointer = new UpdateValuePointer(UpdateRectTransform);
		else
			updateValuePointer = new UpdateValuePointer(UpdateTransform);
	}

	/// <summary>
	/// Value updated.
	/// </summary>
	/// <param name="_fValue">_f value.</param>
	protected override void ValueUpdated (float _fValue)
	{
		base.ValueUpdated (_fValue);
		if(updateValuePointer!=null)
		{
			updateValuePointer(_fValue);
		}
	}


	/// <summary>
	/// Updates the maskable graphic.
	/// </summary>
	/// <param name="_fValue">_f value.</param>
	private void UpdateRectTransform(float _fValue)
	{
		if(bKeepZToOne)
		{
			Vector3 v2dScale = Vector3.Lerp( m_vFrom,  m_vTo, _fValue);
			v2dScale.z = 1f;
			m_curRectTransfrom.localScale = v2dScale;
		}
		else
			m_curRectTransfrom.localScale = Vector3.Lerp( m_vFrom,  m_vTo, _fValue);
	}

	/// <summary>
	/// Updates the canvas group.
	/// </summary>
	/// <param name="_fValue">_f value.</param>
	private void UpdateTransform(float _fValue)
	{
		if(bKeepZToOne)
		{
			Vector3 v2dScale = Vector3.Lerp( m_vFrom,  m_vTo, _fValue);
			v2dScale.z = 1f;
			m_trsfCur.localScale = v2dScale;
		}
		else
			m_trsfCur.localScale = Vector3.Lerp( m_vFrom,  m_vTo, _fValue);
	}


	protected override void StartTween (bool _bResume=false)
	{
		base.StartTween (_bResume);
		if(m_bRelative)
		{
			if(m_curRectTransfrom)
			{
				m_vFrom = new Vector3(m_curRectTransfrom.localScale.x * m_vFrom.x, m_curRectTransfrom.localScale.y * m_vFrom.y,m_curRectTransfrom.localScale.z * m_vFrom.z);
				m_vTo = new Vector3( m_curRectTransfrom.localScale.x * m_vTo.x,m_curRectTransfrom.localScale.y * m_vTo.y,m_curRectTransfrom.localScale.z * m_vTo.z);
			}
			else
			{
				m_vFrom = new Vector3(m_trsfCur.localScale.x * m_vFrom.x, m_trsfCur.localScale.y * m_vFrom.y,m_trsfCur.localScale.z * m_vFrom.z);
				m_vTo = new Vector3( m_trsfCur.localScale.x * m_vTo.x,m_trsfCur.localScale.y * m_vTo.y,m_trsfCur.localScale.z * m_vTo.z);
			}
		}
	}

}

