using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class CollisionsParticle : MonoBehaviour {
public class CollisionsParticle : MonoBehaviour {

    #region Header variables
    //Stores every instance of CollisionsParticle which has been created allowing for access from any location.
    public static List<CollisionsParticle> ParticleInstances = new List<CollisionsParticle>();
    //Stores reference to the prefab object which will be used for the simulation 
	public GameObject PrefabSphere;

    #endregion

    #region Variables for graph

    public List<Vector2> momentumGraphPointsX = new List<Vector2>();
    public List<Vector2> momentumGraphPointsY = new List<Vector2>();

    #endregion

    #region Particle properties
    //Reference to the GameObject in the scene which IS the particle 
    public GameObject MyGameObject;

    //variable-property link because when initial velocity is assigned the current veloicty will always be equal to the initial
	private Vector3 _initialVelocity = new Vector3();
	public Vector3 initialVelocity
	{
		get {return _initialVelocity;}
		set
        {
			_initialVelocity = value;
			currentVelocity = value;
		}
	}

    //Stores the current velocity of the particle (x,y,z)
	public Vector3 currentVelocity = new Vector3();

    //Mass cannot be 0 or negative therefore validation must occur
    //Validation is done by a variable-property link
	private float _mass = 1;
	public float mass
	{
		get {return _mass;}
		set
        {
            if (value == 0)
            {
                //A mass of 1 is the default 
                _mass = 1;
            }
            else
            {
                _mass = MyMaths.Magnitude(value);
            }
        }
	}
    //Restitution value must be between 0 and 1 therefore a clamp validation must occur
	private float _restitution = 1;
	public float restitution
	{
		get { return _restitution;}
		set { _restitution = MyMaths.Clamp (value, 0, 1);}
	}

    //Radius cannot be negative or 0 and when assigned the  particles gameobject must be scaled accordingly
	private float _radius = 1;
	public float radius
	{
		get {return _radius;}
		set
        {
            if (value == 0)
            {
                //Default value
                _radius = 1;
            }
            else
            {
                _radius = MyMaths.Magnitude(value);
            }
            //Sets the radius of the sphere gameobject representing the particle (sets all dimentions)
			MyGameObject.transform.localScale = Vector3.one * _radius;
        }
	}
    #endregion

    #region Constructors and creation methods
    //Constructor which must create the prefab reference and instatiate the particles gameobject
    public CollisionsParticle()
	{
        //Resources.Load loads a prefab from the resources folder which is cast as a type: GameObject
		PrefabSphere = Resources.Load ("CollisionsSphere") as GameObject;
        //Instatiates the prefab
		CreateObject ();
	}
    //Controls the instatiation process of creating the particle
    private void CreateObject()
	{
        //Instatiates prefab into scene
		GameObject particleObject = Instantiate (PrefabSphere) as GameObject;
        //Scales the prefab
		particleObject.transform.localScale = Vector3.one * radius;
        //Centers prefab to middle of scene
		particleObject.transform.position = new Vector3 (0, 1, 0);
        //Assigns object to this instance
		MyGameObject = particleObject;
        //Assigns particles index value to the attached script 
        //This is used when performing collision calculations
		particleObject.GetComponent <CollisionCalculator> ().particleIndex = CollisionsParticle.ParticleInstances.Count;

	}
    #endregion

}
