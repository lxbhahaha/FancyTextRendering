using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LogicUI.FancyTextRendering;

public class Manager : MonoBehaviour
{
    #region UI

    // 放置Topic的父物体
    public Transform topicConentParent;
    // Topic物体UI的预制体
    public GameObject topicConentPrefab;
    public MarkdownRenderer markdownRenderer;
    // 标签dropdown
    public TMP_Dropdown tagDropdown;

    public FancyTextRendering.Demo.DemoRenderUpdater demoRenderUpdater;

    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textSummary;

    public TMP_InputField searchTextInput;

    #endregion

    public EditPanel editPanel;

    // json的文件名称
    private string topicListStr = "TopicList.json";
    // json文件的路径
    private string topicListJsonRootPath; // = Application.dataPath + "/ContentData/";
    // md文件的路径
    private string markdownRootPath;
    // topic的列表
    private List<Topic> topicList = new List<Topic>();
    // 保存已有的标签
    public HashSet<string> tagSet = new HashSet<string>();

    // 当前选择的Topic
    private Topic currentTopic = null;
    // 当前正在显示的所有TopicUI
    private List<TopicUI> showingTopicUIs = new List<TopicUI>();

    private void Awake()
    {
        // 计算路径
        topicListJsonRootPath = Application.dataPath + "/ContentData/";
        markdownRootPath = Application.dataPath + "/ContentData/Markdowns/";

        // 读取Json
        ReadJson();

        // 设置标签选项
        List<string> tagList = new List<string>();
        foreach (var tag in tagSet) tagList.Add(tag);
        tagDropdown.AddOptions(tagList);
    }

    private void Start()
    {
        // 显示全部
        Filter();
    }

    public void OnDropDownChange()
    {
        string tag = tagDropdown.captionText.text;
        if (tag == "全部")
            Filter();
        else
            Filter(tag);
    }

    /// <summary>
    /// 进行过滤
    /// </summary>
    /// <param name="tags"></param>
    public void Filter(params string[] tags)
    {
        // 清空原先内容
        showingTopicUIs.Clear();
        for (var i = 0; i < topicConentParent.transform.childCount; i++)
            Destroy(topicConentParent.transform.GetChild(i).gameObject);

        // 没有指定 全部显示
        if (tags.Length == 0)
            foreach (var item in topicList)
                    AddTopicTo(item, topicConentParent);
        // 添加UI物体
        else foreach(var item in topicList)
        {
            if(ContainStr(item.tags, tags))
                AddTopicTo(item, topicConentParent);
        }
    }

    /// <summary>
    /// 在当前显示的所有Topic中搜索
    /// </summary>
    /// <param name="target">搜索的内容</param>
    public void Search(string target)
    {
        List<TopicUI> deleteList = new List<TopicUI>();
        for (int i = 0; i < showingTopicUIs.Count; i++)
        {
            // 如果名字和简介都找不到这个搜索内容则进行记录
            if (!showingTopicUIs[i].topic.name.Contains(target) &&
                !showingTopicUIs[i].topic.summary.Contains(target))
            {
                deleteList.Add(showingTopicUIs[i]);
            }
        }
        // 对标记的内容进行删除
        for (int i = 0; i < deleteList.Count; i++)
        {
            Destroy(deleteList[i].gameObject);
            showingTopicUIs.Remove(deleteList[i]);
        }
    }

    /// <summary>
    /// 搜索输入框中的内容
    /// </summary>
    public void Search()
    {
        Search(searchTextInput.text);
    }

    /// <summary>
    /// 检查是否全部符合
    /// </summary>
    /// <param name="value"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool ContainStr(string[] value, string[] target)
    {
        bool finded = false;
        for (int i = 0; i < target.Length; i++)
        {
            finded = false;
            for (int j = 0; j < value.Length; j++)
            {
                if (value[j].Equals(target[i]))
                {
                    finded = true;
                    break;
                }
            }
            if(!finded) return false; 
        }
        return finded;
    }

    /// <summary>
    /// 读取Topic的json文件
    /// </summary>
    public void ReadJson()
    {
        // 如果不存在则返回
        if (!File.Exists(topicListJsonRootPath + topicListStr)) return;

        // 读取Json
        StreamReader streamReader = new StreamReader(topicListJsonRootPath + topicListStr);
        string jsonStr = streamReader.ReadToEnd();
        TopicArray tempArray = JsonUtility.FromJson<TopicArray>(jsonStr);

        // 转换成List
        topicList.Clear();
        for (int i = 0; i < tempArray.allContents.Length; i++)
        {
            topicList.Add(tempArray.allContents[i]);
            // 读取tag
            foreach (var tag in tempArray.allContents[i].tags)
                tagSet.Add(tag);
        }
    }

