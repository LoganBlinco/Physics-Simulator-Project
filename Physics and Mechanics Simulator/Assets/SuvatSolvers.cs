using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuvatSolvers : MonoBehaviour
{
    //Ran when a Suvat calculation is too occur on the passed in instance of Particle.
    public static Particle FindEquation(Particle values)
    {
        //i is the dimention to calculate
        int i = 0;
        //Dictionary reference between key and function to be ran
        //When the key is indexed the function is ran with parameters values and i
        var Equations = new Dictionary<string, Action>
            {
                { "00111", () => RanOn_00111(values, i) },
                { "01110", () => RanOn_01110(values, i) },
                { "01011", () => RanOn_01011(values, i) },
                { "01101", () => RanOn_01101(values, i) },
                { "01111", () => RanOn_01111(values, i) },
                { "10011", () => RanOn_10011(values, i) },
                { "10101", () => RanOn_10101(values, i) },
                { "10110", () => RanOn_10110(values, i) },
                { "10111", () => RanOn_10111(values, i) },
                { "11001", () => RanOn_11001(values, i) },
                { "11010", () => RanOn_11010(values, i) },
                { "11011", () => RanOn_11011(values, i) },
                { "11100", () => RanOn_11100(values, i) },
                { "11101", () => RanOn_11101(values, i) },
                { "11110", () => RanOn_11110(values, i) },
                { "11111", () => RanOn_11111(values, i) },
            };

        //The emergency escape incase something goes wrong
        //Prevents an infinite loop
        int j = 0;
        int maxj = 10;
        //While their are still values to calculate in any dimentions
        //and not all dimentions have invalid inputs
        while (((values.GetNumberOfInputs()[0] != 5 && values.GetNumberOfInputs()[1] != 5 && values.GetNumberOfInputs()[2] != 5) || (values.inValidInput[0] == false || values.inValidInput[1] == false || values.inValidInput[2] == false)) && (j < maxj))
        {
            //Dimention has 3 or more inputs
            if (values.GetNumberOfInputs()[0] >= 3)
            {
                i = 0;
                //Calls the corressponding equation
                Equations[values.Key[i]]();
            }
            //Dimention has 3 or more inputs
            if (values.GetNumberOfInputs()[1] >= 3)
            {
                i = 1;
                //Calls the corressponding equation
                Equations[values.Key[i]]();
            }
            //Dimention has 3 or more inputs
            if (values.GetNumberOfInputs()[2] >= 3)
            {
                i = 2;
                //Calls the corressponding equation
                Equations[values.Key[i]]();
            }
            j++;
        }
        return values;
    }
    //S = (v^2-u^2) / 2a
    private static void RanOn_01110(Particle values, int i)
    {
        //cannot devide by 0.Therefore would be invalid input
        if (values.Acceleration[i] != 0)
        {
            //S = (v^2-u^2) / 2a
            values.Displacement[0] = (Mathf.Pow(values.FinalVelocity[i], 2) - Mathf.Pow(values.InitialVelocity[i], 2)) / (2 * values.Acceleration[i]);
            values.Key[i] = ReplaceAtIndex(0, '1', values.Key[i]);
        }
        else
        {
            values.inValidInput[i] = true;
        }
    }

    //S = vt - 1/2 a t^2
    private static void RanOn_00111(Particle values, int i)
    {
        //S = vt - 1/2 a t^2
        values.Displacement[i] = values.FinalVelocity[i] * values.Time - 0.5f * (values.Acceleration[i] * Mathf.Pow(values.Time, 2));
        values.Key[i] = ReplaceAtIndex(0, '1', values.Key[i]);
    }

    //sets inValidInput = true (all inputs calculated)
    private static void RanOn_11111(Particle values, int i)
    {
        values.inValidInput[i] = true;
    }
    // s = ut + 1/2 * a * t^2
    public static void RanOn_01011(Particle values, int i)
    {
        values.Displacement[i] = values.InitialVelocity[i] * values.Time + 0.5f * values.Acceleration[i] * Mathf.Pow(values.Time, 2);
        values.Key[i] = ReplaceAtIndex(0, '1', values.Key[i]);
    }
    // s = 1/2 (u + v) t
    public static void RanOn_01101(Particle values, int i)
    {
        values.Displacement[i] = 0.5f * (values.InitialVelocity[i] + values.FinalVelocity[i]) * values.Time;
        values.Key[i] = ReplaceAtIndex(0, '1', values.Key[i]);
    }
    //Input condition used 01101
    public static void RanOn_01111(Particle values, int i)
    {
        RanOn_01101(values, i);
    }
    // s = ut + 1/2 * a * t^2 rearranged for u
    public static void RanOn_10011(Particle values, int i)
    {
        values.InitialVelocity[i] = (values.Displacement[i] / values.Time) - 0.5f * (values.Acceleration[i] * values.Time);
        values.Key[i] = ReplaceAtIndex(1, '1', values.Key[i]);
    }
    // s = 1/2 (u + v)t rearranged for u
    public static void RanOn_10101(Particle values, int i)
    {
        //Cannot divide by 0
        if (values.Time == 0)
        {
            values.inValidInput[i] = true;
        }
        else
        {
            values.InitialVelocity[i] = 2 * (values.Displacement[i] / values.Time) - values.FinalVelocity[i];
            values.Key[i] = ReplaceAtIndex(1, '1', values.Key[i]);
        }
    }
    // V^2 = u^2 + 2as reaaragned for u
    public static void RanOn_10110(Particle values, int i)
    {
        float InsideRoot = Mathf.Pow(values.FinalVelocity[i], 2) - 2 * values.Acceleration[i] * values.Displacement[i];
        //Square root must be positive
        if (InsideRoot < 0)
        {
            values.inValidInput[i] = true;
        }
        else
        {
            values.InitialVelocity[i] = Mathf.Sqrt(InsideRoot);
            values.Key[i] = ReplaceAtIndex(1, '1', values.Key[i]);
        }
    }
    //v = u + at , rearranged for u
    public static void RanOn_10111(Particle values, int i)
    {
        values.InitialVelocity[i] = values.FinalVelocity[i] - values.Acceleration[i] * values.Time;
        values.Key[i] = ReplaceAtIndex(1, '1', values.Key[i]);
    }

    //s = 0.5 * *u+v)t , rearranged for v
    public static void RanOn_11001(Particle values, int i)
    {
        //Cannot divide by 0 therefore will be invalid input.
        if (values.Time == 0)
        {
            values.inValidInput[i] = true;
        }
        else
        {
            values.FinalVelocity[i] = 2 * (values.Displacement[i] / values.Time) - values.InitialVelocity[i];
            values.Key[i] = ReplaceAtIndex(2, '1', values.Key[i]);

        }
    }
    // V^2 = u^2 + 2as
    public static void RanOn_11010(Particle values, int i)
    {
        float insideRoot = Mathf.Pow(values.InitialVelocity[i], 2) + (2 * values.Acceleration[i] * values.Displacement[i]);
        //Square root must be positive.Therefore invalid input if not true
        if (insideRoot < 0)
        {
            values.inValidInput[i] = true;
        }
        else
        {
            values.FinalVelocity[i] = MyMaths.SquareRoot(insideRoot);
            //Checking direction which is lost by squaring
            if (values.Acceleration[i] < 0 && values.Displacement[i] <= 0)
            {
                values.FinalVelocity[i] = -values.FinalVelocity[i];
            }
            values.Key[i] = ReplaceAtIndex(2, '1', values.Key[i]);
        }
    }
    //V = u + at
    public static void RanOn_11011(Particle values, int i)
    {
        values.FinalVelocity[i] = values.InitialVelocity[i] + values.Acceleration[i] * values.Time;
        values.Key[i] = ReplaceAtIndex(2, '1', values.Key[i]);
    }
    // V^2 = u^2 + 2as reaaragned for a
    public static void RanOn_11100(Particle values, int i)
    {
        //Cannot divide by 0 which would cause invalid inputs
        if (values.Displacement[i] == 0)
        {
            values.inValidInput[i] = true;
        }
        else
        {
            values.Acceleration[i] = (Mathf.Pow(values.FinalVelocity[i], 2) - Mathf.Pow(values.InitialVelocity[i], 2)) / (2 * values.Displacement[i]);
            values.Key[i] = ReplaceAtIndex(3, '1', values.Key[i]);
        }
    }
    //V = u + at , rearranged for a
    public static void RanOn_11101(Particle values, int i)
    {
        //Cannot divide by 0
        if (values.Time == 0)
        {
            values.inValidInput[i] = true;
        }
        else
        {
            values.Acceleration[i] = (values.FinalVelocity[i] - values.InitialVelocity[i]) / values.Time;
            values.Key[i] = ReplaceAtIndex(3, '1', values.Key[i]);
        }
    }
    //V = u + at , rearranged for t
    public static void RanOn_11110(Particle values, int i)
    {
        //Diffrent equations are used depending on acceleration
        if (values.Acceleration[i] == 0)
        {
            //Cannot divide by 0
            if ((values.InitialVelocity[i] + values.FinalVelocity[i]) != 0)
            {
                values.Time = 2 * values.Displacement[i] / (values.InitialVelocity[i] + values.FinalVelocity[i]);
            }
            else
            {
                values.inValidInput[i] = true;
            }
        }
        else
        {
            //Time calculations change the Key in 3 dimentions because time is shared
            values.Time = (values.FinalVelocity[i] - values.InitialVelocity[i]) / values.Acceleration[i];
            values.Key[0] = ReplaceAtIndex(4, '1', values.Key[0]);
            values.Key[1] = ReplaceAtIndex(4, '1', values.Key[1]);
            values.Key[2] = ReplaceAtIndex(4, '1', values.Key[2]);
        }
    }


    //Replaces a character of a string at the index value.
    public static string ReplaceAtIndex(int index, char value, string word)
    {
        try
        {
            //Creates a char array for each character in word
            char[] letters = word.ToCharArray();
            letters[index] = value;
            //creates a string from the array
            return new string(letters);
        }
        //if index out of range
        catch
        {
            return "";
        }

    }

}