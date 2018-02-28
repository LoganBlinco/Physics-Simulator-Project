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


    public void EarthMoonSystem()
    {

        //Creating earth
        GravityPlanets earth = new GravityPlanets();
        earth.MyGameObject.name = "Earth";
        GravityPlanets.PlanetInstances.Add(earth);

        earth.diameter = 0.25f;
        earth.mass = 1;
        earth.initialVelocity = Vector3.zero;
        earth.MyGameObject.transform.position = new Vector3(
            0,
            -2,
            0);
        GravityPlanets.PlanetInstances.Add(earth);

        //Creating moon
        GravityPlanets moon = new GravityPlanets();
        moon.diameter = 0.1f;
        moon.mass = 0.012f;
        moon.MyGameObject.transform.position = new Vector3(
            0,
            3.5f,
            0);
        moon.MyGameObject.name = "Moon";
        Vector3 deltaPosition = moon.MyGameObject.transform.position - earth.MyGameObject.transform.position;
        moon.initialVelocity = new Vector3(
            Mathf.Sqrt(GravitySimulationController.G * earth.mass / MyMaths.Vector_Magnitude(deltaPosition)),
            0,
            0);
        Debug.Log(moon.currentVelocity);
        GravityPlanets.PlanetInstances.Add(moon);
        Debug.Log(GravitySimulationController.G);
        Debug.Log(MyMaths.Vector_Magnitude(deltaPosition));

        Gravity_InputController.Instance.CameraTarget.options.Add(new Dropdown.OptionData() { text = "Free Roam" });

        Gravity_InputController.Instance.DropBoxGraphTarget.options.Add(new Dropdown.OptionData() { text = "Particle 1" });
        Gravity_InputController.Instance.CameraTarget.options.Add(new Dropdown.OptionData() { text = "Particle 1" });
        Gravity_InputController.Instance.DropBoxPlanet.options.Add(new Dropdown.OptionData() { text = "Particle 1" });

        Gravity_InputController.Instance.DropBoxGraphTarget.options.Add(new Dropdown.OptionData() { text = "Particle 2" });
        Gravity_InputController.Instance.CameraTarget.options.Add(new Dropdown.OptionData() { text = "Particle 2" });
        Gravity_InputController.Instance.DropBoxPlanet.options.Add(new Dropdown.OptionData() { text = "Particle 2" });

        Gravity_InputController.Instance.DropBoxPlanet.options.Add(new Dropdown.OptionData() { text = "Add Particle" });

        Gravity_InputController.Instance.DropBoxPlanet.RefreshShownValue();
        Gravity_InputController.Instance.CameraTarget.RefreshShownValue();
        Gravity_InputController.Instance.DropBoxPlanet.RefreshShownValue();
    }

    private static void DestroyObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}
