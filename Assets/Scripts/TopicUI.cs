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
    /// ���ʱ��ִ�еĺ���
    /// </summary>
    public Action ClickAction;

    public void OnPointerDown(PointerEventData eventData)
    {
        ClickAction();
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="topic"></param>
    public void SetData(Topic topic)
    {
        this.topic = topic;
        // ��������
        textName.text = topic.name;
        textSummary.text = topic.summary;

        // ���tag
        foreach (var item in topic.tags)
        {
            Instantiate(tagPrefab, tagsParent).GetComponentInChildren<TextMeshProUGUI>().text = item;
        }
    }
}
