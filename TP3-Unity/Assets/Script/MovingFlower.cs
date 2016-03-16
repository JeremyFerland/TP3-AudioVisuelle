using UnityEngine;
using System.Collections;

public class MovingFlower : MonoBehaviour {
	Vector3 targetPosition;
	Vector3 targetScale;
	bool bGoAway = false;
	Vector3 velocity = Vector3.zero;


	float timeLastUpdate;

	// Use this for initialization
	void Start () {
		timeLastUpdate = Time.time;
		targetPosition = new Vector3 (Random.Range (-6f, 6f), Random.Range (-10f, 10f), 0);
		float tempScal = Random.Range (1f, 1.5f);
		targetScale = new Vector3 (tempScal, tempScal, 1); 
	}

	public void goAway(){
		bGoAway = true;
		targetScale = new Vector3 (0, 0, 0);
		targetPosition = new Vector3 (-6, -10, 0);
		Invoke ("destroy", 0.05f);
	}

	public void ChangeState(){
		bGoAway = true;
		targetScale = new Vector3 (0, 0, 0);
		targetPosition = new Vector3 (-6, -10, 0);
		Invoke ("destroy", 1.0f);
	}

	void destroy(){
		ImagesMgr.instance.allImages.Remove(this.gameObject);
		ImagesMgr.instance.nbImagesOnScreen--;
		GameObject.Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {

		if (bGoAway) {
			transform.position = Vector3.SmoothDamp (transform.position, targetPosition, ref velocity, 5f);
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, 0.1f);
			return;
		}

		if (Time.time - timeLastUpdate >= 5f) {
			targetPosition = new Vector3 (Random.Range (-6f, 6f), Random.Range (-10f, 10f), 0);
			float tempScal = Random.Range (1f, 1.5f);
			targetScale = new Vector3 (tempScal, tempScal, 1);
			timeLastUpdate = Time.time;
		}
		if (Mathf.Abs(transform.position.x )> 8 || Mathf.Abs(transform.position.y)> 13 ) {
			destroy ();
		}

		transform.position = Vector3.SmoothDamp (transform.position, targetPosition, ref velocity, 7f);
		transform.localScale = Vector3.Lerp (transform.localScale, targetScale, 0.01f);
	}

	void OnCollisionEnter(Collision collision) {
		float tempX = 0;
		float tempY = 0;
		if (collision.transform.position.x < transform.position.x) {
			tempX = 6f;
		} else {
			tempX = -6f;
		}
		if (collision.transform.position.y < transform.position.y) {
			tempY = 10;
		} else {
			tempY = -10;
		}
		targetPosition = new Vector3 (tempX, tempY, 0);
	}
}
