using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sov.MessageNotificationCenter;

//using Event = Sov.EventDispatchCenter;
//TODO: DEBUG类
namespace Sov.AVGPart
{
    public class OpCommand
    {
        public OpCommand(Opcode OpType)
        {
            Op = OpType;
            AdditionalOp = new List<Opcode>();
            Params = new Dictionary<string, string>();
        }

        public Opcode Op;

        //public string TagName

        //Params: contains the param write in the script
        public Dictionary<string, string> Params
        {
            get;
            set;
        }

        public List<Opcode> AdditionalOp
        {
            get;
            set;
        }
        public int LineID
        {
            get;
            set;
        }
    };


    public class ScriptEngine: MonoBehaviour
    {
        static ScriptEngine _sharedScriptedEngine = null;

        public string ScriptFilePath = "/AVGPart/Resources/ScenarioScripts/";

        public class EngineStatus
        {
            public bool SkipThisTag
            {
                get;
                internal set;
            }

            public string CurrentScenario
            {
                get;
                internal set;
            }

            public bool EnableClickContinue
            {
                get;
                internal set;
            }

            public bool EnableNextCommand
            {
                get;
                internal set;
            }

            public EngineStatus()
            {
                Reset();
            }

            internal void Reset()
            {
                //Status reset
                CurrentScenario = "";
                EnableClickContinue = true;
                EnableNextCommand = true;
                SkipThisTag = false;
            }
        
        }
        public EngineStatus Status;

       // List<OpCommand> _scenarioLines;
        List<OpCommand> _opLines;

        List<AbstractTag> _opTags;

        //Directory?
        //用于记录场景所指的行号
        Dictionary<string, int> _scenarioDict;

        int _currentLine        = 0;

        KAGPhraser _phraser;

        
        #region Public Method

        public virtual bool Init()
        {
            _phraser = new KAGPhraser();

            _opLines = new List<OpCommand>();

            _scenarioDict = new Dictionary<string, int>();

            _opTags = new List<AbstractTag>();

            Status = new EngineStatus();
            
            return true;
        }

        void Awake()
        {
            //注册监听事件
            MessageListener listener = new MessageListener("SCRIPT_CLICK_CONTINUE",
                                                 OnClickContinue);

            MessageDispatcher.Instance.RegisterMessageListener(listener);
        }

        public static ScriptEngine Instance
        {
            get
            {
                if (_sharedScriptedEngine == null)
                {
                    GameObject go = new GameObject("ScriptEngine");
                    Debug.Log("create ScriptEngine");
                    if(go == null)
                    {
                        Debug.LogError("ScriptEngine Create Wrong!");
                    }
                    _sharedScriptedEngine =  go.AddComponent<ScriptEngine>();
                    _sharedScriptedEngine.Init();
                }
                return _sharedScriptedEngine;
            }
        }
                                     
        public void AddCommand(OpCommand op)
        {
            op.LineID = GetLastedLineNo();

            _opLines.Add(op);
        }

        public void AddCommand(AbstractTag tag)
        {
            //tag.LineNo = _opTags.Count;
            tag.Engine = this;
            if(tag.Name == "scenario")
            {
                AddScenario(tag);
            }
            else
                _opTags.Add(tag);
        }

        public void AddScenario(string scenarioName)
        {
            if (_scenarioDict.ContainsKey(scenarioName))
            {
                //Do Nothing
                return;
            }
            else
            {
                _scenarioDict.Add(scenarioName, _currentLine);
                Debug.LogFormat("[Add Scenario]{0}:{1}", _currentLine, scenarioName);
                Status.CurrentScenario = scenarioName;
            }
        }

        public void AddScenario(AbstractTag tag)
        {
            string scenarioName = tag.Params["scenario"];

            if (_scenarioDict.ContainsKey(scenarioName))
            {
                Debug.LogFormat("Scenario: {0}Is Already Exist", scenarioName);               
                return;
            }
            else
            {
                _scenarioDict.Add(scenarioName, GetLastedLineNo());
                Debug.LogFormat("[Add Scenario]{0}:{1}", GetLastedLineNo(), scenarioName);
                Status.CurrentScenario = scenarioName;
            }
        }

        public void JumpToScenario(string scenarioName)
        {
            
            if (_scenarioDict.ContainsKey(scenarioName))
            {
                Debug.LogFormat("JumpTo line:{0}:{1}", _scenarioDict[scenarioName], scenarioName);
                int line = _scenarioDict[scenarioName];
                _currentLine = line;
                Status.EnableClickContinue = true;
                Status.EnableNextCommand = true;
            }
            else
            {
                Debug.LogFormat("Do not have scenario:{0}", scenarioName);
            }
        }

        public void InsertCommandBefore(AbstractTag tag)
        {
            _opTags.Insert(_currentLine, tag);
        }

        /*
         * @param string filePath:
         * 脚本文件在Resource下的路径
         */
        public void LoadScript(string filePath)
        {
            string path = Application.dataPath + Settings.SCENARIO_SCRIPT_PATH + filePath;
            if(!File.Exists(path))
            {
                Debug.LogFormat("cannot find script file: {0}!", path);
            }else
                Debug.LogFormat("load script file: {0}!", path);
            StreamReader sr = File.OpenText(path);
            string str = sr.ReadToEnd();
            sr.Close();
            _phraser.SetStringStream(str);
            _phraser.Phrase();
        }

        public void RunScript()
        {
            Debug.Log("Run Script!");
            StartCoroutine(OnRun());
        }
        public void NextCommand()
        {
            _currentLine++;

           // if (_currentLine < _opTags.Count)
           //     ExcuteCommand();
        }
        
        void ExcuteCommand()
        {
            Status.SkipThisTag = false;
            if (_currentLine < _opTags.Count)
            {
                _opTags[_currentLine].Before();
                if (!Status.SkipThisTag)
                {
                    Status.EnableNextCommand = true;
                    _opTags[_currentLine].Excute();
                    _opTags[_currentLine].After();
                }
            }
        }

        #endregion

        #region   Private Method

        void ResetEngine()
        {
            _currentLine = 0;
            
            _opLines.Clear();
            _scenarioDict.Clear();

            
        }

        IEnumerator OnRun()
        {
            while (_currentLine < _opTags.Count)
            {
                if(Status.EnableNextCommand)
                    ExcuteCommand();
                yield return new WaitForEndOfFrame();
            }
        }

       
        bool DispatchMessage(Message e)
        {
            return MessageDispatcher.Instance.DispatchMessage(e);
        }
        int GetLastedLineNo()
        {
            return _opTags.Count;
        }

        //TODO: 实现一个消息通知系统
        void OnClickContinue(Message pMessage)
        {
            /*
            if (_hideTextBox && _hasTextBoxToHide)
            {
                _hideTextBox = true;
                _hasTextBoxToHide = false;
                Message e = new Message("Message_HIDE_TEXTBOX");
                e.UserData = "TextBox_Name";
                DispatchMessage(e);
            }
            _hasTextBoxToHide = false;
            NextLine();*/
            if (Status.EnableClickContinue)
            {
                _currentLine++;
                Status.EnableNextCommand = true;
            }
            //NextCommand();
        }

     
        /*
        void OnCommandFinish(Message pMessage)
        {
            ++_currentLine;
            if (_waitTouch)
            {
                Debug.Log("wait touch! Click to continue!");
                _waitTouch = false;
            }
            else
            {
                NextLine();
            }
        }
        void OnWaitTouch()
        {
            _waitTouch = true;
        }
        */
        #endregion


    }
}
