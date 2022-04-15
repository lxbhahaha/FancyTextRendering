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
    // ��ǩdropdown
    public TMP_Dropdown tagDropdown;

    public FancyTextRendering.Demo.DemoRenderUpdater demoRenderUpdater;

    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textSummary;

    public TMP_InputField searchTextInput;

    #endregion

    public EditPanel editPanel;

    // json���ļ�����
    private string topicListStr = "TopicList.json";
    // json�ļ���·��
    private string topicListJsonRootPath; // = Application.dataPath + "/ContentData/";
    // md�ļ���·��
    private string markdownRootPath;
    // topic���б�
    private List<Topic> topicList = new List<Topic>();
    // �������еı�ǩ
    public HashSet<string> tagSet = new HashSet<string>();

    // ��ǰѡ���Topic
    private Topic currentTopic = null;
    // ��ǰ������ʾ������TopicUI
    private List<TopicUI> showingTopicUIs = new List<TopicUI>();

    private void Awake()
    {
        // ����·��
        topicListJsonRootPath = Application.dataPath + "/ContentData/";
        markdownRootPath = Application.dataPath + "/ContentData/Markdowns/";

        // ��ȡJson
        ReadJson();

        // ���ñ�ǩѡ��
        List<string> tagList = new List<string>();
        foreach (var tag in tagSet) tagList.Add(tag);
        tagDropdown.AddOptions(tagList);
    }

    private void Start()
    {
        // ��ʾȫ��
        Filter();
    }

    public void OnDropDownChange()
    {
        string tag = tagDropdown.captionText.text;
        if (tag == "ȫ��")
            Filter();
        else
            Filter(tag);
    }

    /// <summary>
    /// ���й���
    /// </summary>
    /// <param name="tags"></param>
    public void Filter(params string[] tags)
    {
        // ���ԭ������
        showingTopicUIs.Clear();
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
    /// �ڵ�ǰ��ʾ������Topic������
    /// </summary>
    /// <param name="target">����������</param>
    public void Search(string target)
    {
        List<TopicUI> deleteList = new List<TopicUI>();
        for (int i = 0; i < showingTopicUIs.Count; i++)
        {
            // ������ֺͼ�鶼�Ҳ������������������м�¼
            if (!showingTopicUIs[i].topic.name.Contains(target) &&
                !showingTopicUIs[i].topic.summary.Contains(target))
            {
                deleteList.Add(showingTopicUIs[i]);
            }
        }
        // �Ա�ǵ����ݽ���ɾ��
        for (int i = 0; i < deleteList.Count; i++)
        {
            Destroy(deleteList[i].gameObject);
            showingTopicUIs.Remove(deleteList[i]);
        }
    }

    /// <summary>
    /// ����������е�����
    /// </summary>
    public void Search()
    {
        Search(searchTextInput.text);
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
            // ��ȡtag
            foreach (var tag in tempArray.allContents[i].tags)
                tagSet.Add(tag);
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

        // �����ļ�
        File.WriteAllText(path, data);

        // ���topic
        for (int i = 0; i < topicList.Count; i++)
        {
            if(topicList[i].name == topic.name)
            {
                topicList.RemoveAt(i);
                break;
            }
        }
        topicList.Add(topic);

        // ����һ��json
        SaveJson();
    }

    /// <summary>
    /// ���topic��UI��
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="parent"></param>
    private void AddTopicTo(Topic topic, Transform parent)
    {
        // ��������
        GameObject tempObj = Instantiate(topicConentPrefab, parent);
        // ��������
        TopicUI tempTopicUI = tempObj.GetComponent<TopicUI>();
        tempTopicUI.SetData(topic);
        // ���ί��
        tempTopicUI.ClickAction = () => { 
            markdownRenderer.Source = GetMarkdownString(topic.fileName); 
            currentTopic = topic;
            textTitle.text = topic.name;
            textSummary.text = topic.summary;
        };
        // ��¼��ǰTopicUIΪ������ʾ
        showingTopicUIs.Add(tempTopicUI);
    }

    /// <summary>
    /// �༭��ǰѡ���md
    /// </summary>
    public void Edit()
    {
        // ��������Ƿ���ѡ��
        if(currentTopic == null)
        {
            LogMessege.Instance.ShowMessege("��ǰû��ѡ�е�MarkDown�ļ�");
            return;
        }
        
        // �����ݴ���ȥ
        editPanel.inputTitle.text = currentTopic.name;
        editPanel.inputSummary.text = currentTopic.summary;
        editPanel.inputContent.text = markdownRenderer.Source;
        editPanel.ClearTags();
        foreach (var tag in currentTopic.tags)
            editPanel.AddTag(tag);

        // ����md��ʾ
        demoRenderUpdater.UpdateRender();
    }

    /// <summary>
    /// ɾ����ǰѡ���md
    /// </summary>
    public void Delete()
    {
        // ��������Ƿ���ѡ��
        if (currentTopic == null)
        {
            LogMessege.Instance.ShowMessege("��ǰû��ѡ�е�MarkDown�ļ�");
            return;
        }

        LogMessege.Instance.ShowComfirmBox("�Ƿ����ɾ��" + currentTopic.name + "��ɾ���޷��һء�", DeleteAction);
    }
    private void DeleteAction()
    {
        // û���ļ�����ʾ����
        if (!File.Exists(markdownRootPath + currentTopic.fileName + ".md"))
        {
            // TODO ���޲���
        }
        else
        {
            // ɾ���ļ�
            File.Delete(markdownRootPath + currentTopic.fileName + ".md");
        }

        // ɾ��Topic
        topicList.Remove(currentTopic);
        // ����һ��json
        SaveJson();

        // ����һ��ɸѡ�ĺ���
        OnDropDownChange();

        LogMessege.Instance.ShowMessege("ɾ���ɹ�");
    }
}