using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class LogMessege : MonoBehaviour
{
    // ��Ϣ����
    [SerializeField]
    private GameObject messegeBoxObj;
    [SerializeField]
    private TextMeshProUGUI messegeBoxText;

    // ȷ�Ϻ���
    [SerializeField]
    private GameObject comfirmBoxObj;
    [SerializeField]
    private TextMeshProUGUI comfirmBoxText;
    [SerializeField]
    private Button comrirmButton;

    // ʵ��
    private static LogMessege _instance;
    public static LogMessege Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        // ����
        if (_instance != null)
            Destroy(this);
        _instance = this;
    }

    /// <summary>
    /// ��Ϣ����
    /// </summary>
    /// <param name="messege"></param>
    public void ShowMessege(string messege)
    {
        messegeBoxText.text = messege;
        messegeBoxObj.SetActive(true);
    }

    /// <summary>
    /// ����ȷ�ϵ���
    /// </summary>
    /// <param name="messege">��ʾ����</param>
    /// <param name="action">ȷ��ʱִ�еĺ���</param>
    public void ShowComfirmBox(string messege, UnityAction action)
    {
        comfirmBoxText.text = messege;
        comfirmBoxObj.SetActive(true);
        // ���ί��
        comrirmButton.onClick.AddListener(action);
    }
}
