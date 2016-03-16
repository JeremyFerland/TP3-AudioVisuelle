using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImagesMgr : MonoBehaviour {

	public int nbImages = 100;
	public GameObject imageTemplate;
	public Material[] material;
	public Material[] luciole;	
	public float timeBeforeCreate = 0.2f;
	float lastCreate ;
	float timeLastChangeStage;
	public int nbImagesOnScreen = 0 ;
	public DepthImageViewer kinectCollider;
	//GameObject[] allImages;

	public bool isLuciole = true;

	public List<GameObject> allImages = new List<GameObject> ();

	public BGControl bg;

	private static ImagesMgr _instance;
	public static ImagesMgr instance{
		
		get{
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<ImagesMgr>();
			return _instance;
		}
	}
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
		lastCreate = Time.time;


	}

	
	// Update is called once per frame
	void Update () {
		if (allImages.Count <= 50 && Time.time - timeLastChangeStage > 10.0f) {
			isLuciole = !isLuciole;
			bg.changeBG();
			OSCReceive.instance.send("/changeState " + (isLuciole ? 1:0));
			foreach (GameObject image in allImages){
				MovingFlower flower = image.GetComponent<MovingFlower>();
				flower.ChangeState();
			}
			timeLastChangeStage = Time.time;
		}
		float timelastMove = Time.time - kinectCollider.lastMove;
		nbImages = 700-(int)(timelastMove * 400);
		nbImages = Mathf.Min (nbImages, 700);
		nbImages = Mathf.Max (nbImages, 50);

		if (Time.time - lastCreate > timeBeforeCreate && nbImagesOnScreen<nbImages) {
			GameObject go = GameObject.Instantiate (imageTemplate) as GameObject;
			go.transform.parent = transform;
			if(isLuciole){
				go.GetComponent<Renderer>().material = luciole[Random.Range(0,luciole.Length)];
			}else{
				go.GetComponent<Renderer>().material = material[Random.Range(0,material.Length)];
			}
			go.transform.localScale = new Vector3(0,0,0);
			float tempx = ((Random.Range(0,2)*2) - 1) * 6;
			float tempy = ((Random.Range(0,2)*2) - 1) * 10;
			go.transform.position = new Vector3 (tempx, tempy, 0);
			allImages.Add (go);
			lastCreate=Time.time;
			nbImagesOnScreen++;
		}
		if (Time.time - lastCreate > 0.05f && nbImagesOnScreen >= nbImages && nbImagesOnScreen >= 50) {
			MovingFlower flower = allImages[nbImagesOnScreen-1].GetComponent<MovingFlower>();
			flower.goAway();
			lastCreate=Time.time;
		}

	}
}
