using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CollisionsSimulationController : MonoBehaviour {

    #region UI References
    //Stores the time in which the simulation has been occuring for
    public float simulationTime = 0;
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

    #region Border References
    //Stores references to the border gameobjects in scene
    private GameObject BorderLeft;
    private GameObject BorderRight;
    private GameObject BorderTop;
    private GameObject BorderBottom;
    #endregion

    #region Start Methods
    public void Start()
    {
        //GameObject.Find looks in the scene view for a gameobject with name ("NAME")
        BorderLeft = GameObject.Find("BorderLeft");
        BorderRight = GameObject.Find("BorderRight");
        BorderTop = GameObject.Find("BorderTop");
        BorderBottom = GameObject.Find("BorderBottom");

        isSimulating = false;
    }
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

    #region Update Methods
    // Update is called once per frame
    void Update () {
		if (isSimulating == true)
		{
            //Time between frames multiplied by speed factor
            deltaT = Time.deltaTime * SimulationSpeed;
			UpdateTimeLabel ();
			MoveParticles ();
            UpdateParticleLabel();
            UpdateParticleGraphValues();
			simulationTime += deltaT;
		}
	}
    //Ran to control the updating of the quantity graphs
    private void UpdateParticleGraphValues()
    {
        if (timeTillUpdate <= 0)
        {
            foreach(CollisionsParticle particle in CollisionsParticle.ParticleInstances)
            {
                //Updates the values which will be used to plot
                UpdateGraphPointValues(particle);
            }
            //Index of particle user has selected for graphing
            int index = Collisions_InputController.Instance.DropBoxParticleGraph.value;
            //Updating graphs
            GraphX.GetComponent<GraphMaker>().CreateGraph(CollisionsParticle.ParticleInstances[index].momentumGraphPointsX);
            GraphY.GetComponent<GraphMaker>().CreateGraph(CollisionsParticle.ParticleInstances[index].momentumGraphPointsY);

            timeTillUpdate = timePerUpdate;
        }
        else
        {
            timeTillUpdate -= Time.deltaTime;
        }
    }
    //Updates the momentum points in the X and Y plane for the current simulation time
    private void UpdateGraphPointValues(CollisionsParticle particle)
    {
        particle.momentumGraphPointsX.Add(new Vector2(
            simulationTime,
            particle.mass * particle.currentVelocity.x));
        particle.momentumGraphPointsY.Add(new Vector2(
            simulationTime,
            particle.mass * particle.currentVelocity.y));
    }


    //Updates partic
    private void UpdateParticleLabel()
    {
        //Gets index of current particle selected
        int index = Collisions_InputController.Instance.ParticleIndexSelected;
        //Updates UI to current values
        try
        {
            Collisions_InputController.Instance.UpdateUI(CollisionsParticle.ParticleInstances[index]);
        }
        catch (ArgumentOutOfRangeException) { }
    }

    //Updates time label to match with current simulation time
    private void UpdateTimeLabel()
	{
        //Rounding to 2 decimal places
		string value2DP = simulationTime.ToString("n2");
		Label_Time.text = "Time : " + value2DP + " s";
	}
    //Moves particles using s = ut  (a = 0)
	private void MoveParticles()
	{
        foreach (CollisionsParticle particle in CollisionsParticle.ParticleInstances)
        {
            Vector3 newPosition = particle.MyGameObject.transform.position + particle.currentVelocity * deltaT;
            //Clamps the particles position to inside of the borders
            float diameter = particle.diameter;

            float minX = BorderLeft.transform.position.x + diameter / 2 + (BorderLeft.GetComponent<Renderer>().bounds.size.x / 2) -0.01f;
            float maxX = BorderRight.transform.position.x - diameter / 2 - (BorderRight.GetComponent<Renderer>().bounds.size.x / 2) + 0.01f;

            float minY = BorderBottom.transform.position.y + diameter / 2 + (BorderBottom.GetComponent<Renderer>().bounds.size.y / 2) - 0.01f;
            float maxY = BorderTop.transform.position.y - diameter / 2 - (BorderTop.GetComponent<Renderer>().bounds.size.y / 2) + 0.01f;


            newPosition.x = MyMaths.Clamp(newPosition.x, minX, maxX);
            newPosition.y = MyMaths.Clamp(newPosition.y, minY , maxY);
            particle.MyGameObject.transform.position = newPosition;

        }
	}
    #endregion
}
