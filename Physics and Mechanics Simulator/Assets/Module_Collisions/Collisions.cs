using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour {

	public GameObject PrefabSphere;




	// Use this for initialization
	void Start () {
		PrefabSphere = Resources.Load ("CollisionsSphere") as GameObject;
		CreateFirstObject ();


	}


	private void CreateFirstObject()
	{
		CollisionsParticle newParticle = new CollisionsParticle ();
		newParticle.initialVelocity = Vector3.zero;
		newParticle.mass = 1.0f;
		newParticle.restitution = 1.0f;
		newParticle.radius = 1.0f;

		CollisionsParticle.ParticleInstances.Add (newParticle);
	}
}
