using UnityEngine;
using System.Collections;

public class HomeBase {

	public float Radius { get; private set; }

	public int MaxHitPoints { get; private set; }
	public int CurrentHitPoints { get; private set; }

	public HomeBase() {

		this.Radius = 2.0f;

		this.MaxHitPoints = 50;
		this.CurrentHitPoints = this.MaxHitPoints;

	}

}
