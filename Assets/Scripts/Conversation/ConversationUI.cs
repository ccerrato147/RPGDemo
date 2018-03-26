using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationUI : MonoBehaviour {

	// The reference to the entire conversation UI
	public GameObject conversationUI;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Check if the button to show or hide the UI has been pressed
		if (Input.GetButtonDown ("Conversation")) {
			conversationUI.SetActive (!conversationUI.activeSelf);
		}
	}
}
