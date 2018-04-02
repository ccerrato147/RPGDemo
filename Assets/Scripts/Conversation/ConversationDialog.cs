using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.Conversation.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Logging;
using System.Collections;
using FullSerializer;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Connection;
using UnityEngine.UI;
using System.Timers;

public class ConversationDialog : MonoBehaviour
{
	// Configurable Text
	public Text Response;
	public SpeechService speech;

	// Private variables for internal use only
	private string _username = "6d576a6c-32e7-4fe4-940b-86a615e845ad";
	private string _password = "aycR4WiIaVu6";
	private string _url = "https://gateway.watsonplatform.net/conversation/api";
	private string _workspaceId = "96f0f492-e5f6-47d9-88ff-33038b6f282b";

	private Conversation _conversation;
	private string _conversationVersionDate = "2017-05-26";

	private fsSerializer _serializer = new fsSerializer();
	private Dictionary<string, object> _context = null;

	void Start()
	{
		LogSystem.InstallDefaultReactors();

		//  Create credential and instantiate service
		Credentials credentials = new Credentials(_username, _password, _url);

		_conversation = new Conversation(credentials);
		_conversation.VersionDate = _conversationVersionDate;
	}

	public void SendDialogMessage(string text)
	{
		MessageRequest messageRequest = new MessageRequest()
		{
			input = new Dictionary<string, object>()
			{
				{ "text", text }
			},
			context = _context
		};

		if (!_conversation.Message(OnMessage, OnFail, _workspaceId, messageRequest))
			Log.Debug("ExampleConversation.AskQuestion()", "Failed to message!");
		
	}

	private void OnMessage(object resp, Dictionary<string, object> customData)
	{
		Log.Debug("ExampleConversation.OnMessage()", "Conversation: Message Response: {0}", customData["json"].ToString());



		//  Convert resp to fsdata
		fsData fsdata = null;
		fsResult r = _serializer.TrySerialize(resp.GetType(), resp, out fsdata);
		if (!r.Succeeded)
			throw new WatsonException(r.FormattedMessages);

		//  Convert fsdata to MessageResponse
		MessageResponse messageResponse = new MessageResponse();
		object obj = messageResponse;
		r = _serializer.TryDeserialize(fsdata, obj.GetType(), ref obj);
		if (!r.Succeeded)
			throw new WatsonException(r.FormattedMessages);

		//  Set context for next round of messaging
		object _tempContext = null;
		(resp as Dictionary<string, object>).TryGetValue("context", out _tempContext);

		if (_tempContext != null)
			_context = _tempContext as Dictionary<string, object>;
		else
			Log.Debug("ExampleConversation.OnMessage()", "Failed to get context");

		if (resp != null && (messageResponse.intents.Length > 0 || messageResponse.intents.Length > 0)) {
			Response.text = string.Join(", ", messageResponse.output.text);
			speech.SpeakText (Response.text);
		}
	}

	private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
	{
		Log.Error("ExampleConversation.OnFail()", "Error received: {0}", error.ToString());
	}
}
