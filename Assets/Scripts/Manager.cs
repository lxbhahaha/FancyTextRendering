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

    // ����Topic�ĸ�����
    public Transform topicConentParent;
    // Topic����UI��Ԥ����
    public GameObject topicConentPrefab;
    public MarkdownRenderer markdownRenderer;

    #endregion

    // json���ļ�����
    private string topicListStr = "TopicList.json";
    // json�ļ���·��
    private string topicListJsonRootPath; // = Application.dataPath + "/ContentData/";
    // md�ļ���·��
    private string markdownRootPath;
    // topic���б�
    private List<Topic> topicList = new List<Topic>();

    private void Awake()
    {
        // ����·��
        topicListJsonRootPath = Application.dataPath + "/ContentData/";
        markdownRootPath = Application.dataPath + "/ContentData/Markdowns/";

        // ��ȡJson
        ReadJson();
    }

    private void Start()
    {
        // ��ʾȫ��
        Filter();
    }

    /// <summary>
    /// ���й���
    /// </summary>
    /// <param name="tags"></param>
    public void Filter(params string[] tags)
    {
        // ���ԭ������
        for (var i = 0; i < topicConentParent.transform.childCount; i++)
            Destroy(topicConentParent.transform.GetChild(i).gameObject);

        // û��ָ�� ȫ����ʾ
        if (tags.Length == 0)
            foreach (var item in topicList)
                    AddTopicTo(item, topicConentParent);
        // ���UI����
        else foreach(var item in topicList)
        {
            if(ContainStr(item.tags, tags))
                AddTopicTo(item, topicConentParent);
        }
    }

    /// <summary>
    /// ����Ƿ�ȫ������
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
    /// ��ȡTopic��json�ļ�
    /// </summary>
    public void ReadJson()
    {
        // ����������򷵻�
        if (!File.Exists(topicListJsonRootPath + topicListStr)) return;

        // ��ȡJson
        StreamReader streamReader = new StreamReader(topicListJsonRootPath + topicListStr);
        string jsonStr = streamReader.ReadToEnd();
        TopicArray tempArray = JsonUtility.FromJson<TopicArray>(jsonStr);

        // ת����List
        topicList.Clear();
        for (int i = 0; i < tempArray.allContents.Length; i++)
        {
            topicList.Add(tempArray.allContents[i]);
        }
    }

    /// <summary>
    /// ����Topic��json�ļ�
    /// </summary>
    public void SaveJson()
    {
        // ���·�������ھʹ���
        if (!Directory.Exists(topicListJsonRootPath))
            Directory.CreateDirectory(topicListJsonRootPath);

        // ת��Ϊ����
        TopicArray tempArray = new TopicArray(topicList.ToArray());

        // ת��ΪJson
        string json = JsonUtility.ToJson(tempArray);

        // ����Json
        File.WriteAllText(topicListJsonRootPath + topicListStr, json);
    }

    /// <summary>
    /// ��ȡmarkdown�����String
    /// </summary>
    /// <param name="fileName">�ļ���</param>
    /// <returns></returns>
    public string GetMarkdownString(string fileName)
    {
        string resString = "ERROR_�������ļ���" + fileName;

        // ���ļ����
        if (File.Exists(markdownRootPath + fileName + ".md"))
        {
            StreamReader streamReader = new StreamReader(markdownRootPath + fileName + ".md");
            resString = streamReader.ReadToEnd(); 
        }

        return resString;
    }

    /// <summary>
    /// ����markDown�ļ�
    /// </summary>
    /// <param name="topic">topic����</param>
    /// <param name="data">md�ļ�������</param>
    public void SaveMarkdown(Topic topic, string data)
    {
        // ���·�������ھʹ���
        if (!Directory.Exists(markdownRootPath))
            Directory.CreateDirectory(markdownRootPath);

        string path = markdownRootPath + topic.fileName + ".md";
        //// �Ѵ��ھͲ�����
        //if (File.Exists(path))
        //{
        //    Debug.LogError("??????????" + topic.fileName + ".md");
        //    return;
        //}

        // �����ļ�
        File.WriteAllText(path, data);
    }

    private void AddTopicTo(Topic topic, Transform parent)
    {
        // ��������
        GameObject tempObj = Instantiate(topicConentPrefab, parent);
        // ��������
        TopicUI tempTopicUI = tempObj.GetComponent<TopicUI>();
        tempTopicUI.SetData(topic);
        // ���ί��
        tempTopicUI.ClickAction = () => markdownRenderer.Source = GetMarkdownString(topic.fileName); 
    }
}