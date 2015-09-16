using UnityEngine;
using System.Collections;
using Sov.AVGPart;

public class SceneCtrl : MonoBehaviour 
{
    public string ScriptFileName;
    Scene s = null;
	// Use this for initialization
	void Start () {
        
       // ScriptEngine.Instance.LoadScript(ScriptFileName);
        s = new Scene(ScriptFileName);
        s.LoadScript();
      //  ScriptEngine.Instance.Run(s);
      //  ScriptEngine.Instance.RunScript();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGameStart()
    {
        Debug.Log("GameStart!");
        ScriptEngine.Instance.Run(s);
        //gameObject.SetActive(false);
    }
}
