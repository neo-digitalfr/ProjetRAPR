using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// User interface tween color.
/// </summary>
public class UI_TweenColor : UI_Tweener
{
	private delegate void UpdateValuePointer(float _fValue);
	private UpdateValuePointer updateValuePointer;
	private MaskableGraphic m_maskableGraphic;
	private CanvasGroup m_canvasGroup;
	public Gradient m_gradient;
	public TextMeshProUGUI m_txtPro;
    public Color m_col;
	protected override void Init()
	{
		base.Init();
        
        m_txtPro = m_goCur.GetComponent<TextMeshProUGUI>();
		if(m_txtPro)
			updateValuePointer = new UpdateValuePointer(UpdateTextMeshpro);
		else
		{
			m_maskableGraphic = m_goCur.GetComponent<MaskableGraphic>();
			if(m_maskableGraphic)
				updateValuePointer = new UpdateValuePointer(UpdateMaskableGraphic);
			else
			{
				m_canvasGroup = m_goCur.AddComponent<CanvasGroup>();
                if (m_canvasGroup)
                    updateValuePointer = new UpdateValuePointer(UpdateCanvasGroup);  
            }
		}

	}

	/// <summary>
	/// Value updated.
	/// </summary>
	/// <param name="_fValue">_f value.</param>
	protected override void ValueUpdated (float _fValue)
	{
		base.ValueUpdated (_fValue);
        m_col = m_gradient.Evaluate(_fValue);
 

        if (updateValuePointer!=null)
		{
			updateValuePointer(_fValue);
		}
	}


	/// <summary>
	/// Updates the maskable graphic.
	/// </summary>
	/// <param name="_fValue">_f value.</param>
	private void UpdateMaskableGraphic(float _fValue)
	{
		m_maskableGraphic.color = m_gradient.Evaluate(_fValue);
	}

	/// <summary>
	/// Updates the canvas group.
	/// </summary>
	/// <param name="_fValue">_f value.</param>
	private void UpdateCanvasGroup(float _fValue)
	{
        
        m_canvasGroup.alpha = m_gradient.Evaluate(_fValue).a;
	}

	private void UpdateTextMeshpro(float _fValue)
	{
		m_txtPro.color = m_gradient.Evaluate(_fValue);
	}
}

