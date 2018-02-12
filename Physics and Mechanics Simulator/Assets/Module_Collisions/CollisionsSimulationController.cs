using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CollisionsSimulationController : MonoBehaviour {

	#region UI References

	public Text Label_Time;

	public static float SimulationSpeed = 1;
	public static float simulationTime = 0;

	public static bool isSimulating = false;

	private float deltaT;

	#endregion


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isSimulating == true)
		{
			deltaT = Time.deltaTime * SimulationSpeed;
			UpdateTimeLabel ();
			MoveParticles ();


			simulationTime += deltaT;
		}


	}

	private void UpdateTimeLabel()
	{
		string value2DP = simulationTime.ToString("n2");
		Label_Time.text = "Time : " + value2DP + " s";
	}

	private void MoveParticles()
	{
		for (int i =0;i<CollisionsParticle.ParticleInstances.Count;i++)
		{
			CollisionsParticle.ParticleInstances [i].MyGameObject.transform.position += CollisionsParticle.ParticleInstances [i].currentVelocity * deltaT;
		}
	}
}
