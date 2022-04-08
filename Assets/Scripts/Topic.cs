using System;

[Serializable]
public class Topic 
{
    // ��������
    public string name;

    // ������еı�ǩ
    public string[] tags;

    // ���ݵ�ժҪ
    public string summary;

    // �ļ�������
    public string fileName;

    public Topic(string name, string fileName, string summary, params string[] tags)
    {
        this.name = name;
        this.fileName = fileName;
        this.summary = summary;
        this.tags = tags;
    }
}

[Serializable]
public class TopicArray
{
    // ��¼�����ѱ�ǵ���Ե�ַ
    public Topic[] allContents;

    public TopicArray(Topic[] value)
    {
        allContents = value;
    }
}