﻿using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	Vector2 playerPosition;
	public Texture2D cursorTexture;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width/2, cursorTexture.height/2), CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.FindWithTag ("Player")) {
			GameObject player = GameObject.FindWithTag ("Player");
			playerPosition = new Vector2 (player.transform.position.x, player.transform.position.y);
		}
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 camPosision = new Vector3 ((playerPosition.x * 4 + mousePosition.x) / 5, (playerPosition.y * 4 + mousePosition.y) / 5, transform.position.z);
		transform.position = Vector3.MoveTowards(transform.position, camPosision, Mathf.Pow(Mathf.Pow(camPosision.x - transform.position.x, 2) + Mathf.Pow(camPosision.y - transform.position.y, 2), 0.75f) * Time.deltaTime * 5);
	}
}
