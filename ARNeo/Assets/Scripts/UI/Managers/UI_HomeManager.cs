using UnityEngine;
using System.Collections;
using Vuforia;

public class UI_HomeManager : UI_Manager 
{

    public UI_ViewLoading m_viewLoading;
    public UI_ViewHomeStep1 m_viewHomeStep1;
    public UI_ViewHomeStep2 m_viewHomeStep2;
    public UI_ViewBg m_viewBg;

    public override void ShowView<T>()
    {
        base.ShowView<T>();
        System.Type viewType = typeof(T);
        if (viewType == typeof(UI_ViewLoading))
            ShowViewAndHideAll(m_viewLoading, AnimationViewType.NONE);
        else if (viewType == typeof(UI_ViewHomeStep1))
        {
            if(m_curPopedView == m_viewHomeStep2)
            {
                ((UI_TweenPosition)m_viewHomeStep1.m_tweenShow).m_vFrom = ((UI_TweenPosition)m_viewHomeStep1.m_tweenHide).m_vTo;
                ((UI_TweenPosition)m_viewHomeStep2.m_tweenHide).m_vTo = ((UI_TweenPosition)m_viewHomeStep2.m_tweenShow).m_vFrom;
            }
            ShowViewAndHideAll(m_viewHomeStep1);
        }
        else if (viewType == typeof(UI_ViewHomeStep2))
        {
            ShowViewAndHideAll(m_viewHomeStep2);
        }
    }

    public override void HideView<T>()
    {
        base.HideView<T>();
        System.Type viewType = typeof(T);
        if (viewType == typeof(UI_ViewLoading))
        {
            HideView(m_viewLoading);
        }
        else if (viewType == typeof(UI_ViewHomeStep2))
        {
            VuforiaBehaviour.Instance.enabled = true;
            ((UI_TweenPosition)m_viewHomeStep2.m_tweenShow).m_vFrom = new Vector3(0, 0, 0);
            ((UI_TweenPosition)m_viewHomeStep2.m_tweenHide).m_vTo = new Vector3(-1, 0, 0);
            HideView(m_viewHomeStep2);
            HideViewBack(AnimationViewType.ANIMATED);
        }
    }

    private void Start()
    {
        ShowViewBack(m_viewBg, AnimationViewType.NONE);
        //ShowView<UI_ViewLoading>();
        ShowView<UI_ViewHomeStep1>();
        VuforiaBehaviour.Instance.enabled = false;
    }

    #region SINGLETON
    public static UI_HomeManager GetInstance ()
	{
		return m_instance;
	}

	protected override void Awake ()
	{
		base.Awake();
		m_instance = this;
	}

	private static UI_HomeManager m_instance;
	#endregion

	private RectTransform m_rctTrsf;
}
