using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle {

    public static List<Particle> Instances = new List<Particle>();
    public Particle beforeSimulation;

    public Vector3 Displacement;
    public Vector3 InitialVelocity;
    public Vector3 FinalVelocity;
    public Vector3 Acceleration;
    public float Time;
    public Vector3 InitialPosition;

    public string[] Key = { "00000", "00000", "00000" };
    public bool[] inValidInput = { false, false, false };

    public bool canCollide;
    public bool hasGravity;

    private int[] NumberOfInputs = new int[3];
    public void UpdateNumberOfInputs()
    {
        int dimention;
        int character;
        for (dimention = 0; dimention < 3; dimention++)
        {
            int numInputs = 0;
            for (character = 0; character < Key[dimention].Length; character++)
            {
                if (Key[dimention][character] == '1')
                {
                    numInputs += 1;
                }
            }
            SetNumberOfInputs(dimention, numInputs);
        }
    }
    public int[] GetNumberOfInputs()
    {
        UpdateNumberOfInputs();
        return NumberOfInputs;
    }
    public void SetNumberOfInputs(int index, int value)
    {
        NumberOfInputs[index] = value;
    }
    private float _mass;
    public float Mass
    {
        set { _mass = MyMaths.Magnitude(value); }
        get { return _mass; }
    }
    private float _restitution;
    public float Restitution
    {
        set { _restitution = MyMaths.Clamp(value, 0, 1); }
        get { return _restitution; }
    }
    private float _radius;
    public float Radius
    {
        set { _radius = MyMaths.Magnitude(value); }
        get { return _radius; }
    }
}