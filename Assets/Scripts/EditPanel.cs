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

    // 初始化tag的选择
    private void InitTagDropDown()
    {
        // 清空
        dropdownTags.options.Clear();

        // 添加选项
        List<string> tagList = new List<string>();
        foreach (var tag in manager.tagSet) tagList.Add(tag);
        dropdownTags.AddOptions(tagList);
    }

    /// <summary>
    /// 添加标签
    /// </summary>
    public void AddTag()
    {
        // 获取当前选择的标签
        string tag = dropdownTags.captionText.text;

        // 查看是否已经存在
        for (int i = 0; i < tagsParent.childCount; i++)
        {
            if(tagsParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text == tag)
            {
                // 已有此标签不添加
                return;
            }
        }

        // 添加标签
        Instantiate(tagPrefab, tagsParent).GetComponentInChildren<TextMeshProUGUI>().text = tag;
    }
    /// <summary>
    /// 直接添加标签
    /// </summary>
    /// <param name="tag"></param>
    public void AddTag(string tag)
    {
        // 查看是否已经存在
        for (int i = 0; i < tagsParent.childCount; i++)
        {
            if (tagsParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text == tag)
            {
                // 已有此标签不添加
                return;
            }
        }

        // 添加标签
        Instantiate(tagPrefab, tagsParent).GetComponentInChildren<TextMeshProUGUI>().text = tag;
    }

    /// <summary>
    /// 保存该md文件
    /// </summary>
    public void Save()
    {
        // 没有标签不保存
        if(tagsParent.childCount == 0)
        {
            LogMessege.Instance.ShowMessege("需要添加至少一个标签");
            return;
        }
        if (inputTitle.text == "")
        {
            LogMessege.Instance.ShowMessege("标题不能为空");
            return;
        }

        // 获取所有标签
        List<string> tagsList = new List<string>();
        for (int i = 0; i < tagsParent.childCount; i++)
        {
            tagsList.Add(tagsParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text);
        }

        // 创建并保存
        Topic topic = new Topic(inputTitle.text, inputTitle.text, inputSummary.text, tagsList.ToArray());
        manager.SaveMarkdown(topic, inputContent.text);
        LogMessege.Instance.ShowMessege("保存成功");
    }
}
