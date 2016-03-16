using UnityEngine;
using System.Collections;

public class DepthImageViewer : MonoBehaviour 
{
	// the KinectManager instance
	private KinectManager manager;

	// the foreground texture
	private Texture2D foregroundTex;
	
	// rectangle taken by the foreground texture (in pixels)
	private Rect foregroundRect;
	private Vector2 foregroundOfs;

	// game objects to contain the joint colliders
	private GameObject[] jointColliders = null;
	

	void Start () 
	{
		// calculate the foreground rectangle
		Rect cameraRect = Camera.main.pixelRect;
		float rectHeight = cameraRect.height;
		float rectWidth = cameraRect.width;
		
		if(rectWidth > rectHeight)
			rectWidth = rectHeight * KinectWrapper.Constants.DepthImageWidth / KinectWrapper.Constants.DepthImageHeight;
		else
			rectHeight = rectWidth * KinectWrapper.Constants.DepthImageHeight / KinectWrapper.Constants.DepthImageWidth;

		foregroundOfs = new Vector2((cameraRect.width - rectWidth) / 2, (cameraRect.height - rectHeight) / 2);
		foregroundRect = new Rect(foregroundOfs.x, cameraRect.height - foregroundOfs.y, rectWidth, -rectHeight);
	
		// create joint colliders
		int numColliders = (int)KinectWrapper.NuiSkeletonPositionIndex.Count;
		jointColliders = new GameObject[numColliders];
		
		for(int i = 0; i < numColliders; i++)
		{
			string sColObjectName = ((KinectWrapper.NuiSkeletonPositionIndex)i).ToString() + "Collider";
			jointColliders[i] = new GameObject(sColObjectName);
			jointColliders[i].transform.parent = transform;
			
			SphereCollider collider = jointColliders[i].AddComponent<SphereCollider>();
			collider.radius = 1.8f;
		}
	}
	float lastX;
	float lastX2;
	float lastY;
	float lastY2;
	public float lastMove = 0;

	void Update () 
	{
		if(manager == null)
		{
			manager = KinectManager.Instance;
		}

		// get the users texture
		if(manager && manager.IsInitialized())
		{
			foregroundTex = manager.GetUsersLblTex();
		}

		if (manager.IsUserDetected ()) {
			uint userId = manager.GetPlayer1ID ();

			// update colliders
			int numColliders = (int)KinectWrapper.NuiSkeletonPositionIndex.Count;
			
			for (int i = 0; i < numColliders; i++) {
				if (manager.IsJointTracked (userId, i)) {
					Vector3 posJoint = manager.GetRawSkeletonJointPos (userId, i);

					if (posJoint != Vector3.zero) {

						// convert the joint 3d position to depth 2d coordinates
						Vector2 posDepth = manager.GetDepthMapPosForJointPos (posJoint);
						
						float scaledX = posDepth.x * foregroundRect.width / KinectWrapper.Constants.DepthImageWidth;
						float scaledY = posDepth.y * -foregroundRect.height / KinectWrapper.Constants.DepthImageHeight;

						float screenX = foregroundOfs.x + scaledX;
						float screenY = Camera.main.pixelHeight - (foregroundOfs.y + scaledY);
						float zDistance = posJoint.z - Camera.main.transform.position.z;
						
						Vector3 posScreen = new Vector3 (screenX + 50, ((screenY - 80) * 1.8f), zDistance);
						Vector3 posCollider = Camera.main.ScreenToWorldPoint (posScreen);

						jointColliders [i].transform.position = posCollider;
						if(i==7){
							if(jointColliders [i].transform.position.x - lastX > 0.2f){
								lastX = jointColliders [i].transform.position.x;
								lastMove= Time.time;
							}else{
								lastX = jointColliders [i].transform.position.x;
							}
							if(jointColliders [i].transform.position.y - lastY > 0.2f){
								lastY = jointColliders [i].transform.position.y;
								lastMove= Time.time;
							}else{
								lastY = jointColliders [i].transform.position.y;
							}
								
						}if(i==11){
							if(jointColliders [i].transform.position.x - lastX2 > 0.2f){
								lastX2 = jointColliders [i].transform.position.x;
								lastMove= Time.time;
							}else{
								lastX2 = jointColliders [i].transform.position.x;
							}
							if(jointColliders [i].transform.position.y - lastY2 > 0.2f){
								lastY2 = jointColliders [i].transform.position.y;
								lastMove= Time.time;
							}else{
								lastY2 = jointColliders [i].transform.position.y;
							}
							
						}
					}
				}
			}
		} else {
			foreach (GameObject x in jointColliders){
				x.transform.position = new Vector3 (-1000,0,0);
			}		
		
		}

	}

	void OnGUI()
	{
		if(foregroundTex)
		{
			GUI.DrawTexture(foregroundRect, foregroundTex);
		}
	}

}
