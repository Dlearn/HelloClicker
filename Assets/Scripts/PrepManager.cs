using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepManager : MonoBehaviour {
	void Start () {
		
	}
	
    public void Ready()
    {
        JSONObject data = new JSONObject(JSONObject.Type.BOOL);
        data.b = true;
        GameManager.socket.Emit("is ready", data);
    }
}
