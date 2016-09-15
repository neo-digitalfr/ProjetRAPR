using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class UI_ViewHomeStep2: UI_View 
{
    public Transform m_trsfArrows;
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
    protected override void Update()
    {
        base.Update();
        if (m_bInterpolateArrow)
        {
            /*if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    m_fOffset = Input.GetTouch(0).position.x - m_trsfArrows.position.x;
                }
                Vector2 vPos = Input.GetTouch(0).position;
                vPos.y = m_trsfArrows.position.y + m_fOffset;
                m_trsfArrows.position = vPos;
            }*/

            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_fOffset = Input.mousePosition.x - m_trsfArrows.position.x;
                }
                else
                {
                    Vector2 vPos = Input.mousePosition;
                    vPos.x -= m_fOffset;
                    vPos.y = m_vArrowInitPos.y;
                    m_trsfArrows.position = vPos;
                }

            }
            else
            {
                m_trsfArrows.localPosition = Vector3.Lerp(m_trsfArrows.localPosition, m_vArrowInitLocalPos, Time.deltaTime * 5f);
            }
            if (m_trsfArrows.localPosition.x - m_vArrowInitLocalPos.x < -90)
            {
                UI_HomeManager.GetInstance().HideView<UI_ViewHomeStep2>();
            }
        }


    }

    public override void Hide(AnimationViewType _eAnimationViewType = AnimationViewType.ANIMATED)
    {
        base.Hide(_eAnimationViewType);
        m_bInterpolateArrow = false;
    }

    public override void OnHideFinished()
    {
        base.OnHideFinished();
        VuforiaBehaviour.Instance.enabled = true;
    }
    public override void OnWillBeShown()
    {
        base.OnWillBeShown();
        m_vArrowInitPos = m_trsfArrows.position;
        m_vArrowInitLocalPos = m_trsfArrows.localPosition;
        MiniGestureRecognizer.isActive = false;
    }


    public override void OnShowFinished()
    {
        base.OnShowFinished();
        MiniGestureRecognizer.isActive = true;
        m_bInterpolateArrow = true;
    }

    private bool m_bInterpolateArrow = false;
    private float m_fOffset;
    private Vector3 m_vArrowInitPos;
    private Vector3 m_vArrowInitLocalPos;
}
