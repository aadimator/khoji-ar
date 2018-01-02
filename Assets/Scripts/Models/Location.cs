using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location {

	public long time;
	public float speed;
	public double altitude;
	public double longitude;
	public double latitude;

	public Location(long time, float speed, double altitude, double longitude, double latitude) {
		this.time = time;
		this.speed = speed;
		this.altitude = altitude;
		this.longitude = longitude;
		this.latitude = latitude;
	}

}
