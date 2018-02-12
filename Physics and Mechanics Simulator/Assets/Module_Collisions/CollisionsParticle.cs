using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class CollisionsParticle : MonoBehaviour {
public class CollisionsParticle : MonoBehaviour {

	public static List<CollisionsParticle> ParticleInstances = new List<CollisionsParticle>();
	public GameObject PrefabSphere;
	#region Variables

	public GameObject MyGameObject;


	private Vector3 _initialVelocity = new Vector2();
	public Vector3 initialVelocity
	{
		get {return _initialVelocity;}
		set {
			_initialVelocity = value;
			currentVelocity = value;
		}
	}
	public Vector3 currentVelocity = new Vector2();

	private float _mass = 1;
	public float mass
	{
		get {return _mass;}
		set {_mass = MyMaths.Magnitude (value);}
	}

	private float _restitution = 1;
	public float restitution
	{
		get { return _restitution;}
		set { _restitution = MyMaths.Clamp (value, 0, 1);}
	}

	private float _radius = 1;
	public float radius
	{
		get {return _radius;}
		set {_radius = MyMaths.Magnitude (value);
			MyGameObject.transform.localScale = Vector3.one * _radius;}
	}
	#endregion

	public CollisionsParticle()
	{
		PrefabSphere = Resources.Load ("CollisionsSphere") as GameObject;
		CreateObject ();
	}

	private void CreateObject()
	{
		GameObject particleObject = Instantiate (PrefabSphere) as GameObject;
		particleObject.transform.localScale = Vector3.one * this.radius;
		particleObject.transform.position = new Vector3 (0, 1, 0);
		this.MyGameObject = particleObject;

		particleObject.GetComponent <CollisionCalculator> ().particleIndex = CollisionsParticle.ParticleInstances.Count;

	}



}
