using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;
using UnityEditor;

public class UIManager
{
    private static UIManager _instance;//��ŵ�ǰ���ʵ�����󣬾�̬ȫ��Ψһ

    private readonly string _uIPanelPrefabPath = Application.dataPath + @"/Resources/UIPanelPrefabs";//ֻ��UIPanel��Prefab�ļ���·��
    private readonly string _jsonPath = Application.streamingAssetsPath + @"/Json/UIPanel/UIPanel.json";//�洢UIPanel��Ϣ��Json�ļ�·��

    private List<UIPanel> _currentUIPanelList;//��ǰ�����е�UIPanel�б�

    private Transform _canvasTransform;//��ŵ�ǰ������canvas�����Transform���

    private Dictionary<UIPanelType, BasePanel> _panelDict;

    private Stack<BasePanel> _currentBasePanelStack;

    //���췽��
    private UIManager()
    {
        UIPanelInfoSaveInJson();//�����౻ʵ����ʱ���͵��ø÷���
        _panelDict = new Dictionary<UIPanelType, BasePanel>();
        foreach (Transform child in GameObject.Find("Canvas").transform)
        {
            if (child.gameObject.tag == "UIPanel")
            {
                _panelDict.Add((UIPanelType)Enum.Parse(typeof(UIPanelType), child.gameObject.name), child.gameObject.GetComponent<BasePanel>());
            }
        }
    }

    //����ģʽ����֤�á�UI�����ࡱֻ�ᱻʵ����һ��
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIManager();
            return _instance;
        }
    }

    //����ģʽ��ȷ����ȡ����Transform���ֻ��һ��
    public Transform CanvasTransform
    {
        get
        {
            if (_canvasTransform == null)
                _canvasTransform = GameObject.Find("Canvas").transform;
            return _canvasTransform;
        }
    }

    /// <summary>
    /// ��JSON�ļ���������һ�������б�
    /// </summary>
    /// <param name="jsonPath">Json�ļ�·��</param>
    /// <returns name="list">UIPanel�б�</returns>
    public List<UIPanel> ReadJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            File.WriteAllText(jsonPath, "[]");
        }

        List<UIPanel> list = JsonMapper.ToObject<List<UIPanel>>(File.ReadAllText(jsonPath));

        return list;
    }

    /// <summary>
    /// �Զ��Ѵ����List����д��Json�ļ�
    /// </summary>
    /// <param name="jsonPath">Json�ļ�·��</param>
    /// <param name="list">UIPanel�б�</param>
    public void WriteJson(string jsonPath, List<UIPanel> list)
    {
        string jsonData = JsonMapper.ToJson(list);
        File.WriteAllText(jsonPath, jsonData);
    }

    /// <summary>
    /// �Զ�����UIPanel��Json�ļ�
    /// </summary>
    public void UIPanelInfoSaveInJson()
    {
        _currentUIPanelList = ReadJson(_jsonPath);//��ȡ��ǰJson�ļ��ڵ�UIPanel�б���Ϣ

        //��ȡ���UIPanel��Prefab���ļ��е���Ϣ
        DirectoryInfo folder = new DirectoryInfo(_uIPanelPrefabPath);

        //�����ļ�����ÿһ��prefab��չ�������֣���������ת��Ϊ��Ӧ��UIPanelType
        //�ټ��UIPanelType�Ƿ����List����ڡ���List����ڣ������path����List�ﲻ���ڣ�����ϡ�
        #region
        foreach (var file in folder.GetFiles("*.prefab"))
        {
            //����Ӧ�ļ���ת����UIPanel����
            UIPanelType uIPanelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), file.Name.Replace(".prefab", ""));

            //��ȡ��ǰUIPanel��Prefab�ļ���·����������չ����
            string path = @"UIPanelPrefabs/" + file.Name.Replace(".prefab", "");

            UIPanel uIPanel = _currentUIPanelList.SearchPanelForType(uIPanelType);

            if (uIPanel != null)
            {
                uIPanel._uIPanelPath = path;
            }
            else
            {
                UIPanel tmpUIPanel = new UIPanel(uIPanelType, path);
                _currentUIPanelList.Add(tmpUIPanel);
            }
        }
        #endregion

        WriteJson(_jsonPath, _currentUIPanelList);
        //AssetDatabase.Refresh();
    }

    /// <summary>
    /// ����UIPanel�����ͣ�����ʵ������Ӧ��Prefab����
    /// </summary>
    /// <param name="type">UIPanel������</param>
    /// <returns>BasePanel����</returns>
    /// <exception cref="Exception"></exception>
    public BasePanel GetPanel(UIPanelType type)
    {
        if (_panelDict == null)
        {
            _panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        BasePanel panel = _panelDict.TryGetValue(type);//��ȡ�ֵ��е�BasePanelʵ������

        if (panel == null)
        {
            //��ȡ��ǰUIPanel���͵�·��
            string path = _currentUIPanelList.SearchPanelForType(type)._uIPanelPath;
            if (path == null)
                throw new Exception("�Ҳ���UIPanel�����Prefab��Ϣ");
            if (Resources.Load(path) == null)
                throw new Exception("�Ҳ����ļ��������Prefab�ļ�");

            //ʵ����UIPanel��Prefab����
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            //�������ΪCanvas��������
            instPanel.transform.SetParent(CanvasTransform, false);

            _panelDict.Add(type, instPanel.GetComponent<BasePanel>());

            return instPanel.GetComponent<BasePanel>();
        }

        return panel;
    }

    /// <summary>
    /// ���´򿪵�UIPanel��ӽ�ջ�У������ö�ӦUIPanel��״̬
    /// </summary>
    /// <param name="type"></param>
    public void PushPanel(UIPanelType type)
    {
        if (_currentBasePanelStack == null)
            _currentBasePanelStack = new Stack<BasePanel>();

        //��Panel��ջ��ԭջ����Panel������ͣ�����ж�ջ������Panel���ݣ�
        if (_currentBasePanelStack.Count > 0)
        {
            BasePanel panel_1 = _currentBasePanelStack.Peek();
            panel_1.OnPause();
        }

        BasePanel panel_2 = GetPanel(type);
        _currentBasePanelStack.Push(panel_2);
        panel_2.OnStart();
    }

    public void PopPanel()
    {
        if (_currentBasePanelStack == null)
            _currentBasePanelStack = new Stack<BasePanel>();
        if (_currentBasePanelStack.Count <= 0) return;

        //��ջ��Ԫ�س�ջ�������ö�ӦUIPanel��״̬
        BasePanel panel_1 = _currentBasePanelStack.Pop();
        panel_1.OnExit();

        if (_currentBasePanelStack.Count <= 0) return;

        BasePanel panel_2 = _currentBasePanelStack.Peek();
        panel_2.OnResume();
    }
}