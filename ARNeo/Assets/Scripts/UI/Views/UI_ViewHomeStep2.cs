using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ViewHomeStep2: UI_View 
{
    public void Prev()
    {
        UI_HomeManager.GetInstance().ShowView<UI_ViewHomeStep1>();
        
    }

    public void Next()
    {        
        UI_HomeManager.GetInstance().HideView<UI_ViewHomeStep2>();
    }

	public void ShowLoading(bool _b)
	{

	}

	public override void OnWillBeShown ()
	{
		base.OnWillBeShown ();

	}
    
    public override void OnShowFinished ()
	{
        base.OnShowFinished();
	}
}
