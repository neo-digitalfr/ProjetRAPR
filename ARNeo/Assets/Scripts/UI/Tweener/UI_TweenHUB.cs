using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// User interface tween HUB, usefull when severall tweens are on the same go
/// </summary>
[System.Serializable]
[AddComponentMenu( "Tweener/UITweenHub", 10 )]
public class UI_TweenHUB : MonoBehaviour 
{
	[SerializeField]
	[HideInInspector]
	public List<UI_Tweener> m_lTweens;

	[SerializeField]
	[HideInInspector]
	public int m_iIndice=0;


	public void TweenIn(string _sId)
	{
		if(m_lTweens != null)
		{
			if(_sId != "" )
			{
				for(int i=0; i<m_lTweens.Count;++i)
				{
					if(m_lTweens[i].m_sId == _sId)
					{
						m_lTweens[i].TweenIn();
						return;
					}
				}
			}

			if(m_lTweens.Count>0)
				m_lTweens[0].TweenIn();
		}
	}

	public void TweenOut(string _sId)
	{
		if(m_lTweens != null)
		{
			if(_sId != "" )
			{
				for(int i=0; i<m_lTweens.Count;++i)
				{
					if(m_lTweens[i].m_sId == _sId)
					{
						m_lTweens[i].TweenOut();
						return;
					}
				}
			}
			
			if(m_lTweens.Count>0)
				m_lTweens[0].TweenOut();
		}
	}


	public void AddTween(UI_Tweener _tween)
	{
		if(m_lTweens == null)
		{
			m_lTweens = new List<UI_Tweener>();
		}
		m_lTweens.Add(_tween);
	}
}