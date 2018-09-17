using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SlideScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [LabelText("单元格长度")] public int cellLength;
    [LabelText("间隔")] public int spacing;
    [LabelText("左偏移")] public int leftOffset;
    [LabelText("单元格数量")] public int allItemNumber;


    private RectTransform m_ContentTrans;
    private ScrollRect m_ScrollRect;

    private float m_BeginMousePosX;
    private float m_EndMousePosX;

    private Vector3 m_CurrentContentLocalPos;

    private float m_EachItemMoveLength;

    private int m_CurrentIndex;


    private void Awake()
    {
        
        m_ScrollRect = GetComponent<ScrollRect>();
        m_ContentTrans = m_ScrollRect.content;

        m_EachItemMoveLength = cellLength + spacing;

        m_CurrentContentLocalPos = m_ContentTrans.localPosition;
        m_CurrentIndex = 1;

        Debug.Log(m_CurrentContentLocalPos);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        m_BeginMousePosX = Input.mousePosition.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_EndMousePosX = Input.mousePosition.x;
        float offsetX = 0;
        float moveDistance = 0;
        offsetX = m_BeginMousePosX - m_EndMousePosX;

        if (offsetX > 0) // 左滑
        {
            if (m_CurrentIndex >= allItemNumber)
                return;

            moveDistance = -m_EachItemMoveLength;
            m_CurrentIndex++;
        }
        else // 右滑
        {
            if (m_CurrentIndex <= 1)
                return;

            moveDistance = m_EachItemMoveLength;
            m_CurrentIndex--;
        }

        m_CurrentContentLocalPos += new Vector3(moveDistance, 0, 0);

        Debug.Log(m_CurrentContentLocalPos);

        m_ContentTrans.DOLocalMoveX(m_CurrentContentLocalPos.x, 0.2f).SetEase(Ease.InOutQuint);

        
    }
}