    /// <summary>
    /// 保存Topic成json文件
    /// </summary>
    public void SaveJson()
    {
        // 如果路径不存在就创建
        if (!Directory.Exists(topicListJsonRootPath))
            Directory.CreateDirectory(topicListJsonRootPath);

        // 转换为数组
        TopicArray tempArray = new TopicArray(topicList.ToArray());

        // 转换为Json
        string json = JsonUtility.ToJson(tempArray);

        // 保存Json
        File.WriteAllText(topicListJsonRootPath + topicListStr, json);
    }

    /// <summary>
    /// 获取markdown里面的String
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    public string GetMarkdownString(string fileName)
    {
        string resString = "ERROR_不存在文件：" + fileName;

        // 有文件则读
        if (File.Exists(markdownRootPath + fileName + ".md"))
        {
            StreamReader streamReader = new StreamReader(markdownRootPath + fileName + ".md");
            resString = streamReader.ReadToEnd(); 
        }

        return resString;
    }

    /// <summary>
    /// 保存markDown文件
    /// </summary>
    /// <param name="topic">topic内容</param>
    /// <param name="data">md文件的内容</param>
    public void SaveMarkdown(Topic topic, string data)
    {
        // 如果路径不存在就创建
        if (!Directory.Exists(markdownRootPath))
            Directory.CreateDirectory(markdownRootPath);

        string path = markdownRootPath + topic.fileName + ".md";

        // 保存文件
        File.WriteAllText(path, data);

        // 添加topic
        for (int i = 0; i < topicList.Count; i++)
        {
            if(topicList[i].name == topic.name)
            {
                topicList.RemoveAt(i);
                break;
            }
        }
        topicList.Add(topic);

        // 保存一下json
        SaveJson();
    }

    /// <summary>
    /// 添加topic到UI中
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="parent"></param>
    private void AddTopicTo(Topic topic, Transform parent)
    {
        // 创建物体
        GameObject tempObj = Instantiate(topicConentPrefab, parent);
        // 设置内容
        TopicUI tempTopicUI = tempObj.GetComponent<TopicUI>();
        tempTopicUI.SetData(topic);
        // 添加委托
        tempTopicUI.ClickAction = () => { 
            markdownRenderer.Source = GetMarkdownString(topic.fileName); 
            currentTopic = topic;
            textTitle.text = topic.name;
            textSummary.text = topic.summary;
        };
        // 记录当前TopicUI为正在显示
        showingTopicUIs.Add(tempTopicUI);
    }

    /// <summary>
    /// 编辑当前选择的md
    /// </summary>
    public void Edit()
    {
        // 检查现在是否有选择
        if(currentTopic == null)
        {
            LogMessege.Instance.ShowMessege("当前没有选中的MarkDown文件");
            return;
        }
        
        // 把内容传过去
        editPanel.inputTitle.text = currentTopic.name;
        editPanel.inputSummary.text = currentTopic.summary;
        editPanel.inputContent.text = markdownRenderer.Source;
        editPanel.ClearTags();
        foreach (var tag in currentTopic.tags)
            editPanel.AddTag(tag);

        // 更新md显示
        demoRenderUpdater.UpdateRender();
    }

    /// <summary>
    /// 删除当前选择的md
    /// </summary>
    public void Delete()
    {
        // 检查现在是否有选择
        if (currentTopic == null)
        {
            LogMessege.Instance.ShowMessege("当前没有选中的MarkDown文件");
            return;
        }

        LogMessege.Instance.ShowComfirmBox("是否真的删除" + currentTopic.name + "？删除无法找回。", DeleteAction);
    }
    private void DeleteAction()
    {
        // 没有文件则提示返回
        if (!File.Exists(markdownRootPath + currentTopic.fileName + ".md"))
        {
            // TODO 暂无操作
        }
        else
        {
            // 删除文件
            File.Delete(markdownRootPath + currentTopic.fileName + ".md");
        }

        // 删除Topic
        topicList.Remove(currentTopic);
        // 保存一下json
        SaveJson();

        // 调用一下筛选的函数
        OnDropDownChange();

        LogMessege.Instance.ShowMessege("删除成功");
    }
}