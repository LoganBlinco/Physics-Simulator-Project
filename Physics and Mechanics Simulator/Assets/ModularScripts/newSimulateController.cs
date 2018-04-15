using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class newSimulateController : MonoBehaviour {

    #region Equation constants

    //Newtons universal gravitational constant
    static float NewtonG = 6.7f * Mathf.Pow(10, -11);

    //6 units is equal to the distance earth to moon
    static float newDistanceMod = 384 * Mathf.Pow(10, 6) / 6;
    //1 mass is equal to the earth mass
    static float newMassMod = 6.0f * Mathf.Pow(10, 24);
    //Module wide speed up
    static float timeMod = 100;
    //New force multiplier (the new G)
    public static float G = timeMod * NewtonG * newMassMod / Mathf.Pow(newDistanceMod, 2);

    #endregion

    #region Graph variables
    public GameObject GraphX;
    public GameObject GraphY;

    //Time for each graph update
    //Lower value = more processing
    private float timePerUpdate = 1f;
    //Time remaining until next graph update
    private float timeTillUpdate = 0.0f;
    #endregion

    public Dropdown DropBoxTarget;
    public Text TimeLabel;

    #region Variables

    //Stores the time in which the simulation has been occuring for
    public float simulationTime = 0;

    //Speed multiplier determined by slider
    //Default value is 1
    public static float SimulationSpeed = 1;
    //Boolean state if simulation should be occuring or not
    public static bool isSimulating = false;
    //Change of time between frames
    private float deltaT;
    #endregion

    #region Simulation type selected
    //Types of simulation the controller supports
    public enum SimulationTypes
    {
        Suvat,
        Collisions,
        Gravity
    }
    //current simulation type
    public SimulationTypes simulationType;

    #endregion

    void Update()
    {
        if (isSimulating == true && newParticle.ParticleInstances.Count >= 1)
        {
            //Controls updating the graph
            UpdateParticleGraphValues();
            //Time between frames multiplied by speed factor
            deltaT = Time.deltaTime * SimulationSpeed;
            UpdateTimeLabel();

            UpdateVelocity();
            UpdatePosition();


            UpdateUI();
            simulationTime += deltaT;
            //Collisions module needs to clamp particles inside of border
            if (simulationType == SimulationTypes.Collisions)
            {
                ClampParticles();
            }
        }
    }
    //Clamps particles from the collisions module inside the borders
    private void ClampParticles()
    {
        //GameObject.Find looks in the scene view for a gameobject with name ("NAME")
        GameObject BorderLeft = GameObject.Find("BorderLeft");
        GameObject BorderRight = GameObject.Find("BorderRight");
        GameObject BorderTop = GameObject.Find("BorderTop");
        GameObject BorderBottom = GameObject.Find("BorderBottom");

        //buffer which changes the border size of the confines
        float buffer = 0.01f;    
        foreach (newParticle particle in newParticle.ParticleInstances)
        {
            Vector3 newPosition = particle.MyGameObject.transform.position + particle.currentVelocity * deltaT;
            //Clamps the particles position to inside of the borders
            float diameter = particle.diameter;

            float minX = BorderLeft.transform.position.x + diameter / 2 + (BorderLeft.GetComponent<Renderer>().bounds.size.x / 2) - buffer;
            float maxX = BorderRight.transform.position.x - diameter / 2 - (BorderRight.GetComponent<Renderer>().bounds.size.x / 2) + buffer;

            float minY = BorderBottom.transform.position.y + diameter / 2 + (BorderBottom.GetComponent<Renderer>().bounds.size.y / 2) - buffer;
            float maxY = BorderTop.transform.position.y - diameter / 2 - (BorderTop.GetComponent<Renderer>().bounds.size.y / 2) + buffer;

            //Clamps position to be inside of the border objects
            newPosition.x = MyMaths.Clamp(newPosition.x, minX, maxX);
            newPosition.y = MyMaths.Clamp(newPosition.y, minY, maxY);
            particle.MyGameObject.transform.position = newPosition;
        }
    }
    //Updates particle selected's values in the UI
    private void UpdateUI()
    {
        switch(simulationType)
        {
            case SimulationTypes.Suvat:
                Suvat_UiController.instance.UpdateUI(newParticle.ParticleInstances[Suvat_UiController.instance.DropBox_Particle.value]);
                break;
            case SimulationTypes.Collisions:
                Collisions_InputController.Instance.UpdateUI(newParticle.ParticleInstances[Collisions_InputController.Instance.ParticleIndexSelected]);
                break;
            case SimulationTypes.Gravity:
                Gravity_InputController.Instance.UpdateUI(newParticle.ParticleInstances[Gravity_InputController.Instance.ParticleIndexSelected]);
                break;
        }
    }

    private void UpdateTimeLabel()
    {
        //Rounding to 2 decimal places
        string value2DP = simulationTime.ToString("n2");
        TimeLabel.text = "Time : " + value2DP + " s";
    }

    private void UpdatePosition()
    {
        foreach (newParticle particle in newParticle.ParticleInstances)
        {
            particle.MyGameObject.transform.position = particle.MyGameObject.transform.position + particle.currentVelocity * deltaT;
        }
    }


    #region Velocity and accleration
    private void UpdateVelocity()
    {
        foreach (newParticle particle in newParticle.ParticleInstances)
        {
            Vector3 acceleration = GetAccelleration(particle);
            particle.currentVelocity = particle.currentVelocity + acceleration * deltaT;
        }
    }

    private Vector3 GetAccelleration(newParticle particle)
    {
        Vector3 accelerationCounter = Vector3.zero;
        if (particle.hasGravity && particle.hasMass)
        {
            accelerationCounter = GetAccellerationGravity(particle);
        }
        if (particle.hasAcceleration)
        {
            accelerationCounter += particle.acceleration;
        }
        return accelerationCounter;
    }

    private Vector3 GetAccellerationGravity(newParticle particle)
    {
        Vector3 sum = Vector3.zero;
        Vector3 positionDelta = Vector3.zero;
        foreach (newParticle planet in newParticle.ParticleInstances)
        {
            if (planet.hasGravity && planet.hasMass)
            {
                positionDelta = planet.MyGameObject.transform.position - particle.MyGameObject.transform.position;
                //Cannot divide by 0
                if (positionDelta != Vector3.zero)
                {
                    sum += planet.mass * positionDelta / Mathf.Pow(MyMaths.Vector_Magnitude(positionDelta), 3);
                }
            }

        }
        return G * sum;
    }

    #endregion

    #region Updating Graphs
    private void UpdateParticleGraphValues()
    {
        //Only updates every X seconds
        if (timeTillUpdate <= 0)
        {
            //Updates the points stored for every planet created
            UpdateGraphValues();
            //Index of particle user has selected for graphing
            int index = DropBoxTarget.value;
            //Updating graphs
            newParticle temp = newParticle.ParticleInstances[index];
            if (temp.hasGraphingValuesSpeed)
            {
                GraphX.GetComponent<GraphMaker>().CreateGraph(temp.graphingValuesSpeed);
            }
            if (temp.hasGraphingValuesAcceleration)
            {
                GraphY.GetComponent<GraphMaker>().CreateGraph(temp.graphingValuesAcceleration);
            }
            else
            {
                if (temp.hasGraphingValuesMomentumX)
                {
                    GraphX.GetComponent<GraphMaker>().CreateGraph(temp.graphingValuesMomentumX);
                }
                if (temp.hasGraphingValuesMomentumY)
                {
                    GraphY.GetComponent<GraphMaker>().CreateGraph(temp.graphingValuesMomentumY);
                }
                if (temp.hasGraphingValuesDistance)
                {
                    GraphY.GetComponent<GraphMaker>().CreateGraph(temp.graphingValuesDistance);
                }
            }
            timeTillUpdate = timePerUpdate;
        }
        else
        {
            timeTillUpdate -= Time.deltaTime;
        }
    }

    //Updates the values of points to plot for every planet
    private void UpdateGraphValues()
    {
        foreach (newParticle particle in newParticle.ParticleInstances)
        {
            if (particle.hasGraphingValuesAcceleration)
            {
                UpdateGraphValuesAcceleration(particle);
            }
            if (particle.hasGraphingValuesSpeed)
            {
                UpdateGraphValuesSpeed(particle);
            }
            if (particle.hasGraphingValuesMomentumX)
            {
                UpdateGraphValuesMomentumX(particle);
            }
            if (particle.hasGraphingValuesMomentumY)
            {
                UpdateGraphValuesMomentumY(particle);
            }
            if(particle.hasGraphingValuesDistance)
            {
                UpdateGraphValuesDistance(particle);
            }

        }
    }

    private void UpdateGraphValuesDistance(newParticle particle)
    {
        particle.graphingValuesDistance.Add(new Vector2(
            simulationTime,
            (particle.MyGameObject.transform.position - particle.initialPosition).magnitude));
    }

    private void UpdateGraphValuesMomentumY(newParticle particle)
    {
        particle.graphingValuesMomentumY.Add(new Vector2(
            simulationTime,
            particle.mass * particle.currentVelocity.y));
    }

    private void UpdateGraphValuesMomentumX(newParticle particle)
    {
        particle.graphingValuesMomentumX.Add(new Vector2(
        simulationTime,
        particle.mass * particle.currentVelocity.x));
    }

    private void UpdateGraphValuesSpeed(newParticle particle)
    {
        particle.graphingValuesSpeed.Add(new Vector2(simulationTime, particle.currentVelocity.magnitude));
    }

    private void UpdateGraphValuesAcceleration(newParticle particle)
    {
        particle.graphingValuesAcceleration.Add(new Vector2(simulationTime, GetAccelleration(particle).magnitude));
    }
    #endregion
}
