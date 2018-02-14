using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Collisions_InputController : MonoBehaviour {

    //Stores refernce to the UI controller allowing external classes to access the UI
	public static Collisions_InputController Instance;

    #region UI References

    #region Simulation Inputs
    //Reference to simulation speed slider
    public Slider Slider_SimulationSpeed;
    //Reference to Label which displays the current simulation speed to user
	public Text Label_SimulationSpeed;

    //When simulation speed changes the variable storing the simulation speed in the simulation controller must update
    public void OnSliderSimulationSpeedChanged()
    {
        //Rounding to 2 Decimal places
        string value2DP = Slider_SimulationSpeed.value.ToString("n2");
        //Updates the label's text
        Label_SimulationSpeed.text = "Speed = " + value2DP.ToString() + "x";
        CollisionsSimulationController.SimulationSpeed = Slider_SimulationSpeed.value;
    }
    //When play gets clicked simulation must begin
    public void OnPlayClicked()
	{
		CollisionsSimulationController.isSimulating = true;
	}
    //When pause gets clicked simulation must stop
	public void OnPauseClicked()
	{
		CollisionsSimulationController.isSimulating = false;
	}
    #endregion

    //Reference to the Slider storing the coefficient of restitution for the border walls
    public Slider Slider_BorderRestitution;
    //Label storing the value of the slider
    public Text Label_BorderRestitution;

    //When value gets changed the Label must get updated
    public void OnSliderBorderRestitutionChanged()
    {
        //Changes to label to store current slider value
        OnSliderChanged(Slider_BorderRestitution, Label_BorderRestitution);
    }

	#region Particle Inputs

    //Dropbox containing particle options
	public Dropdown DropBoxParticle;
    //Current particle selected from the dropbox
	public int ParticleIndexSelected = 0;

    //Inputs for velocity in X and Y dimention
	public InputField InputField_VelocityX;
	public InputField InputField_VelocityY;

    //Slider for the mass
	public Slider Slider_Mass;
    //Label storing value of slider
	public Text Label_Mass;

    //Slider for the restitution of particle
	public Slider Slider_Restitution;
    //Label storing value of slider
	public Text Label_Restitution;

    //Slider for the radius of particle
	public Slider Slider_Radius;
    //Label storing value of slider
	public Text Label_Radius;
	#endregion

	#endregion

	#region ParticleSliderUpdates
    //When slider is changed must update label and current particles value
	public void OnMassSliderChanged()
	{
        //Updates Label
		OnSliderChanged(Slider_Mass, Label_Mass);
		CollisionsParticle.ParticleInstances [ParticleIndexSelected].mass = Slider_Mass.value;
	}
    //When slider is changed must update label and current particles value
    public void OnRestitutionChanged()
	{
        //Updates Label
        OnSliderChanged(Slider_Restitution, Label_Restitution);
		CollisionsParticle.ParticleInstances [ParticleIndexSelected].restitution = Slider_Restitution.value;
	}
    //When slider is changed must update label and current particles value
    public void OnRadiusChanged()
	{
        //Updates Label
        OnSliderChanged(Slider_Radius, Label_Radius);
		CollisionsParticle.ParticleInstances [ParticleIndexSelected].radius = Slider_Radius.value;
	}
    //Updates the labels text to store the value of slider rounded to 2 D.P
	public void OnSliderChanged(Slider sliderChanged , Text LabelToUpdate)
	{
		//Rounding to 2 Decimal places
		string value2DP = sliderChanged.value.ToString("n2");
		//Updates the label's text
		LabelToUpdate.text = value2DP;
	}
    #endregion

    #region Velocity Update
    //When update velocity is clicked the input fields must be checked
    //if they are not empty then the dimention they are not empty in will be the new initial velocityu of the particle
    public void OnUpdateVelocityClicked()
	{
        //gets current initial velocity
		Vector2 Velocity = CollisionsParticle.ParticleInstances [ParticleIndexSelected].initialVelocity;
        //If not empty
		if (InputField_VelocityX.text != "")
		{
            //float.parse is a cast from string to float
			Velocity.x = float.Parse(InputField_VelocityX.text);
		}
        //If not empty
		if (InputField_VelocityY.text != "")
		{
            //float.parse is a cast from string to float
            Velocity.y = float.Parse(InputField_VelocityY.text);
		}
        //Updates velocity
		CollisionsParticle.ParticleInstances [ParticleIndexSelected].initialVelocity = Velocity;
	}
    #endregion

    #region Constructors / Start
    //Called when scene is first loaded
    public void Start()
	{
        //Creates the reference to the UI instance
		Instance = this;
	}
    #endregion

    #region DropBox updates
    //Ran when the value of DropBox_Particle is changed
    //Must check if add particle has been selected and if so add it
    public void OnDropBoxParticleChanged()
	{
        //Max number of options in dropbox
		int maximum = DropBoxParticle.options.Count;
		int currentValue = DropBoxParticle.value;
        //If the last option is selected , which is "Add Particle" then a particle must be created
		if (currentValue == maximum-1)
		{
            //Adds additional particle to dropbox
			AddOptionToDropBox (maximum);
            //Creates new particle
			CollisionsParticle.ParticleInstances.Add (new CollisionsParticle ());
		}
        ParticleIndexSelected = DropBoxParticle.value;
        //Updates the UI 
        UpdateValues();
	}
    //Adds another particle option to the dropbox and upates the "Add particle" option
	private void AddOptionToDropBox (int size)
	{
		string _text = "Particle "+(size).ToString();
        //Sets new options to be the next particle
		DropBoxParticle.options [size-1] = new Dropdown.OptionData () { text = _text };
		_text = "Add Particle";
        //Option to add particle added at end of list
		DropBoxParticle.options.Add(new Dropdown.OptionData() { text = _text });
        //Updates option selected
		DropBoxParticle.value = size - 1;
		DropBoxParticle.RefreshShownValue ();
	}

	//Updates values in the User interface depending on the particle selected by the user
	private void UpdateValues()
	{
		//Gets the particle index value fromt the dropbox
		int current = DropBoxParticle.value;
		//particle may not be created yet therefore try ,catch exception prevents error.
		try
		{
			//Updates UI using a particles values
			UpdateUI(CollisionsParticle.ParticleInstances[current]);
		}
		catch (ArgumentOutOfRangeException)
		{
			//If no particle is found the default values should be used
			ResetUI();
		}
	}
    #endregion

    #region Update and Resetting UI

    //Updates UI with values from a particle
    public void UpdateUI(CollisionsParticle values)
	{
		InputField_VelocityX.text = values.currentVelocity.x.ToString ();
		InputField_VelocityY.text = values.currentVelocity.y.ToString ();
		Slider_Mass.value = values.mass;
		Slider_Restitution.value = values.restitution;
		Slider_Radius.value = values.radius;
	}
    //Resets UI elements to default values
	public void ResetUI()
	{
		InputField_VelocityX.text = "";
		InputField_VelocityY.text = "";
		Slider_Mass.value = 1;
		Slider_Restitution.value = 1;
		Slider_Radius.value = 1;
	}
    #endregion
}
