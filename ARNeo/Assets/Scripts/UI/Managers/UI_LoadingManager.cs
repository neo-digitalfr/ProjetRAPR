using UnityEngine;
using System.Collections;

public class UI_LoadingManager : UI_Manager 
{
    public UI_ViewLoading m_viewLoading;

    public override void ShowView<T> ()
	{
		base.ShowView<T> ();
		System.Type viewType =typeof(T);
		if( viewType== typeof(UI_ViewLoading))
		{
			ShowViewAndHideAll(m_viewLoading);
		}
    }

	public override void HideView<T> ()
	{
		base.HideView<T> ();
		System.Type viewType =typeof(T);
        if(viewType == typeof(UI_ViewLoading))
        {
            HideView(m_viewLoading);
        }
    }
	

	#region SINGLETON
	public static UI_LoadingManager GetInstance ()
	{
		return m_instance;
	}

	protected override void Awake ()
	{
		base.Awake();
		m_instance = this;
        ShowView<UI_ViewLoading>();
	}

	private static UI_LoadingManager m_instance;
	#endregion

	private RectTransform m_rctTrsf;
}
