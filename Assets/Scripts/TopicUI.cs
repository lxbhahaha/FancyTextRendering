using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public class TopicUI : MonoBehaviour, IPointerDownHandler
{
    public GameObject tagPrefab;
    public Topic topic;

    public TextMeshProUGUI textName;
    public TextMeshProUGUI textSummary;
    public Transform tagsParent;

    /// <summary>
    /// 点击时候执行的函数
    /// </summary>
    public Action ClickAction;

    public void OnPointerDown(PointerEventData eventData)
    {
        ClickAction();
    }

    /// <summary>
    /// 设置内容
    /// </summary>
    /// <param name="topic"></param>
    public void SetData(Topic topic)
    {
        this.topic = topic;
        // 设置文字
        textName.text = topic.name;
        textSummary.text = topic.summary;

        // 添加tag
        foreach (var item in topic.tags)
        {
            Instantiate(tagPrefab, tagsParent).GetComponentInChildren<TextMeshProUGUI>().text = item;
        }
    }
}
