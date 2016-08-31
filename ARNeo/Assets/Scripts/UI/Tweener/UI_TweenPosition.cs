using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using ExtensionMethods;
using UnityEngine.UI;

/// <summary>
/// User interface tween position.
/// </summary>
[AddComponentMenu( "Tweener/UITweenPosition", 10 )]
public class UI_TweenPosition : UI_Tweener
{
	public Vector2 m_vFrom;
	public Vector2 m_vTo;
	public Canvas m_canvas;
    private DeviceOrientation curOrientation;
    private RectTransform rctCanvas;

    protected override void Init ()
	{
		base.Init ();
        curOrientation = Input.deviceOrientation;

        if (!m_canvas)
			m_canvas = m_trsfCur.FindCanvasInParents();
        rctCanvas = m_canvas.GetComponent<RectTransform>();
        
    }

	protected override void ValueUpdated (float _fValue)
	{
		base.ValueUpdated (_fValue);

        if (rctCanvas )
        {
            m_vCanvasSize = new Vector2(rctCanvas.GetWidth(), rctCanvas.GetHeight());
        }



        if (m_curRectTransfrom)
			m_curRectTransfrom.anchoredPosition = Vector2.Lerp (Vector2.Scale(m_vFrom,m_vCanvasSize), Vector2.Scale(m_vTo,m_vCanvasSize), _fValue);
    }

    private Vector2 m_vCanvasSize;
}
