using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlanets : MonoBehaviour {

    #region Header variables
    public static List<GravityPlanets> PlanetInstances = new List<GravityPlanets>();
    public static List<GameObject> PlanetPrefabs = new List<GameObject>();
    public static UnityEngine.Object[] spriteList;
    #endregion

    #region Multipliers 

    float diameterMod = 0.25f;

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
    private float _diameter = 1;
    public float diameter
    {
        get { return _diameter; }
        set
        {
            if (value == 0)
            {
                //Default value
                _diameter = 1 * diameterMod;
            }
            else
            {
                _diameter = MyMaths.Magnitude(value * diameterMod);
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
        CreatePlanetReferences();
        //Instatiates the prefab
        CreateObject();
    }

    private void CreatePlanetReferences()
    {
        PlanetPrefabs.Add(Resources.Load("GravityEarth") as GameObject);
        spriteList = Resources.LoadAll("Sprites",typeof(Sprite));
    }

    //Controls the instatiation process of creating the particle
    private void CreateObject()
    {
        //Instatiates random prefab from options
        GameObject planetObject = Instantiate(PlanetPrefabs[0]) as GameObject;
        //Scales the prefab
        planetObject.transform.localScale = Vector3.one * diameter * diameterMod;
        //Centers prefab to middle of scene
        planetObject.transform.position = new Vector3(0, 1, 0);
        //Assigns object to this instance
        MyGameObject = planetObject;

        System.Random generator = new System.Random();
        int index = generator.Next(0, spriteList.Length - 1);
        MyGameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)spriteList[index];
        MyGameObject.GetComponent<GravityCollisions>().particleIndex = GravityPlanets.PlanetInstances.Count;
    }
    #endregion

}
