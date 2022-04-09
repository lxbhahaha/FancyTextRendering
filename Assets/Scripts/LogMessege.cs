using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class LogMessege : MonoBehaviour
{
    // 消息盒子
    [SerializeField]
    private GameObject messegeBoxObj;
    [SerializeField]
    private TextMeshProUGUI messegeBoxText;

    // 确认盒子
    [SerializeField]
    private GameObject comfirmBoxObj;
    [SerializeField]
    private TextMeshProUGUI comfirmBoxText;
    [SerializeField]
    private Button comrirmButton;

    // 实例
    private static LogMessege _instance;
    public static LogMessege Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        // 单例
        if (_instance != null)
            Destroy(this);
        _instance = this;
    }

    /// <summary>
    /// 消息弹窗
    /// </summary>
    /// <param name="messege"></param>
    public void ShowMessege(string messege)
    {
        messegeBoxText.text = messege;
        messegeBoxObj.SetActive(true);
    }

    /// <summary>
    /// 弹出确认弹窗
    /// </summary>
    /// <param name="messege">提示内容</param>
    /// <param name="action">确认时执行的函数</param>
    public void ShowComfirmBox(string messege, UnityAction action)
    {
        comfirmBoxText.text = messege;
        comfirmBoxObj.SetActive(true);
        // 添加委托
        comrirmButton.onClick.AddListener(action);
    }
}
