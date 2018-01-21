using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle {

    //Stores reference to all instances of Particle created
    public static List<Particle> Instances = new List<Particle>();
    //Stores a copy of the particles instance before simulation occured
    public Particle beforeSimulation;

    //List of struct storing infomation for graphing motion
    public List<ParticleProperties> ParticleValues = new List<ParticleProperties>();


    #region Suvat Variables
    public Vector3 Displacement;
    public Vector3 InitialVelocity;
    public Vector3 FinalVelocity;
    public Vector3 Acceleration;
    public float Time;
    public Vector3 InitialPosition;

    #endregion

    #region Key and inValidInput
    //Key stores the state of each variable in each dimention.1 meaning has been calculated , 0 means has not been calculated
    public string[] Key = { "00000", "00000", "00000" };
    //Dimentions has invalid input state
    public bool[] inValidInput = { false, false, false };
#endregion

    //Ability for particle to collide with other particles
    public bool canCollide;
    //Ability for particle to be attracted and cause attraction by gravity.
    public bool hasGravity;

    #region NumberOfInputs variables and methods
    //Number of inputs which have been calclated in each dimention
    private int[] NumberOfInputs = new int[3];

    //Re-calculates the NumberOfInputs by using the Key
    public void UpdateNumberOfInputs()
    {
        int dimention;
        int character;
        //for every dimention
        for (dimention = 0; dimention < 3; dimention++)
        {
            int numInputs = 0;
            //for every character
            for (character = 0; character < Key[dimention].Length; character++)
            {
                //if character = '1' , meaning value has been calculated
                if (Key[dimention][character] == '1')
                {
                    //valid inputs increases by one
                    numInputs += 1;
                }
            }
            SetNumberOfInputs(dimention, numInputs);
        }
    }
    //Updates NumberOfInputs then,
    //Returns NumberOfInputs
    public int[] GetNumberOfInputs()
    {
        UpdateNumberOfInputs();
        return NumberOfInputs;
    }
    //Sets NumberOfInputs in an index value
    public void SetNumberOfInputs(int index, int value)
    {
        NumberOfInputs[index] = value;
    }
    #endregion

    #region Mass , Restitution and radius
    private float _mass;
    public float Mass
    {
        //Mass can only be positive
        set { _mass = MyMaths.Magnitude(value); }
        get { return _mass; }
    }
    private float _restitution;
    public float Restitution
    {
        //Resitituion is between 0 and 1 always
        set { _restitution = MyMaths.Clamp(value, 0, 1); }
        get { return _restitution; }
    }
    private float _radius;
    public float Radius
    {
        //Radius is always positive
        set { _radius = MyMaths.Magnitude(value); }
        get { return _radius; }
    }
    #endregion
}