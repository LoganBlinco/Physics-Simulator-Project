using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GravitySimulationController : MonoBehaviour {
    //Simulation gravity constant (~ 10^11 times bigger than in real life)
    const float G = 6.7f;
    //Makes 5 in game units be equal to the distance Moon to Earth
    const float distanceMod = 76880;

    #region Variables
    //Stores the time in which the simulation has been occuring for
    public static float simulationTime = 0;
    //Displays to the user the time the simulation has occured for
    public Text Label_Time;

    //Speed multiplier determined by slider
    //Default value is 1
    public static float SimulationSpeed = 1;
    //Boolean state if simulation should be occuring or not
    public static bool isSimulating = false;
    //Change of time between frames
    private float deltaT;
    #endregion

    #region Update Methods

    void Update()
    {
        if (isSimulating == true)
        {
            //Time between frames multiplied by speed factor
            deltaT = Time.deltaTime * SimulationSpeed;
            UpdateTimeLabel();

            UpdateVelocity();
            UpdatePosition();

            Gravity_InputController.Instance.UpdateUI(GravityPlanets.PlanetInstances[Gravity_InputController.Instance.ParticleIndexSelected]);
            simulationTime += deltaT;
        }
    }

    private void UpdatePosition()
    {
        foreach (GravityPlanets planet in GravityPlanets.PlanetInstances)
        {
            planet.MyGameObject.transform.position = planet.MyGameObject.transform.position + planet.currentVelocity * deltaT;
        }
    }

    private void UpdateVelocity()
    {
        foreach(GravityPlanets planet in GravityPlanets.PlanetInstances)
        {
            Vector3 acceleration = GetAccelleration(planet);
            planet.currentVelocity = planet.currentVelocity + acceleration * deltaT;
        }
    }
    //Returns the acceleration due to gravity of a planet
    //Aceeleration from newtons law of gravitation
    //Then F = ma
    private Vector3 GetAccelleration(GravityPlanets attractor)
    {
        Vector3 sum = Vector3.zero;
        Vector3 positionDelta = Vector3.zero;
        foreach (GravityPlanets planet in GravityPlanets.PlanetInstances)
        {
            positionDelta = planet.MyGameObject.transform.position - attractor.MyGameObject.transform.position;
            //Cannot divide by 0
            if (positionDelta != Vector3.zero)
            {
                sum += planet.mass * positionDelta / Mathf.Pow(MyMaths.Vector_Magnitude(positionDelta),3);
            }
        }
        return G * sum;
    }

    //Updates time label to match with current simulation time
    private void UpdateTimeLabel()
    {
        //Rounding to 2 decimal places
        string value2DP = simulationTime.ToString("n2");
        Label_Time.text = "Time : " + value2DP + " s";
    }



    #endregion

}
