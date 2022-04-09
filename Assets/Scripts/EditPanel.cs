using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditPanel : MonoBehaviour
{
    public Manager manager;

    #region UI

    public TMP_InputField inputTitle;
    public TMP_InputField inputSummary;
    public TMP_InputField inputContent;
    public TMP_Dropdown dropdownTags;
    public Transform tagsParent;

    public GameObject tagPrefab;

    #endregion

    private void OnEnable()
    {
        InitTagDropDown();
    }

    private void OnDisable()
    {
        manager.tagDropdown.value = 0;
        manager.Filter();
    }

    // ��ʼ��tag��ѡ��
    private void InitTagDropDown()
    {
        // ���
        dropdownTags.options.Clear();

        // ���ѡ��
        List<string> tagList = new List<string>();
        foreach (var tag in manager.tagSet) tagList.Add(tag);
        dropdownTags.AddOptions(tagList);
    }

    /// <summary>
    /// ��ӱ�ǩ
    /// </summary>
    public void AddTag()
    {
        // ��ȡ��ǰѡ��ı�ǩ
        string tag = dropdownTags.captionText.text;

        // �鿴�Ƿ��Ѿ�����
        for (int i = 0; i < tagsParent.childCount; i++)
        {
            if(tagsParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text == tag)
            {
                // ���д˱�ǩ�����
                return;
            }
        }

        // ��ӱ�ǩ
        Instantiate(tagPrefab, tagsParent).GetComponentInChildren<TextMeshProUGUI>().text = tag;
    }
    /// <summary>
    /// ֱ����ӱ�ǩ
    /// </summary>
    /// <param name="tag"></param>
    public void AddTag(string tag)
    {
        // �鿴�Ƿ��Ѿ�����
        for (int i = 0; i < tagsParent.childCount; i++)
        {
            if (tagsParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text == tag)
            {
                // ���д˱�ǩ�����
                return;
            }
        }

        // ��ӱ�ǩ
        Instantiate(tagPrefab, tagsParent).GetComponentInChildren<TextMeshProUGUI>().text = tag;
    }

    /// <summary>
    /// �����md�ļ�
    /// </summary>
    public void Save()
    {
        // û�б�ǩ������
        if(tagsParent.childCount == 0)
        {
            LogMessege.Instance.ShowMessege("��Ҫ�������һ����ǩ");
            return;
        }
        if (inputTitle.text == "")
        {
            LogMessege.Instance.ShowMessege("���ⲻ��Ϊ��");
            return;
        }

        // ��ȡ���б�ǩ
        List<string> tagsList = new List<string>();
        for (int i = 0; i < tagsParent.childCount; i++)
        {
            tagsList.Add(tagsParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text);
        }

        // ����������
        Topic topic = new Topic(inputTitle.text, inputTitle.text, inputSummary.text, tagsList.ToArray());
        manager.SaveMarkdown(topic, inputContent.text);
        LogMessege.Instance.ShowMessege("����ɹ�");
    }
}
