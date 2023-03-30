using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;
using UnityEditor;

public class UIManager
{
    private static UIManager _instance;//存放当前类的实例对象，静态全局唯一

    private readonly string _uIPanelPrefabPath = Application.dataPath + @"/Resources/UIPanelPrefabs";//只读UIPanel的Prefab文件的路径
    private readonly string _jsonPath = Application.streamingAssetsPath + @"/Json/UIPanel/UIPanel.json";//存储UIPanel信息的Json文件路径

    private List<UIPanel> _currentUIPanelList;//当前程序中的UIPanel列表

    private Transform _canvasTransform;//存放当前场景的canvas对象的Transform组件

    private Dictionary<UIPanelType, BasePanel> _panelDict;

    private Stack<BasePanel> _currentBasePanelStack;

    //构造方法
    private UIManager()
    {
        UIPanelInfoSaveInJson();//当该类被实例化时，就调用该方法
        _panelDict = new Dictionary<UIPanelType, BasePanel>();
        foreach (Transform child in GameObject.Find("Canvas").transform)
        {
            if (child.gameObject.tag == "UIPanel")
            {
                _panelDict.Add((UIPanelType)Enum.Parse(typeof(UIPanelType), child.gameObject.name), child.gameObject.GetComponent<BasePanel>());
            }
        }
    }

    //单列模式，保证该“UI管理类”只会被实例化一次
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIManager();
            return _instance;
        }
    }

    //单列模式，确保获取到的Transform组件只有一份
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
    /// 读JSON文件，并返回一个数据列表
    /// </summary>
    /// <param name="jsonPath">Json文件路径</param>
    /// <returns name="list">UIPanel列表</returns>
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
    /// 自动把传入的List数据写入Json文件
    /// </summary>
    /// <param name="jsonPath">Json文件路径</param>
    /// <param name="list">UIPanel列表</param>
    public void WriteJson(string jsonPath, List<UIPanel> list)
    {
        string jsonData = JsonMapper.ToJson(list);
        File.WriteAllText(jsonPath, jsonData);
    }

    /// <summary>
    /// 自动更新UIPanel的Json文件
    /// </summary>
    public void UIPanelInfoSaveInJson()
    {
        _currentUIPanelList = ReadJson(_jsonPath);//读取当前Json文件内的UIPanel列表信息

        //获取存放UIPanel的Prefab的文件夹的信息
        DirectoryInfo folder = new DirectoryInfo(_uIPanelPrefabPath);

        //遍历文件夹里每一个prefab扩展名的名字，并把名字转换为对应的UIPanelType
        //再检查UIPanelType是否存在List里存在。若List里存在，则更新path；若List里不存在，则加上。
        #region
        foreach (var file in folder.GetFiles("*.prefab"))
        {
            //将对应文件名转化成UIPanel类型
            UIPanelType uIPanelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), file.Name.Replace(".prefab", ""));

            //获取当前UIPanel的Prefab文件夹路径（不加扩展名）
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
    /// 输入UIPanel的类型，就能实例化对应的Prefab对象
    /// </summary>
    /// <param name="type">UIPanel的类型</param>
    /// <returns>BasePanel对象</returns>
    /// <exception cref="Exception"></exception>
    public BasePanel GetPanel(UIPanelType type)
    {
        if (_panelDict == null)
        {
            _panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        BasePanel panel = _panelDict.TryGetValue(type);//获取字典中的BasePanel实例对象

        if (panel == null)
        {
            //获取当前UIPanel类型的路径
            string path = _currentUIPanelList.SearchPanelForType(type)._uIPanelPath;
            if (path == null)
                throw new Exception("找不到UIPanel里面的Prefab信息");
            if (Resources.Load(path) == null)
                throw new Exception("找不到文件夹里面的Prefab文件");

            //实例化UIPanel的Prefab对象
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            //设置面板为Canvas的子物体
            instPanel.transform.SetParent(CanvasTransform, false);

            _panelDict.Add(type, instPanel.GetComponent<BasePanel>());

            return instPanel.GetComponent<BasePanel>();
        }

        return panel;
    }

    /// <summary>
    /// 把新打开的UIPanel添加进栈中，并设置对应UIPanel的状态
    /// </summary>
    /// <param name="type"></param>
    public void PushPanel(UIPanelType type)
    {
        if (_currentBasePanelStack == null)
            _currentBasePanelStack = new Stack<BasePanel>();

        //新Panel入栈，原栈顶的Panel设置暂停（先判断栈里有无Panel数据）
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

        //把栈顶元素出栈，并设置对应UIPanel的状态
        BasePanel panel_1 = _currentBasePanelStack.Pop();
        panel_1.OnExit();

        if (_currentBasePanelStack.Count <= 0) return;

        BasePanel panel_2 = _currentBasePanelStack.Peek();
        panel_2.OnResume();
    }
}