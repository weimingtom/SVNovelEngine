using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Sov.AVGPart
{
    /*
     * 一个场景类
     * 一个游戏中可以有多个场景但是运行中的只有一个
     * 压栈
     */

    public class Scene
    {
        public class SceneStatus
        {
            public bool SkipThisTag
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

            public SceneStatus()
            {
                Reset();
            }

            internal void Reset()
            {
                //Status reset
                EnableClickContinue = true;
                EnableNextCommand = true;
                SkipThisTag = false;
            }

        }

        public SceneStatus Status;

        public List<AbstractTag> Tags
        {
            get;
            set;
        }

        public int CurrentLine
        {
            get;
            set;
        }

        public string CurrentScenario
        {
            get;
            private set;
        }

        public string ScriptFilePath
        {
            get;
            set;
        }

        public string ScriptContent;

        public Dictionary<string, int> ScenarioDict
        {
            get;
            private set;
        }

        ScriptEngine _engine;

        public Scene(string scriptPath):
            this() //init
        {
            ScriptFilePath = scriptPath;
        }

        //some init
        public Scene()
        {
            Tags = new List<AbstractTag>();
            Status = new SceneStatus();
            ScriptFilePath = "";
            ScriptContent = "";
            CurrentLine = 0;

            ScenarioDict = new Dictionary<string, int>();
            _engine = ScriptEngine.Instance;
        }

        
        public void LoadScript()
        {
            string path = Application.dataPath + Settings.SCENARIO_SCRIPT_PATH + ScriptFilePath;
            if(!File.Exists(path))
            {
                Debug.LogFormat("cannot find script file: {0}!", path);
            }else
                Debug.LogFormat("load script file: {0}!", path);
            StreamReader sr = File.OpenText(path);
            ScriptContent = sr.ReadToEnd();
            sr.Close();

            //_phraser.SetScript(str);
            _engine.Phrase(this);
        }

        public void AddCommand(AbstractTag tag)
        {
            //tag.LineNo = _opTags.Count;
            tag.Engine = _engine;
            if (tag.Name == "scenario")
            {
                AddScenario(tag);
            }
            else
                Tags.Add(tag);
        }


        private void AddScenario(AbstractTag tag)
        {
            string scenarioName = tag.Params["scenario"];

            if (ScenarioDict.ContainsKey(scenarioName))
            {
                Debug.LogFormat("Scenario: {0}Is Already Exist", scenarioName);
                return;
            }
            else
            {
                ScenarioDict.Add(scenarioName, GetLastedTagLineNo());
                Debug.LogFormat("[Add Scenario]{0}:{1}", GetLastedTagLineNo(), scenarioName);
                CurrentScenario = scenarioName;
            }
        }

        int GetLastedTagLineNo()
        {
            return Tags.Count;
        }

        
    }
}
