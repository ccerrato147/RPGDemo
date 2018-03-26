using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationUI : MonoBehaviour {

	// The reference to the entire conversation UI
	public GameObject conversationUI;

	// Create Conversation Service instance
	public ConversationService conversationService;

	// Use this for initialization
	void Start () {
		//conversationService = new ConversationService ();
	}
	
	// Update is called once per frame
	void Update () {
		// Check if the button to show or hide the UI has been pressed
		if (Input.GetButtonDown ("Conversation")) {
			bool flag = !conversationUI.activeSelf;
			conversationUI.SetActive (flag);
			if (flag) {
				conversationService.StartListening ();
			} else {
				conversationService.StopListening ();
			}
		}
	}
}
