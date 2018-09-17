using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlideCanCoverScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [LabelText("单元格长度")] public int cellLength;
    [LabelText("间隔")] public int spacing;
    [LabelText("左偏移")] public int leftOffset;
    [LabelText("单元格数量")] public int allItemNumber;


    private ScrollRect m_ScrollRect;

    /// <summary>
    /// 滚动画布的长度
    /// </summary>
    private float m_ContentLength;

    /// <summary>
    /// 鼠标开始位置
    /// </summary>
    private float m_BeginMousePosX;

    /// <summary>
    /// 鼠标结束位置
    /// </summary>
    private float m_EndMousePosX;

    /// <summary>
    /// 上一次位置的比例
    /// </summary>
    private float m_LastProportion;

    /// <summary>
    /// 上限值
    /// </summary>
    private float m_MaxLimit;

    /// <summary>
    /// 下限值
    /// </summary>
    private float m_MinLimit;

    /// <summary>
    /// 移动第一个单元格的距离
    /// </summary>
    private float m_FirstItemLength;

    /// <summary>
    /// 滑动一个单元格需要的距离
    /// </summary>
    private float m_EachItemLength;

    /// <summary>
    /// 滑动一个单元格所占比例
    /// </summary>
    private float m_EachItemProportion;

    private int m_CurrentIndex;


    private void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
        m_ContentLength = m_ScrollRect.content.rect.xMax - 2 * leftOffset - cellLength;
        m_FirstItemLength = cellLength / 2 + leftOffset;
        m_EachItemLength = cellLength + spacing;
        m_EachItemProportion = m_EachItemLength / m_ContentLength;
        m_MinLimit = m_FirstItemLength / m_ContentLength;
        m_MaxLimit = 1 - m_MinLimit;
        m_CurrentIndex = 0;
        m_ScrollRect.horizontalNormalizedPosition = 0;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        m_BeginMousePosX = Input.mousePosition.x;
        Debug.Log(m_BeginMousePosX);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float offsetX = 0;
        m_EndMousePosX = Input.mousePosition.x;

        offsetX = (m_BeginMousePosX - m_EndMousePosX) * 2;

        if (Mathf.Abs(offsetX) > m_FirstItemLength)
        {
            if (offsetX > 0) // 向右滑动
            {
                if (m_CurrentIndex >= allItemNumber)
                    return;

                // 当次可以移动的格子数目
                int moveCount = (int)((offsetX - m_FirstItemLength) / m_EachItemLength) + 1;
                m_CurrentIndex += moveCount;
                if (m_CurrentIndex >= allItemNumber)
                    m_CurrentIndex = allItemNumber;

                // 当次需要移动的比例
                m_LastProportion += m_EachItemProportion * moveCount;
                if (m_LastProportion >= m_MaxLimit)
                    m_LastProportion = 1;
            }
            else // 向左滑动
            {
                if (m_CurrentIndex <= 1)
                    return;

                // 当次可以移动的格子数目
                int moveCount = (int)((offsetX + m_FirstItemLength) / m_EachItemLength) - 1;
                m_CurrentIndex += moveCount;
                if (m_CurrentIndex <= 1)
                    m_CurrentIndex = 1;

                // 当次需要移动的比例
                m_LastProportion += m_EachItemProportion * moveCount;
                if (m_LastProportion <= m_MinLimit)
                    m_LastProportion = 0;
            }
        }

        DOTween.To(() => m_ScrollRect.horizontalNormalizedPosition,
            lerpValue => m_ScrollRect.horizontalNormalizedPosition = lerpValue,
            m_LastProportion, 0.5f).SetEase(Ease.InOutQuint);
    }
}