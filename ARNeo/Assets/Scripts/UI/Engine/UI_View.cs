using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;


/// <summary>
/// User interface view. All the views in the game herits from it
/// </summary>
public class UI_View : MonoBehaviour 
{
	public delegate void ViewCallBack(UI_View _view);
	[Header ("Widgets")]
	public List<UI_Widget> m_lWidgets;
	[Header ("Black blocking background")]
	public bool m_bHasBlackBG;

    public UI_Tweener[] m_tTweensLandscape;

	public UI_Tweener m_tweenShow;

	public UI_Tweener m_tweenHide;
    protected  DeviceOrientation curOrientation;
    /// <summary>
    /// Show the specified _eAnimationViewType. Only called by the UIManager, don't call it, uimanager call it in the PopView() and PushView()
    /// </summary>
    /// <param name="_eAnimationViewType">_e animation view type.</param>
    public virtual void Show( AnimationViewType _eAnimationViewType = AnimationViewType.ANIMATED)
	{
		Init();
		if(m_tweenHide)
			m_tweenHide.Stop();
		m_canvasGroup.alpha = 1f;
		m_canvasGroup.interactable = false;
		gameObject.SetActive(true);
		OnWillBeShown();
		//if a tween is available for show, we use it, empty string will use the first tween of tweenhud
		if(m_tweenShow && _eAnimationViewType == AnimationViewType.ANIMATED)
			m_tweenShow.TweenIn();
		else if (m_tweenShow && _eAnimationViewType == AnimationViewType.OVERRIDE_POSITION_LEFT && m_tweenShow.GetType() == typeof(UI_TweenPosition))
		{
			((UI_TweenPosition)m_tweenShow).m_vFrom = new Vector2(-1,0);
			((UI_TweenPosition)m_tweenShow).m_vTo = new Vector2(0,0);
			m_tweenShow.TweenIn();
		}
		else if (m_tweenShow && _eAnimationViewType == AnimationViewType.OVERRIDE_POSITION_RIGHT && m_tweenShow.GetType() == typeof(UI_TweenPosition))
		{
			((UI_TweenPosition)m_tweenShow).m_vFrom = new Vector2(1,0);
			((UI_TweenPosition)m_tweenShow).m_vTo = new Vector2(0,0);
			m_tweenShow.TweenIn();
		}
		else if ( _eAnimationViewType == AnimationViewType.NONE || _eAnimationViewType == AnimationViewType.ANIMATED)
		{
			transform.localScale = Vector3.one;
			OnShowFinished();
		}
	}

	/// <summary>
	/// Hide the specified _eAnimationViewType. Only called by the UIManager, don't call it, uimanager call it in the DePopView()
	/// </summary>
	/// <param name="_eAnimationViewType">_e animation view type.</param>
	public virtual void Hide(AnimationViewType _eAnimationViewType = AnimationViewType.ANIMATED)
	{
		if(gameObject.activeInHierarchy)
		{
			Init();
			m_canvasGroup.interactable = false;
			if(m_tweenHide && _eAnimationViewType == AnimationViewType.ANIMATED)
				m_tweenHide.TweenIn();
			else if (m_tweenHide && _eAnimationViewType == AnimationViewType.OVERRIDE_POSITION_LEFT && m_tweenHide.GetType() == typeof(UI_TweenPosition))
			{
				((UI_TweenPosition)m_tweenHide).m_vFrom = new Vector2(0,0);
				((UI_TweenPosition)m_tweenHide).m_vTo = new Vector2(-1,0);
				m_tweenHide.TweenIn();
			}
			else if (m_tweenHide && _eAnimationViewType == AnimationViewType.OVERRIDE_POSITION_RIGHT && m_tweenHide.GetType() == typeof(UI_TweenPosition))
			{
				((UI_TweenPosition)m_tweenHide).m_vFrom = new Vector2(0,0);
				((UI_TweenPosition)m_tweenHide).m_vTo = new Vector2(1,0);
				m_tweenHide.TweenIn();
			}
			else if ( _eAnimationViewType == AnimationViewType.NONE || _eAnimationViewType == AnimationViewType.ANIMATED)
				OnHideFinished();
		}
	}

    protected virtual void Update()
    {
        if(Input.deviceOrientation != curOrientation || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.A))
        {            
            if ((ScreenOrientation)Input.deviceOrientation == ScreenOrientation.LandscapeLeft || (ScreenOrientation)Input.deviceOrientation == ScreenOrientation.LandscapeRight ||  Input.GetKey(KeyCode.Space))
            {
                for(int i = 0;i < m_tTweensLandscape.Length;++i)
                {
                    m_tTweensLandscape[i].TweenIn(true);
                }
            }
            else if ((ScreenOrientation)Input.deviceOrientation == ScreenOrientation.Portrait || (ScreenOrientation)Input.deviceOrientation == ScreenOrientation.PortraitUpsideDown || Input.GetKey(KeyCode.A))
            {
                for (int i = 0; i < m_tTweensLandscape.Length; ++i)
                {
                    m_tTweensLandscape[i].TweenOut(true);
                }
            }
            curOrientation = Input.deviceOrientation;
        }
    }

	/// <summary>
	/// Called before the tween show is called
	/// </summary>
	public virtual void OnWillBeShown()
    {
        Refresh();
    }

	/// <summary>
	/// Called when the show animation is finished
	/// </summary>
	public virtual void OnShowFinished()
	{        
		m_canvasGroup.interactable = true;
	}

	public virtual void OnHideFinished()
	{
		gameObject.SetActive(false);
		//if there is a callback we call it
		if(m_viewHideCallBack!=null)
		{
			m_viewHideCallBack(this);
		}
	}

	protected virtual void Init()
	{
		if(!m_bIsInit)
		{
			if(m_tweenShow && m_tweenShow.m_onEnd == null)
				m_tweenShow.m_onEnd.AddListener(OnShowFinished);
			m_canvasGroup = GetComponent<CanvasGroup>();
			if(!m_canvasGroup)
				m_canvasGroup = gameObject.AddComponent<CanvasGroup>();
			m_bIsInit = true;
			m_curTrsf = transform;
			m_uiManager = GetComponentInParent<UI_Manager>();

			for (int i = 0, max = m_lWidgets.Count; i < max; i++) 
				m_lWidgets[i].ParentView = this;
		}
	}

    protected virtual void Refresh()
    {
		for(int i=0, iSize = m_lWidgets.Count; i<iSize;++i)
			m_lWidgets[i].Refresh();
    }

	protected virtual T GetManager<T>() where T : UI_Manager
	{
		return (T)m_uiManager;
	}

	protected virtual T GetWidget<T>() where T : UI_Widget
	{
		for (int i = 0, max = m_lWidgets.Count; i < max; i++) 
		{
			if(m_lWidgets[i].GetType() == typeof(T))
				return ((T)m_lWidgets[i]);
		}
		return null;
	}

	protected UI_Manager m_uiManager;
	protected bool m_bIsInit=false;
	protected CanvasGroup m_canvasGroup;
	protected ViewCallBack m_viewHideCallBack;
	protected Transform m_curTrsf;
}

