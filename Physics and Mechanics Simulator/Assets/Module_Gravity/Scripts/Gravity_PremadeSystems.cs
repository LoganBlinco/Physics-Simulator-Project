using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Gravity_PremadeSystems : MonoBehaviour {

    public Gravity_PremadeSystems()
    {

        DestroyObjectsWithTag("Particle");
        DestroyObjectsWithTag("Marker");
        GravityPlanets.PlanetInstances.Clear();

        GravitySimulationController.isSimulating = false;
        GravitySimulationController.SimulationSpeed = 1;
        Gravity_InputController.Instance.gameObject.GetComponent<GravitySimulationController>().simulationTime = 0;

        Gravity_InputController.Instance.DropBoxGraphTarget.options.Clear();
        Gravity_InputController.Instance.CameraTarget.options.Clear();
        Gravity_InputController.Instance.DropBoxPlanet.options.Clear();
    }

    #region Earth Moon System
    public void EarthMoonSystem()
    {
        //Creating earth
        CreateEarth();
        //Creating moon
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
            3.5f,
            0);
        moon.MyGameObject.name = "Moon";
        moon.MyGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Planet_Sprites\\Ice-Planet", typeof(Sprite)) as Sprite;
        Vector3 deltaPosition = moon.MyGameObject.transform.position - earth.MyGameObject.transform.position;
        moon.initialVelocity = new Vector3(
            Mathf.Sqrt(GravitySimulationController.G * earth.mass / MyMaths.Vector_Magnitude(deltaPosition)),
            0,
            0);
        GravityPlanets.PlanetInstances.Add(moon);
    }

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
        earth.MyGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Planet_Sprites\\earth-like", typeof(Sprite)) as Sprite;
        GravityPlanets.PlanetInstances.Add(earth);
    }
    #endregion


    private void AddToDropBoxs(int numberOfParticles)
    {
        Gravity_InputController.Instance.CameraTarget.options.Add(new Dropdown.OptionData() { text = "Free Roam" });
        string _text = "";
        for (int i =1;i<=numberOfParticles;i++)
        {
            _text = "Particle " + i.ToString();
            Gravity_InputController.Instance.DropBoxGraphTarget.options.Add(new Dropdown.OptionData() { text = _text });
            Gravity_InputController.Instance.CameraTarget.options.Add(new Dropdown.OptionData() { text = _text });
            Gravity_InputController.Instance.DropBoxPlanet.options.Add(new Dropdown.OptionData() { text = _text });
        }
        Gravity_InputController.Instance.DropBoxPlanet.options.Add(new Dropdown.OptionData() { text = "Add Particle" });

        Gravity_InputController.Instance.ParticleIndexSelected = Gravity_InputController.Instance.DropBoxPlanet.value;
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
