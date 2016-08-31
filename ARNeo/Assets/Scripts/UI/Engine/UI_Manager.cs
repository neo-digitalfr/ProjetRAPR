using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;



public enum AnimationViewType{NONE, ANIMATED, OVERRIDE_SCALE,OVERRIDE_ROTATE, OVERRIDE_ALPHA,OVERRIDE_POSITION_LEFT,OVERRIDE_POSITION_RIGHT};

/// <summary>
/// User interface manager. Handle all the views in a scene
/// </summary>
public class UI_Manager : MonoBehaviour 
{
	public GameObject m_goBlackBg;
	
	public virtual void ShowView<T>() where T : UI_View { }

	public virtual void HideView<T>() where T : UI_View { }

	protected void ShowViewBack(UI_View _view, AnimationViewType _eAnimationViewType= AnimationViewType.ANIMATED)
	{
		m_curBackView = _view;
		m_curBackView.Show(_eAnimationViewType);
		UpdateAllPopedViewZOrder();
		ShowBlackBG(m_curBackView);
	}

	protected void ShowViewFront(UI_View _view, AnimationViewType _eAnimationViewType= AnimationViewType.ANIMATED)
	{
		m_curFrontView = _view;
		Debug.Log (_view);
		m_curFrontView.Show(_eAnimationViewType);
		UpdateAllPopedViewZOrder();
		ShowBlackBG(m_curFrontView);
	}

	/// <summary>
	/// Pop a view over the pushed view, if no pushed view => it will be set as the pushed view
	/// </summary>
	/// <param name="_view">_view.</param>
	/// <param name="_inventoryElement">_inventory element.</param>
	/// <param name="_eAnimationViewType">_e animation view type.</param>
	protected void ShowView(UI_View _view, AnimationViewType _eAnimationViewType= AnimationViewType.ANIMATED)
	{
		if(m_curPopedView == _view)
		{
			//View already shown, we want to update the display but don't play the animation
			m_curPopedView.Show(AnimationViewType.ANIMATED);
		}
		else
		{
			m_curPopedView = _view;
			m_qPopedViews.Push(m_curPopedView);
			m_curPopedView.Show(_eAnimationViewType);
			UpdateAllPopedViewZOrder();
		}
		ShowBlackBG(m_curPopedView);
	}

	/// <summary>
	/// Pop a view over the pushed view, if no pushed view => it will be set as the pushed view
	/// </summary>
	/// <param name="_view">_view.</param>
	/// <param name="_inventoryElement">_inventory element.</param>
	/// <param name="_eAnimationViewTypeSHOW">_e animation view type SHO.</param>
	/// <param name="_eAnimationViewTypeHIDE">_e animation view type HID.</param>
	protected void ShowViewAndHideLast(UI_View _view, AnimationViewType _eAnimationViewTypeSHOW= AnimationViewType.ANIMATED, AnimationViewType _eAnimationViewTypeHIDE= AnimationViewType.ANIMATED)
	{
		if (m_curPopedView != _view && !_view.gameObject.activeInHierarchy)
		{
			m_curPopedView = _view;
			if(m_qPopedViews.Count>0)
			{
				UI_View view = m_qPopedViews.Pop();
				view.Hide(_eAnimationViewTypeHIDE);
				m_qPopedViews.Push(view);
			}
			m_qPopedViews.Push(_view);
			m_curPopedView.Show(_eAnimationViewTypeSHOW);
			UpdateAllPopedViewZOrder();
			ShowBlackBG(m_curPopedView);
		}
	}

	/// <summary>
	/// Pop in a view and hide all underneath views
	/// </summary>
	/// <param name="_view">_view.</param>
	/// <param name="_inventoryElement">_inventory element.</param>
	/// <param name="_eAnimationViewTypeSHOW">_e animation view type SHO.</param>
	/// <param name="_eAnimationViewTypeHIDE">_e animation view type HID.</param>
	protected void ShowViewAndHideAll(UI_View _view, AnimationViewType _eAnimationViewTypeSHOW= AnimationViewType.ANIMATED, AnimationViewType _eAnimationViewTypeHIDE= AnimationViewType.ANIMATED)
	{
		bool bSameView = false;
		if(m_qPopedViews.Count>0)
		{
			UI_View view1 = m_qPopedViews.Pop();
			if(_view!=view1)
				view1.Hide(_eAnimationViewTypeHIDE);
			else
			{
				m_qPopedViews.Push(_view);
				bSameView = true;
			}
		}
		if(!bSameView)
		{
			while(m_qPopedViews.Count>0)
			{
				m_qPopedViews.Pop().Hide(_eAnimationViewTypeHIDE);
			}
			m_curPopedView = _view;
			m_qPopedViews.Push(m_curPopedView);
			m_curPopedView.Show(_eAnimationViewTypeSHOW);
			UpdateAllPopedViewZOrder();
			ShowBlackBG(m_curPopedView);
		}
	}

