using UnityEngine;
using System.Collections;

public class UI_Widget : MonoBehaviour {

	public virtual void Refresh()
	{

	}

	protected virtual void ParentViewSetted(UI_View _view)
	{

	}

	public UI_View ParentView {
		get {
			return m_parentView;
		}
		set {
			if(value != m_parentView)
			{
				m_parentView = value;
				ParentViewSetted(m_parentView);
			}
			else
				m_parentView = value;
		}
	}

	private UI_View m_parentView;
}
