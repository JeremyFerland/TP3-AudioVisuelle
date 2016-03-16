using UnityEngine;
using System.Collections;

public class OSCReceive : MonoBehaviour {

	private static OSCReceive _instance;

	public static OSCReceive instance{

		get{
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<OSCReceive>();
			return _instance;
		}
	}

	public OSC oscReference;

	void Awake() 
	{
		if(_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != _instance)
				Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	void Update () {
	}
	public void send(string message){
		OscMessage tempMessage = new OscMessage ();
		tempMessage = OSC.StringToOscMessage (message);
		oscReference.Send (tempMessage);
	}
}
