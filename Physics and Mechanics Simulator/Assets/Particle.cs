using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

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

    public float Mass
    {
        set { Mass = MyMaths.Magnitude(value); }
        get { return Mass; }
    }

    public float Restitution
    {
        set { Restitution = MyMaths.Clamp(value, 0, 1); }
        get { return Restitution; }
    }
    public float Radius
    {
        set { Radius = MyMaths.Magnitude(value); }
        get { return Radius; }
    }
}
