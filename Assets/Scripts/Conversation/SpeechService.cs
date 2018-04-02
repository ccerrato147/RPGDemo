using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Connection;

public class SpeechService : MonoBehaviour
{
	// Public configurable
	public ConversationService conversationWithUser;

	// Private variables

	private string _username = "987e4937-25a3-426d-8c00-f2f32c5f88c5";
	private string _password = "bT32xeXIG1uI";
	private string _url = "https://stream.watsonplatform.net/text-to-speech/api";

	TextToSpeech _textToSpeech;

	private bool flag = false;

	void Start()
	{
		LogSystem.InstallDefaultReactors();

		//  Create credential and instantiate service
		Credentials credentials = new Credentials(_username, _password, _url);

		_textToSpeech = new TextToSpeech(credentials);
	}

	public void SpeakText(string text)
	{
		//  Synthesize
		Log.Debug("ExampleTextToSpeech.Examples()", "Attempting synthesize.");
		_textToSpeech.Voice = VoiceType.en_US_Michael;
		_textToSpeech.ToSpeech(HandleToSpeechCallback, OnFail, text, true);
	}

	void HandleToSpeechCallback(AudioClip clip, Dictionary<string, object> customData = null)
	{
		Debug.Log ("ENTER SPEECH HANDLER");
		if (!flag) {
			PlayClip (clip);
		}
	}

	private void PlayClip(AudioClip clip)
	{
		if (Application.isPlaying && clip != null)
		{
			flag = true;
			conversationWithUser.SetSpeaking (true);
			GameObject audioObject = new GameObject("AudioObject");
			AudioSource source = audioObject.AddComponent<AudioSource>();
			source.spatialBlend = 0.0f;
			source.loop = false;
			source.clip = clip;
			source.Play();

			Destroy(audioObject, clip.length);
			flag = false;
			conversationWithUser.SetSpeaking (false);
		}
	}

	private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
	{
		Log.Error("ExampleTextToSpeech.OnFail()", "Error received: {0}", error.ToString());
	}
}
