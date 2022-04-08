using System;

[Serializable]
public class Topic 
{
    // 主题名称
    public string name;

    // 主题带有的标签
    public string[] tags;

    // 内容的摘要
    public string summary;

    // 文件的名称
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
    // 记录所有已标记的相对地址
    public Topic[] allContents;

    public TopicArray(Topic[] value)
    {
        allContents = value;
    }
}