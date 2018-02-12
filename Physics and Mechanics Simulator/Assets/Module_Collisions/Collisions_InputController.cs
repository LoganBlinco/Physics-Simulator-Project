using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Collisions_InputController : MonoBehaviour {

	public static Collisions_InputController Instance;

	#region UI References

	#region Simulation Inputs

	public Slider Slider_SimulationSpeed;
	public Text Label_SimulationSpeed;

	public void OnPlayClicked()
	{
		CollisionsSimulationController.isSimulating = true;
	}
	public void OnPauseClicked()
	{
		CollisionsSimulationController.isSimulating = false;
	}

	public void OnSliderSimulationSpeedChanged()
	{
		//Rounding to 2 Decimal places
		string value2DP = Slider_SimulationSpeed.value.ToString("n2");
		//Updates the label's text
		Label_SimulationSpeed.text = "Speed = " + value2DP.ToString () + "x";

		CollisionsSimulationController.SimulationSpeed = Slider_SimulationSpeed.value;
	}


    #endregion

    public Slider Slider_BorderRestitution;
    public Text Label_BorderRestitution;

    public void OnSliderBorderRestitutionChanged()
    {
        OnSliderChanged(Slider_BorderRestitution, Label_BorderRestitution);
    }

	#region Particle Inputs

	public Dropdown DropBoxParticle;
	private int ParticleIndexSelected = 0;

	public InputField InputField_VelocityX;
	public InputField InputField_VelocityY;

	public Slider Slider_Mass;
	public Text Label_Mass;

	public Slider Slider_Restitution;
	public Text Label_Restitution;

	public Slider Slider_Radius;
	public Text Label_Radius;


	#endregion

	#endregion

	#region ParticleSliderUpdates

	public void OnMassSliderChanged()
	{
		OnSliderChanged(Slider_Mass, Label_Mass);
		CollisionsParticle.ParticleInstances [ParticleIndexSelected].mass = Slider_Mass.value;
	}

	public void OnRestitutionChanged()
	{
		OnSliderChanged(Slider_Restitution, Label_Restitution);
		CollisionsParticle.ParticleInstances [ParticleIndexSelected].restitution = Slider_Restitution.value;
	}
	public void OnRadiusChanged()
	{
		OnSliderChanged(Slider_Radius, Label_Radius);
		CollisionsParticle.ParticleInstances [ParticleIndexSelected].radius = Slider_Radius.value;
	}

	public void OnSliderChanged(Slider sliderChanged , Text LabelToUpdate)
	{
		//Rounding to 2 Decimal places
		string value2DP = sliderChanged.value.ToString("n2");
		//Updates the label's text
		LabelToUpdate.text = value2DP;
	}


	#endregion


	public void OnUpdateVelocityClicked()
	{
		Vector2 Velocity = CollisionsParticle.ParticleInstances [ParticleIndexSelected].initialVelocity;
		if (InputField_VelocityX.text != "")
		{
			Velocity.x = float.Parse(InputField_VelocityX.text);
		}
		if (InputField_VelocityY.text != "")
		{
			Velocity.y = float.Parse(InputField_VelocityY.text);
		}
		CollisionsParticle.ParticleInstances [ParticleIndexSelected].initialVelocity = Velocity;
	}




	public void Start()
	{
		Instance = this;
	}

	#region DropBox updates
	//Ran when the value of DropBox_Particle is changed
	//Must check if add particle has been selected and if so add it
	public void OnDropBoxParticleChanged()
	{
		int maximum = DropBoxParticle.options.Count;
		int currentValue = DropBoxParticle.value;

		if (currentValue == maximum-1)
		{
			AddOptionToDropBox (maximum);
			CollisionsParticle.ParticleInstances.Add (new CollisionsParticle ());
		}
		UpdateValues ();
		ParticleIndexSelected = DropBoxParticle.value;
	}

	private void AddOptionToDropBox (int size)
	{
		string _text = "Particle "+(size).ToString();
		DropBoxParticle.options [size-1] = new Dropdown.OptionData () { text = _text };
		_text = "Add Particle";
		DropBoxParticle.options.Add(new Dropdown.OptionData() { text = _text });

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
		catch (ArgumentOutOfRangeException e)
		{
            Debug.Log("Cathc2");
			//If no particle is found the default values should be used
			ResetUI();
		}
	}
	#endregion


	private void UpdateUI(CollisionsParticle values)
	{
		InputField_VelocityX.text = values.currentVelocity.x.ToString ();
		InputField_VelocityY.text = values.currentVelocity.y.ToString ();
		Slider_Mass.value = values.mass;
		Slider_Restitution.value = values.restitution;
		Slider_Radius.value = values.radius;

		//Must add color
	}

	private void ResetUI()
	{
		InputField_VelocityX.text = "";
		InputField_VelocityY.text = "";
		Slider_Mass.value = 1;
		Slider_Restitution.value = 1;
		Slider_Radius.value = 1;
	}
}
