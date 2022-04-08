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

    #endregion

    // json的文件名称
    private string topicListStr = "TopicList.json";
    // json文件的路径
    private string topicListJsonRootPath; // = Application.dataPath + "/ContentData/";
    // md文件的路径
    private string markdownRootPath;
    // topic的列表
    private List<Topic> topicList = new List<Topic>();

    private void Awake()
    {
        // 计算路径
        topicListJsonRootPath = Application.dataPath + "/ContentData/";
        markdownRootPath = Application.dataPath + "/ContentData/Markdowns/";

        // 读取Json
        ReadJson();
    }

    private void Start()
    {
        // 显示全部
        Filter();
    }

    /// <summary>
    /// 进行过滤
    /// </summary>
    /// <param name="tags"></param>
    public void Filter(params string[] tags)
    {
        // 清空原先内容
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
        //// 已存在就不保存
        //if (File.Exists(path))
        //{
        //    Debug.LogError("??????????" + topic.fileName + ".md");
        //    return;
        //}

        // 保存文件
        File.WriteAllText(path, data);
    }

    private void AddTopicTo(Topic topic, Transform parent)
    {
        // 创建物体
        GameObject tempObj = Instantiate(topicConentPrefab, parent);
        // 设置内容
        TopicUI tempTopicUI = tempObj.GetComponent<TopicUI>();
        tempTopicUI.SetData(topic);
        // 添加委托
        tempTopicUI.ClickAction = () => markdownRenderer.Source = GetMarkdownString(topic.fileName); 
    }
}