using UnityEngine;
using System.Collections;
using Sov.AVGPart;
public class SceneCtrl : MonoBehaviour {


    public string ScriptFileName;
	// Use this for initialization
	void Start () {
        
        ScriptEngine.Instance.LoadScript(ScriptFileName);
      //  ScriptEngine.Instance.RunScript();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGameStart()
    {
        Debug.Log("GameStart!");
        ScriptEngine.Instance.RunScript();
        //gameObject.SetActive(false);
    }
}
