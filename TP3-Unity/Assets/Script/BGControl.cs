using UnityEngine;
using System.Collections;

public class BGControl : MonoBehaviour {

	public SpriteRenderer[] BG;
	Color targetColorBG1 = new Color(1,1,1,0);
	Color targetColorBG2 = new Color(1,1,1,1);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		BG [0].color = Color.Lerp (BG [0].color, targetColorBG1, 0.01f); 
		BG [1].color = Color.Lerp (BG [1].color, targetColorBG2, 0.01f); 
		BG [2].color = Color.Lerp (BG [1].color, targetColorBG2, 0.01f);
		BG [3].color = Color.Lerp (BG [1].color, targetColorBG2, 0.01f);
		BG [4].color = Color.Lerp (BG [1].color, targetColorBG2, 0.01f);
		BG [5].color = Color.Lerp (BG [0].color, targetColorBG1, 0.01f);
		BG [6].color = Color.Lerp (BG [0].color, targetColorBG1, 0.01f);
	}

	public void changeBG(){
		Color tempColor = targetColorBG1;
		targetColorBG1 = targetColorBG2;
		targetColorBG2 = tempColor;
	}
}
