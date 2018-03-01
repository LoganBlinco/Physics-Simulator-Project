using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Gravity_PremadeSystems : MonoBehaviour {

    //Constructor of class which removes the unnessary gameobjects and dropbox options in the scene
    public Gravity_PremadeSystems()
    {
        //Destroys previous created planets/particles
        DestroyObjectsWithTag("Particle");
        //Destroys markers in the scene
        DestroyObjectsWithTag("Marker");
        GravityPlanets.PlanetInstances.Clear();

        //Resetting Simulation Settings
        GravitySimulationController.isSimulating = false;
        GravitySimulationController.SimulationSpeed = 1;
        Gravity_InputController.Instance.gameObject.GetComponent<GravitySimulationController>().simulationTime = 0;

        //Removing dropbox options because they may contain more than 2 particles or less
        Gravity_InputController.Instance.DropBoxGraphTarget.options.Clear();
        Gravity_InputController.Instance.CameraTarget.options.Clear();
        Gravity_InputController.Instance.DropBoxPlanet.options.Clear();
    }

    #region Eclipse System
    //Eclipse system is the based upon the circular system but with difffrent veloicty
    public void EclipseSystem()
    {
        //Multiplier for "moon" velocity
        float veloictyMultiplier = 1.15f;
        //Create Earth-Moon Circular system
        EarthMoonSystem();
        //Changes the "moon" veloicty by multiplier
        GravityPlanets.PlanetInstances[1].currentVelocity *= veloictyMultiplier;
    }
    #endregion

    #region Earth Moon System
    //Creates an EarthMoonSystem
    //Assumes perfect circular orbit
    public void EarthMoonSystem()
    {
        CreateEarth();
        CreateMoon(GravityPlanets.PlanetInstances[0]);
        //Adds particle , free roam and add particle options to the dropboxs
        AddToDropBoxs(2);
    }
    private void CreateMoon(GravityPlanets earth)
    {
        GravityPlanets moon = new GravityPlanets();
        moon.diameter = 0.1f;
        moon.mass = 0.012f;
        moon.MyGameObject.transform.position = new Vector3(
            0,
            4f,
            0);
        moon.MyGameObject.name = "Moon";
        //Changes sprite to a moon-like sprite
        moon.MyGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Planet_Sprites\\Ice-Planet", typeof(Sprite)) as Sprite;
        Vector3 deltaPosition = moon.MyGameObject.transform.position - earth.MyGameObject.transform.position;
        //Velocity in X component required for perfect circular motion
        moon.initialVelocity = new Vector3(
            Mathf.Sqrt(GravitySimulationController.G * earth.mass / MyMaths.Vector_Magnitude(deltaPosition)),
            0,
            0);
        GravityPlanets.PlanetInstances.Add(moon);
    }
    //Creates the earth planet and gameobject
    private void CreateEarth()
    {
        GravityPlanets earth = new GravityPlanets();
        earth.MyGameObject.name = "Earth";

        earth.diameter = 0.25f;
        earth.mass = 1;
        earth.initialVelocity = Vector3.zero;
        earth.MyGameObject.transform.position = new Vector3(
            0,
            -2,
            0);
        //Changes sprite to an earth sprite
        earth.MyGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Planet_Sprites\\earth-like", typeof(Sprite)) as Sprite;
        GravityPlanets.PlanetInstances.Add(earth);
    }
    #endregion

    #region Hyperbolic encounter 

    public void HyperbolicEncounter()
    {
        GravityPlanets bigMass = new GravityPlanets();

        bigMass.diameter = 0.25f;
        bigMass.mass = 2.3f;
        bigMass.initialVelocity = Vector3.zero;
        bigMass.MyGameObject.transform.position = new Vector3(
            1,
            0,
            0);
        //Changes sprite to an earth sprite
        bigMass.MyGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Planet_Sprites\\earth-like", typeof(Sprite)) as Sprite;
        GravityPlanets.PlanetInstances.Add(bigMass);

        GravityPlanets smallMass = new GravityPlanets();
        smallMass.diameter = 0.1f;
        smallMass.mass = 0.01f;
        smallMass.MyGameObject.transform.position = new Vector3(
            -3,
            5f,
            0);
        smallMass.MyGameObject.name = "Moon";
        //Changes sprite to a moon-like sprite
        smallMass.MyGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Planet_Sprites\\Ice-Planet", typeof(Sprite)) as Sprite;
        //Velocity in X component required for perfect circular motion
        smallMass.initialVelocity = new Vector3(
            2.5f,
            -1f,
            0);
        GravityPlanets.PlanetInstances.Add(smallMass);

        AddToDropBoxs(2);
    }

    #endregion

    //Adds the options to the 3 dropboxs , camera , graph and planet selection
    //Adds the number of particles given
    private void AddToDropBoxs(int numberOfParticles)
    {
        //Camera selection must have free roam
        Gravity_InputController.Instance.CameraTarget.options.Add(new Dropdown.OptionData() { text = "Free Roam" });
        string _text = "";
        //Adds the particle : X options to dropboxes
        for (int i =1;i<=numberOfParticles;i++)
        {
            _text = "Particle " + i.ToString();
            Gravity_InputController.Instance.DropBoxGraphTarget.options.Add(new Dropdown.OptionData() { text = _text });
            Gravity_InputController.Instance.CameraTarget.options.Add(new Dropdown.OptionData() { text = _text });
            Gravity_InputController.Instance.DropBoxPlanet.options.Add(new Dropdown.OptionData() { text = _text });
        }
        //Particle selection must have an option to add particles
        Gravity_InputController.Instance.DropBoxPlanet.options.Add(new Dropdown.OptionData() { text = "Add Particle" });

        Gravity_InputController.Instance.ParticleIndexSelected = Gravity_InputController.Instance.DropBoxPlanet.value;
        //Updates values shown
        Gravity_InputController.Instance.OnDropBoxParticleChanged();
    }


    public static void DestroyObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}
