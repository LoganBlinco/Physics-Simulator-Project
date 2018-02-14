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

    public Dropdown DropBoxParticleGraph;
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

    #region Particle Infomation / Particle graph selection
    //References to UI elements
    public GameObject PanelParticleInfomation;
    public GameObject PanelParticeGraph;

    //Selecting the particle infomation panel
    public void OnParticleInfomationClicked()
    {
        PanelParticleInfomation.gameObject.SetActive(true);
        PanelParticeGraph.gameObject.SetActive(false);
    }
    //Selecting the particle graph panel
    public void OnParticleGraphClicked()
    {
        PanelParticleInfomation.gameObject.SetActive(false);
        PanelParticeGraph.gameObject.SetActive(true);
    }
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
		CollisionsParticle.ParticleInstances [ParticleIndexSelected].diameter = Slider_Radius.value;
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
        //Selecting the particle infomation panel
        PanelParticleInfomation.gameObject.SetActive(true);
        PanelParticeGraph.gameObject.SetActive(false);
        PanelAbout.gameObject.SetActive(false);

        BorderLeft = GameObject.Find("BorderLeft");
        BorderRight = GameObject.Find("BorderRight");
        BorderTop = GameObject.Find("BorderTop");
        BorderBottom = GameObject.Find("BorderBottom");
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
        Debug.Log("ran~");
		string _text = "Particle "+(size).ToString();
        //Sets new options to be the next particle
		DropBoxParticle.options [size-1] = new Dropdown.OptionData () { text = _text };
        //The graph's particle selection only needs the adding to the option list
        DropBoxParticleGraph.options.Add(new Dropdown.OptionData() { text = _text });
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
		Slider_Radius.value = values.diameter;
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

    #region Creating random particles

    private int numberOfRandom = 5;

    private GameObject BorderLeft;
    private GameObject BorderRight;
    private GameObject BorderTop;
    private GameObject BorderBottom;

    public void OnRandomClicked()
    {
        //Empties list of particles created
        CollisionsParticle.ParticleInstances = new List<CollisionsParticle>();
        //Destroys any particle gameobjects in the scene
        DestroyObjectsWithTag("Particle");
        //Adds the particles options to the dropbox's (graph and selector)
        AddRandomParticlesToDropBox();
        //Creates the particles with values
        for (int i =0;i<numberOfRandom;i++)
        {
            CreateRandomParticle();
        }
    }
    //Creates and gives random values to particles
    private void CreateRandomParticle()
    {
        //UnityEngine.Random.RandomRange generates a float between min and max
        CollisionsParticle random = new CollisionsParticle();
        random.initialVelocity = new Vector2(
            UnityEngine.Random.RandomRange(-5f, 5f),
            UnityEngine.Random.RandomRange(-5f, 5f));
        random.mass = UnityEngine.Random.Range(1f, 5f);
        random.restitution = UnityEngine.Random.Range(1f,1f);
        Debug.Log(random.restitution);
        random.diameter = UnityEngine.Random.Range(1f, 1.5f);
        Vector3 newPosition = new Vector2(
            UnityEngine.Random.Range(BorderLeft.transform.position.x, BorderRight.transform.position.x),
            UnityEngine.Random.Range(BorderBottom.transform.position.y, BorderTop.transform.position.y));
        random.MyGameObject.transform.position = newPosition;
        CollisionsParticle.ParticleInstances.Add(random);
    }
    //Adds particle options to the dropboxs
    private void AddRandomParticlesToDropBox()
    {
        //Removing all options
        DropBoxParticleGraph.options.Clear();
        DropBoxParticle.options.Clear();

        string _text;
        for (int i = 0; i < numberOfRandom; i++)
        {
            _text = "Particle " + (i + 1).ToString();
            //Sets new options to be the next particle
            DropBoxParticle.options.Add(new Dropdown.OptionData() { text = _text });
            //The graph's particle selection only needs the adding to the option list
            DropBoxParticleGraph.options.Add(new Dropdown.OptionData() { text = _text });
        }
        _text = "Add Particle";
        //Option to add particle added at end of list
        DropBoxParticle.options.Add(new Dropdown.OptionData() { text = _text });
        DropBoxParticle.RefreshShownValue();
    }
    private void DestroyObjectsWithTag(string _tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(_tag);

        for (var i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }
    #endregion

    #region About section
    //Reference to the about panel
    public GameObject PanelAbout;

    public void OnAboutClicked()
    {
        PanelAbout.gameObject.SetActive(true);
        PanelParticeGraph.gameObject.SetActive(false);
        PanelParticleInfomation.gameObject.SetActive(false);
    }

    public void OnAboutOkClicked()
    {
        PanelAbout.gameObject.SetActive(false);
        PanelParticeGraph.gameObject.SetActive(false);
        PanelParticleInfomation.gameObject.SetActive(true);
    }

    #endregion
}