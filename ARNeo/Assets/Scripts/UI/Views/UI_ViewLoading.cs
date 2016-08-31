using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ViewLoading : UI_View 
{
	public string m_sLevelToLoad="";
	public bool m_bInstantLoad = false;
    public Image m_imgLoadBar;

	public void ShowLoading(bool _b)
	{

	}

	public override void OnWillBeShown ()
	{
		base.OnWillBeShown ();
		if(m_bInstantLoad)
		{
			if(m_sLevelToLoad != "" && m_sLevelToLoad != null)
			{
				SceneManager.LoadSceneAsync(m_sLevelToLoad);

				m_sLevelToLoad = "";
				Time.timeScale = 1f;
			}
		}
	}

    IEnumerator AsynchronousLoad(string scene)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;
        m_sLevelToLoad = "";
        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            m_imgLoadBar.fillAmount = progress;
            // Loading completed
            if (ao.progress == 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public override void OnShowFinished ()
	{
		base.OnShowFinished ();
		if(!m_bInstantLoad)
		{
			if(m_sLevelToLoad != "" && m_sLevelToLoad != null)
			{
                StartCoroutine(AsynchronousLoad(m_sLevelToLoad));                
				Time.timeScale = 1f;
                
			}
		}
	}
}
