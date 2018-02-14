using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CollisionsSimulationController : MonoBehaviour {

    #region UI References
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
    }
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
			simulationTime += deltaT;
		}
	}
    //Updates partic
    private void UpdateParticleLabel()
    {
        //Gets index of current particle selected
        int index = Collisions_InputController.Instance.ParticleIndexSelected;
        //Updates UI to current values
        Collisions_InputController.Instance.UpdateUI(CollisionsParticle.ParticleInstances[index]);
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
            newPosition.x = MyMaths.Clamp(newPosition.x, BorderLeft.transform.position.x, BorderRight.transform.position.x);
            newPosition.y = MyMaths.Clamp(newPosition.y, BorderBottom.transform.position.y, BorderTop.transform.position.y);

            particle.MyGameObject.transform.position = newPosition;
        }
	}
    #endregion
}
