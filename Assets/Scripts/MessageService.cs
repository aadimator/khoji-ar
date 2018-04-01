namespace Mapbox.Unity.Ar
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	//using GameSparks.Core;

	public class MessageService : MonoBehaviour {

		/// <summary>
		/// This class handles loading, and writing new messages. New Message
		/// objects are instantiated here.
		/// </summary>
		private static MessageService _instance;
		public static MessageService Instance { get { return _instance; } } 

		public Transform mapRootTransform;

		public GameObject messagePrefabAR;

		void Awake(){
			_instance = this;
		}

		public void LoadAllMessages(){

			List<GameObject> messageObjectList = new List<GameObject> ();

            

            //GameObject MessageBubble = Instantiate(messagePrefabAR, mapRootTransform);
            //Message message = MessageBubble.GetComponent<Message>();

            //message.latitude = 33.9858943;
            //message.longitude = 72.9703613;
            //message.text = "aadimator";
            //messageObjectList.Add(MessageBubble);

            //Unity.Utilities.Console.Instance.Log("Added aadimator to list", "lightblue");

            //new GameSparks.Api.Requests.LogEventRequest().SetEventKey("LOAD_MESSAGE").Send((response) => {
            //	if (!response.HasErrors) {
            //		Debug.Log("Received Player Data From GameSparks...");
            //		List<GSData> locations = response.ScriptData.GetGSDataList ("all_Messages");
            //		for (var e = locations.GetEnumerator (); e.MoveNext ();) {

            //			GameObject MessageBubble = Instantiate (messagePrefabAR,mapRootTransform);
            //			Message message = MessageBubble.GetComponent<Message>();

            //			message.latitude = double.Parse(e.Current.GetString ("messLat"));
            //			message.longitude = double.Parse(e.Current.GetString ("messLon"));
            //			message.text = e.Current.GetString ("messText");
            //			messageObjectList.Add(MessageBubble);
            //		}
            //	} else {
            //		Debug.Log("Error Loading Message Data...");
            //	}
            //});
            //pass list of objects to ARmessage provider so they can be placed
            ARMessageProvider.Instance.LoadARMessages (messageObjectList);
		}
	}
}
