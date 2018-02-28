using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlanets : MonoBehaviour {

    #region Header variables
    //Stores instances of GravityPlanets which have been created
    public static List<GravityPlanets> PlanetInstances = new List<GravityPlanets>();
    //References to planet prefabs 
    public static List<GameObject> PlanetPrefabs = new List<GameObject>();
    //Stores sprite objects which are used when creating planets
    public static UnityEngine.Object[] PlanetSprites;
    #endregion

    #region Graph variables
    //Stores values which will be used for plotting graph
    // x = time
    // y = quantity
    public List<Vector2> graphPointsSpeed = new List<Vector2>();
    public List<Vector2> graphPointsAcceleration = new List<Vector2>();

    #endregion

    #region Particle properties
    //Reference to the GameObject in the scene which IS the particle 
    public GameObject MyGameObject;

    //variable-property link because when initial velocity is assigned the current veloicty will always be equal to the initial
    private Vector3 _initialVelocity = new Vector3();
    public Vector3 initialVelocity
    {
        get { return _initialVelocity; }
        set
        {
            _initialVelocity = value;
            currentVelocity = _initialVelocity;
        }
    }

    //Stores the current velocity of the planet (x,y,z)
    public Vector3 currentVelocity = new Vector3();

    //Mass cannot be 0 or negative therefore validation must occur
    //Validation is done by a variable-property link
    private float _mass = 1;
    public float mass
    {
        get { return _mass; }
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

    //Diameter cannot be negative or 0 and when assigned the planet gameobject must be scaled accordingly
    private float _diameter = 0.25f;
    public float diameter
    {
        get { return _diameter; }
        set
        {
            if (value == 0)
            {
                //Default value
                _diameter = 0.25f;
            }
            else
            {
                _diameter = MyMaths.Magnitude(value);
            }
            //Sets the Diameter of the sphere gameobject representing the particle (sets all dimentions)
            MyGameObject.transform.localScale = Vector3.one * _diameter;
        }
    }
    #endregion

    #region Constructors and creation methods
    //Constructor which must create the prefab reference and instatiate the planets gameobject
    public GravityPlanets()
    {
        //Creates referecnes to prefabs and sprites to be used as planets
        CreatePlanetReferences();
        //Instatiates the prefab
        CreateObject();
    }
    //Creates sprite and prefab references 
    private void CreatePlanetReferences()
    {
        PlanetPrefabs.Add(Resources.Load("GravityEarth") as GameObject);
		PlanetSprites = Resources.LoadAll ("Planet_Sprites", typeof(Sprite));
    }

    //Controls the instatiation process of creating the particle
    private void CreateObject()
    {

        GameObject planetObject = Instantiate(PlanetPrefabs[0]) as GameObject;
        planetObject.transform.position = new Vector3(0, 1, 0);
        MyGameObject = planetObject;
        //Gets random number
        System.Random generator = new System.Random();
        int index = generator.Next(0, PlanetSprites.Length - 1);
        //Random sprite is seected from the list
        MyGameObject.GetComponent<SpriteRenderer> ().sprite = PlanetSprites[index] as Sprite;
        //Default value
        diameter = 0.25f;

        //Assigns particles index value to the attached script 
        //This is used when performing collision calculations
        MyGameObject.GetComponent<GravityCalculator>().particleIndex = GravityPlanets.PlanetInstances.Count;
    }
    #endregion

}
