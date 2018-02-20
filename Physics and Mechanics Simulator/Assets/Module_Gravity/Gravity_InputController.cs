using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Gravity_InputController : MonoBehaviour {

    //Stores refernce to the UI controller allowing external classes to access the UI
    public static Gravity_InputController Instance;

    public int ParticleIndexSelected =0;

    #region Simulation References

    public Text Label_SimulationTime;
    public Slider Slider_SimulationSpeed;
    public Text Label_SimulationSpeed;

    #endregion

    #region Particle Infomation references

    public InputField Inputfield_VelocityX;
    public InputField Inputfield_VelocityY;

    public Slider Slider_Mass;
    public Text Label_Mass;

    public Slider Slider_Diameter;
    public Text Label_Diameter;

    //Dropbox containing Planet options
    public Dropdown DropBoxPlanet;


    #endregion

    #region Simulation Controls

    public void OnPlayClicked()
    {
        GravitySimulationController.isSimulating = true;
    }
    public void OnPauseClicked()
    {
        GravitySimulationController.isSimulating = false;
    }

    public void OnSliderSimulationSpeedChanged()
    {
        GravitySimulationController.SimulationSpeed = Slider_SimulationSpeed.value;
        //Rounding to 2 Decimal places
        string value2DP = Slider_SimulationSpeed.value.ToString("n2");
        Label_SimulationSpeed.text = "Speed = " + value2DP + "x";
    }
    #endregion

    #region DropBox updates
    //Ran when the value of DropBox_Particle is changed
    //Must check if add particle has been selected and if so add it
    public void OnDropBoxParticleChanged()
    {
        //Max number of options in dropbox
        int maximum = DropBoxPlanet.options.Count;
        int currentValue = DropBoxPlanet.value;
        //If the last option is selected , which is "Add Particle" then a particle must be created
        if (currentValue == maximum - 1)
        {
            //Adds additional particle to dropbox
            AddOptionToDropBox(maximum);
            //Creates new particle
            GravityPlanets.PlanetInstances.Add(new GravityPlanets());
        }
        ParticleIndexSelected = DropBoxPlanet.value;
        //Updates the UI 
        UpdateValues();
    }
    //Adds another particle option to the dropbox and upates the "Add particle" option
    private void AddOptionToDropBox(int size)
    {
        string _text = "Particle " + (size).ToString();
        //Sets new options to be the next particle
        DropBoxPlanet.options[size - 1] = new Dropdown.OptionData() { text = _text };
        _text = "Add Particle";
        //Option to add particle added at end of list
        DropBoxPlanet.options.Add(new Dropdown.OptionData() { text = _text });
        //Updates option selected
        DropBoxPlanet.value = size - 1;
        DropBoxPlanet.RefreshShownValue();
    }

    //Updates values in the User interface depending on the particle selected by the user
    private void UpdateValues()
    {
        //Gets the particle index value fromt the dropbox
        int current = DropBoxPlanet.value;
        //particle may not be created yet therefore try ,catch exception prevents error.
        try
        {
            //Updates UI using a particles values
            UpdateUI(GravityPlanets.PlanetInstances[current]);
        }
        catch (ArgumentOutOfRangeException)
        {
            //If no particle is found the default values should be used
            ResetUI();
        }
    }
    #endregion

    #region Particle Slider Updates
    //When slider is changed must update label and current particles value
    public void OnMassSliderChanged()
    {
        //Updates Label
        OnSliderChanged(Slider_Mass, Label_Mass);
        GravityPlanets.PlanetInstances [ParticleIndexSelected].mass = Slider_Mass.value;
    }
    //When slider is changed must update label and current particles value
    public void OnRadiusChanged()
    {
        //Updates Label
        OnSliderChanged(Slider_Diameter, Label_Diameter);
        Debug.Log(GravityPlanets.PlanetInstances[ParticleIndexSelected].diameter);
        GravityPlanets.PlanetInstances[ParticleIndexSelected].diameter = Slider_Diameter.value;
    }
    //Updates the labels text to store the value of slider rounded to 2 D.P
    public void OnSliderChanged(Slider sliderChanged, Text LabelToUpdate)
    {
        //Rounding to 2 Decimal places
        string value2DP = sliderChanged.value.ToString("n2");
        //Updates the label's text
        LabelToUpdate.text = value2DP;
    }

    #endregion

    #region Start methods

    //When the scene is first loaded the first particle should be in the scene ready for manipulation
    public void Start()
    {
        Instance = this;
        //Assigns prefab to the varaible from the resources folder
        //Generates the first object in the scene
        CreateFirstObject();
    }

    private void CreateFirstObject()
    {
        //Assigns default values to the particle
        GravityPlanets newParticle = new GravityPlanets();
        newParticle.initialVelocity = Vector3.zero;
        newParticle.mass = 1.0f;
        newParticle.diameter = 0.25f;
        //Adds particle to the list which causes the prefab to be instatiated
        GravityPlanets.PlanetInstances.Add(newParticle);
        OnRadiusChanged();
        OnMassSliderChanged();
    }

    #endregion


    #region Updating UI and resetting

    //Updates UI with values from a planet
    public void UpdateUI(GravityPlanets values)
    {
        Inputfield_VelocityX.text = values.currentVelocity.x.ToString();
        Inputfield_VelocityY.text = values.currentVelocity.y.ToString();
        Slider_Mass.value = values.mass;
        Slider_Diameter.value = values.diameter;

        OnMassSliderChanged();
        OnRadiusChanged();
    }
    //Resets UI elements to default values
    public void ResetUI()
    {
        Inputfield_VelocityX.text = "";
        Inputfield_VelocityY.text = "";
        Slider_Mass.value = 1;
        Slider_Diameter.value = 0.25f;
    }


    #endregion

    #region Velocity Update
    //When update velocity is clicked the input fields must be checked
    //if they are not empty then the dimention they are not empty in will be the new initial velocity of the particle
    public void OnUpdateVelocityClicked()
    {
        //gets current initial velocity
        Vector2 Velocity = GravityPlanets.PlanetInstances[ParticleIndexSelected].initialVelocity;
        //If not empty
        if (Inputfield_VelocityX.text != "")
        {
            //float.parse is a cast from string to float
            Velocity.x = float.Parse(Inputfield_VelocityX.text);
        }
        //If not empty
        if (Inputfield_VelocityY.text != "")
        {
            //float.parse is a cast from string to float
            Velocity.y = float.Parse(Inputfield_VelocityY.text);
        }
        //Updates velocity
        GravityPlanets.PlanetInstances[ParticleIndexSelected].initialVelocity = Velocity;
    }
    #endregion

}