	/// <summary>
	/// Depop in a view, last view in stack will be shown
	/// </summary>
	/// <param name="_view">_view.</param>
	/// <param name="_eAnimationViewTypeSHOW">_e animation view type SHO.</param>
	/// <param name="_eAnimationViewTypeHIDE">_e animation view type HID.</param>
	protected void HideView(UI_View _view, AnimationViewType _eAnimationViewTypeSHOW= AnimationViewType.ANIMATED, AnimationViewType _eAnimationViewTypeHIDE= AnimationViewType.ANIMATED)
	{
		if(m_qPopedViews.Contains(_view) && m_qPopedViews.Count>0 && _view.gameObject.activeInHierarchy)
		{
			List<UI_View> lViews = new List<UI_View>();
			bool bFound = false;

			while(!bFound && m_qPopedViews.Count>0)
			{
				UI_View view = m_qPopedViews.Pop();
				if(view == _view)
				{
					view.Hide(_eAnimationViewTypeHIDE);
					HideBlackBG(view);
					bFound = true;
				}
				else
					lViews.Add(view);
			}

			if(lViews.Count>0)
				for (int i = lViews.Count-1; i >= 0; i--) 
					m_qPopedViews.Push(lViews[i]);

			if(m_qPopedViews.Count == 0)
			{
				m_curPopedView = null;
			}
			else
			{
				//Check if last pop is active, if not we active it
				UI_View view= m_qPopedViews.Pop();
				m_curPopedView = view;
				if(!view.gameObject.activeInHierarchy)
					view.Show(_eAnimationViewTypeSHOW);
				else
					view.Show(AnimationViewType.NONE);
				m_qPopedViews.Push(view);
				UpdateAllPopedViewZOrder();
				ShowBlackBG(m_curPopedView);
			}
		}
	}

	protected void HideViewBack(AnimationViewType _eAnimationViewType= AnimationViewType.ANIMATED)
	{
		if(m_curBackView)
		{
			m_curBackView.Hide(AnimationViewType.ANIMATED);
			UpdateAllPopedViewZOrder();
			ShowBlackBG(m_curBackView);
			m_curBackView = null;
		}
	}
	
	protected void HideViewFront(UI_View _view, AnimationViewType _eAnimationViewType= AnimationViewType.ANIMATED)
	{
		if(m_curFrontView && m_curFrontView == _view)
		{
			m_curFrontView.Hide(AnimationViewType.ANIMATED);
			UpdateAllPopedViewZOrder();
			ShowBlackBG(m_curFrontView);
			m_curFrontView = null;
		}
	}

	/// <summary>
	/// Depop in all poped views to the desired view
	/// </summary>
	/// <param name="_view">_view.</param>
	/// <param name="_eAnimationViewTypeSHOW">_e animation view type SHO.</param>
	/// <param name="_eAnimationViewTypeHIDE">_e animation view type HID.</param>
	protected void HideToView(UI_View _view, AnimationViewType _eAnimationViewTypeSHOW= AnimationViewType.ANIMATED, AnimationViewType _eAnimationViewTypeHIDE= AnimationViewType.ANIMATED)
	{
		if(m_qPopedViews.Contains(_view))
		{
			bool bFound = false;
			while(!bFound && m_qPopedViews.Count>1)
			{
				UI_View view1 = m_qPopedViews.Pop();
				UI_View view2 = m_qPopedViews.Pop();
				if(view2 == _view)
				{
					m_curPopedView = view2;
					view1.Hide(_eAnimationViewTypeHIDE);
					m_qPopedViews.Push(view2);
					bFound = true;
				}
			}

			if(bFound)
			{
				//Check if desired is active, if not we active it
				if(!m_curPopedView.gameObject.activeInHierarchy)
					m_curPopedView.Show(_eAnimationViewTypeSHOW);
				else
					m_curPopedView.Show(AnimationViewType.NONE);
				UpdateAllPopedViewZOrder();
			}
		}
	}


	/// <summary>
	/// Depop in all views, only the pushed view will be shown
	/// </summary>
	/// <param name="_eAnimationViewType">_e animation view type.</param>
	protected virtual void HideAllViews(AnimationViewType _eAnimationViewType= AnimationViewType.ANIMATED)
	{
		while(m_qPopedViews.Count>0)
		{
			UI_View view = m_qPopedViews.Pop();
			if(view)
			{
				view.Hide(_eAnimationViewType);
			}
		}
		m_curPopedView = null;
	}


	/// <summary>
	/// Update the z order of all the views
	/// </summary>
	private void UpdateAllPopedViewZOrder()
	{
		int iBackViewOffSet = 0;
		if(m_curBackView)
		{
			iBackViewOffSet =1;
			m_curBackView.transform.SetSiblingIndex(0);
		}
		int iZOrder = m_qPopedViews.Count + iBackViewOffSet;
		foreach(UI_View view in m_qPopedViews)
		{
			iZOrder--;
			view.transform.SetSiblingIndex(iZOrder);
		}
		if(m_curFrontView)
			m_curFrontView.transform.SetSiblingIndex(m_qPopedViews.Count + iBackViewOffSet+1);
	}

	protected virtual void SetViewZOrder(UI_View _view, int _iZOrder)
	{
		_view.transform.SetSiblingIndex(_iZOrder);
	}

	protected virtual void Awake()
	{
		m_qPopedViews = new Stack<UI_View>();

		UI_View[] tViews = GetComponentsInChildren<UI_View>();
		for(int i=0;i < tViews.Length;++i)
			tViews[i].gameObject.SetActive(false);
	}

	protected virtual void AddViewToStack(UI_View _view)
	{
		m_qPopedViews.Push(_view);
	}

/*
#if UNITY_EDITOR
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
			PrintStack();
	}
#endif*/

	private void PrintStack()
	{
		foreach(UI_View view in m_qPopedViews)
		{
			Debug.Log(view);
		}
	}

	private void ShowBlackBG(UI_View _view)
	{
		if(_view.m_bHasBlackBG )
		{
			m_goBlackBg.SetActive(true);
			m_goBlackBg.transform.SetSiblingIndex(_view.transform.GetSiblingIndex());
		}
	}

	private void HideBlackBG(UI_View _view)
	{
		if(_view.m_bHasBlackBG )
			m_goBlackBg.SetActive(false);
	}
	
	protected static UI_Manager m_instance;
	protected UI_View m_curPopedView = null;
	protected UI_View m_curBackView = null;
	protected UI_View m_curFrontView = null;
	protected Stack<UI_View> m_qPopedViews;
}
