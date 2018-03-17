﻿namespace Mapbox.Unity.Ar
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
    using Firebase;
    using Firebase.Unity.Editor;
    using Firebase.Database;

    public class ARMessageProvider : MonoBehaviour {
		/// <summary>
		/// This loads messages according to GPS coordinates, removes messages, and repositions messages
		/// within the scene. 
		/// </summary>
		private static ARMessageProvider _instance;
		public static ARMessageProvider Instance { get { return _instance; } }

		[SerializeField]
		private AbstractMap _map;

		[HideInInspector]
		public List<GameObject> currentMessages = new List<GameObject>();
		[HideInInspector]
		public bool deviceAuthenticated = true;
		private bool gotInitialAlignment = false;

		public Mapbox.Unity.Location.DeviceLocationProvider deviceLocation;

		void Awake(){
			_instance = this;

            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://khoji-aadimator.firebaseio.com/");
            Unity.Utilities.Console.Instance.Log("Firebase connection", "lightblue");

            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            Unity.Utilities.Console.Instance.Log("Firebase reference", "lightblue");


        }

		public void GotAlignment(){
			if (deviceAuthenticated){
				if (!gotInitialAlignment) {
					gotInitialAlignment = true;
					//set UI active once we are authenticated
					UIBehavior.Instance.ShowUI ();
                    //load first messages
                    Unity.Utilities.Console.Instance.Log("Loading UI and initial messages!", "lightblue");
                    MessageService.Instance.LoadAllMessages ();
                    Unity.Utilities.Console.Instance.Log("Loaded UI and initial messages!", "lightblue");

                } else {
                    UpdateARMessageLocations (deviceLocation.CurrentLocation.LatitudeLongitude);
					Unity.Utilities.Console.Instance.Log("Repositioning messages!", "lightblue");

					FirebaseDatabase.DefaultInstance
						.GetReference("users")
						.GetValueAsync().ContinueWith(task => {
							if (task.IsFaulted)
							{
								// Handle the error...
								Unity.Utilities.Console.Instance.Log("Firebase users retrievel error", "red");
							}
							else if (task.IsCompleted)
							{
								DataSnapshot snapshot = task.Result;
								// Do something with snapshot...
								Unity.Utilities.Console.Instance.Log("user retrievel success", "lightblue");
							}
						});
					
				}
			} else {
				Debug.Log ("Got Alignment---DEVICE NOT AUTHENTICATED!");
			}
		}

		public void RemoveCurrentMessages(){
			foreach (GameObject messageObject in currentMessages) {
				Destroy (messageObject);
			}
			currentMessages.Clear ();
		}

		public void LoadARMessages(List<GameObject> messageObjectList){
			StartCoroutine (LoadARMessagesRoutine (messageObjectList));
		}

		//this placed initial messages after they are loaded in from gamesparks
		IEnumerator LoadARMessagesRoutine(List<GameObject> messageObjectList){

			RemoveCurrentMessages ();

			yield return new WaitForSeconds(2f);

			foreach (GameObject messageObject in messageObjectList) {

				Message thisMessage = messageObject.GetComponent<Message> ();

				Vector3 _targetPosition =_map.Root.TransformPoint(Conversions.GeoToWorldPosition(thisMessage.latitude,thisMessage.longitude, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz()); 

				Debug.Log ("~~~~TARGET POSITION: " + _targetPosition);
                Unity.Utilities.Console.Instance.Log("TARGET POSITION: " + _targetPosition, "lightblue");

                messageObject.transform.position = _targetPosition;
				messageObject.GetComponent<Message> ().SetText (thisMessage.text);
				//add to list so we can update positions later
				currentMessages.Add(messageObject);
			}
		}
		//this repositions messages everytime our location is updated
		public void UpdateARMessageLocations(Vector2d currentLocation){

			if (currentMessages.Count > 0) {

				Debug.Log ("Repositioning Messages...");

				foreach (GameObject messageObject in currentMessages) {

					Message message = messageObject.GetComponent<Message> ();

					Vector3 _targetPosition =_map.Root.TransformPoint(Conversions.GeoToWorldPosition(message.latitude,message.longitude, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz()); 

					messageObject.transform.position = _targetPosition;
				}
			}
		}
	}
}

