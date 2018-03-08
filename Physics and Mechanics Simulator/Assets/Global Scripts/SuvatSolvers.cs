using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuvatSolvers : MonoBehaviour
{
    //Ran when a Suvat calculation is too occur on the passed in instance of Particle.
    public static newParticle FindEquation(newParticle values)
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
        while (((values.numberOfInputs[0] != 5 && values.numberOfInputs[1] != 5 && values.numberOfInputs[2] != 5) || (values.invalidInputs[0] == false || values.invalidInputs[1] == false || values.invalidInputs[2] == false)) && (j < maxj))
        {
            //Dimention has 3 or more inputs
            if (values.numberOfInputs[0] >= 3)
            {
                i = 0;
                //Calls the corressponding equation
                Equations[values.key[i]]();

            }
            //Dimention has 3 or more inputs
            if (values.numberOfInputs[1] >= 3)
            {
                i = 1;
                //Calls the corressponding equation
                Equations[values.key[i]]();
            }
            //Dimention has 3 or more inputs
            if (values.numberOfInputs[2] >= 3)
            {
                i = 2;
                //Calls the corressponding equation
                Equations[values.key[i]]();
            }
            j++;
        }
        return values;
    }
    //S = (v^2-u^2) / 2a
    private static void RanOn_01110(newParticle values, int dimention)
    {
        //cannot devide by 0.Therefore would be invalid input
        if (values.acceleration[dimention] != 0)
        {
            //S = (v^2-u^2) / 2a
            Vector3 temp = values.displacement;
            temp[dimention] = (Mathf.Pow(values.currentVelocity[dimention], 2) - Mathf.Pow(values.initialVelocity[dimention], 2)) / (2 * values.acceleration[dimention]);
            values.displacement = temp;
            values.key[dimention] = ReplaceAtIndex(0, '1', values.key[dimention]);
        }
        else
        {
            values.invalidInputs[dimention] = true;
        }
    }

    //S = vt - 1/2 a t^2
    private static void RanOn_00111(newParticle values, int dimention)
    {
        //S = vt - 1/2 a t^2
        Vector3 temp = values.displacement;
        temp[dimention] = values.currentVelocity[dimention] * values.motionTime - 0.5f * (values.acceleration[dimention] * Mathf.Pow(values.motionTime, 2));
        values.displacement = temp;
        values.key[dimention] = ReplaceAtIndex(0, '1', values.key[dimention]);
    }

    //sets inValidInput = true (all inputs calculated)
    private static void RanOn_11111(newParticle values, int dimention)
    {
        values.invalidInputs[dimention] = true;
    }
    // s = ut + 1/2 * a * t^2
    public static void RanOn_01011(newParticle values, int dimention)
    {
        Vector3 temp = values.displacement;
        temp[dimention] = values.initialVelocity[dimention] * values.motionTime + 0.5f * values.acceleration[dimention] * Mathf.Pow(values.motionTime, 2);
        values.displacement = temp;
        values.key[dimention] = ReplaceAtIndex(0, '1', values.key[dimention]);
    }
    // s = 1/2 (u + v) t
    public static void RanOn_01101(newParticle values, int dimention)
    {
        Vector3 temp = values.displacement;
        temp[dimention] = 0.5f * (values.initialVelocity[dimention] + values.currentVelocity[dimention]) * values.motionTime;
        values.displacement = temp;
        values.key[dimention] = ReplaceAtIndex(0, '1', values.key[dimention]);
    }
    //Input condition used 01101
    public static void RanOn_01111(newParticle values, int dimention)
    {
        RanOn_01101(values, dimention);
    }
    // s = ut + 1/2 * a * t^2 rearranged for u
    public static void RanOn_10011(newParticle values, int dimention)
    {
        Vector3 temp = values.initialVelocity;
        temp[dimention] = (values.displacement[dimention] / values.motionTime) - 0.5f * (values.acceleration[dimention] * values.motionTime);
        values.initialVelocity = temp;
        values.key[dimention] = ReplaceAtIndex(1, '1', values.key[dimention]);
    }
    // s = 1/2 (u + v)t rearranged for u
    public static void RanOn_10101(newParticle values, int dimention)
    {
        //Cannot divide by 0
        if (values.motionTime == 0)
        {
            values.invalidInputs[dimention] = true;
        }
        else
        {
            Vector3 temp = values.initialVelocity;
            temp[dimention] = 2 * (values.displacement[dimention] / values.motionTime) - values.currentVelocity[dimention];
            values.initialVelocity = temp;
            values.key[dimention] = ReplaceAtIndex(1, '1', values.key[dimention]);
        }
    }
    // V^2 = u^2 + 2as reaaragned for u
    public static void RanOn_10110(newParticle values, int dimention)
    {
        float InsideRoot = Mathf.Pow(values.currentVelocity[dimention], 2) - 2 * values.acceleration[dimention] * values.displacement[dimention];
        //Square root must be positive
        if (InsideRoot < 0)
        {
            values.invalidInputs[dimention] = true;
        }
        else
        {
            Vector3 temp = values.initialVelocity;
            temp[dimention] = Mathf.Sqrt(InsideRoot);
            values.initialVelocity = temp;
            values.key[dimention] = ReplaceAtIndex(1, '1', values.key[dimention]);
        }
    }
    //v = u + at , rearranged for u
    public static void RanOn_10111(newParticle values, int dimention)
    {
        Vector3 temp = values.initialVelocity;
        temp[dimention] = values.currentVelocity[dimention] - values.acceleration[dimention] * values.motionTime;
        values.initialVelocity = temp;
        values.key[dimention] = ReplaceAtIndex(1, '1', values.key[dimention]);
    }

    //s = 0.5 * *u+v)t , rearranged for v
    public static void RanOn_11001(newParticle values, int dimention)
    {
        //Cannot divide by 0 therefore will be invalid input.
        if (values.motionTime == 0)
        {
            values.invalidInputs[dimention] = true;
        }
        else
        {
            Vector3 temp = values.currentVelocity;
            temp[dimention] = 2 * (values.displacement[dimention] / values.motionTime) - values.initialVelocity[dimention];
            values.currentVelocity = temp;
            values.key[dimention] = ReplaceAtIndex(2, '1', values.key[dimention]);
        }
    }
    // V^2 = u^2 + 2as
    public static void RanOn_11010(newParticle values, int dimention)
    {
        float insideRoot = Mathf.Pow(values.initialVelocity[dimention], 2) + (2 * values.acceleration[dimention] * values.displacement[dimention]);
        //Square root must be positive.Therefore invalid input if not true
        if (insideRoot < 0)
        {
            values.invalidInputs[dimention] = true;
        }
        else
        {
            Vector3 temp = values.currentVelocity;
            temp[dimention] = MyMaths.SquareRoot(insideRoot);
            values.currentVelocity = temp;
            //Checking direction which is lost by squaring
            if (values.acceleration[dimention] < 0 && values.displacement[dimention] <= 0)
            {
                temp[dimention] = -values.currentVelocity[dimention];
                values.currentVelocity = temp;
            }
            values.key[dimention] = ReplaceAtIndex(2, '1', values.key[dimention]);
        }
    }
    //V = u + at
    public static void RanOn_11011(newParticle values, int dimention)
    {
        Vector3 temp = values.currentVelocity;
        temp[dimention] = values.initialVelocity[dimention] + values.acceleration[dimention] * values.motionTime;
        values.currentVelocity = temp;
        values.key[dimention] = ReplaceAtIndex(2, '1', values.key[dimention]);
    }
    // V^2 = u^2 + 2as reaaragned for a
    public static void RanOn_11100(newParticle values, int dimention)
    {
        //Cannot divide by 0 which would cause invalid inputs
        if (values.displacement[dimention] == 0)
        {
            values.invalidInputs[dimention] = true;
        }
        else
        {
            Vector3 temp = values.acceleration;
            temp[dimention] = (Mathf.Pow(values.currentVelocity[dimention], 2) - Mathf.Pow(values.initialVelocity[dimention], 2)) / (2 * values.displacement[dimention]);
            values.acceleration = temp;
            values.key[dimention] = ReplaceAtIndex(3, '1', values.key[dimention]);
        }
    }
    //V = u + at , rearranged for a
    public static void RanOn_11101(newParticle values, int dimention)
    {
        //Cannot divide by 0
        if (values.motionTime == 0)
        {
            values.invalidInputs[dimention] = true;
        }
        else
        {
            Vector3 temp = values.acceleration;
            temp[dimention] = (values.currentVelocity[dimention] - values.initialVelocity[dimention]) / values.motionTime;
            values.acceleration = temp;
            values.key[dimention] = ReplaceAtIndex(3, '1', values.key[dimention]);
        }
    }
    //V = u + at , rearranged for t
    public static void RanOn_11110(newParticle values, int dimention)
    {
        //Diffrent equations are used depending on acceleration
        if (values.acceleration[dimention] == 0)
        {
            //Cannot divide by 0
            if ((values.initialVelocity[dimention] + values.currentVelocity[dimention]) != 0)
            {
                values.motionTime = 2 * values.displacement[dimention] / (values.initialVelocity[dimention] + values.currentVelocity[dimention]);
                values.key[0] = ReplaceAtIndex(4, '1', values.key[0]);
                values.key[1] = ReplaceAtIndex(4, '1', values.key[1]);
                values.key[2] = ReplaceAtIndex(4, '1', values.key[2]);
            }
            else
            {
                values.invalidInputs[dimention] = true;
            }
        }
        else
        {
            //Time calculations change the Key in 3 dimentions because time is shared
            values.motionTime = (values.currentVelocity[dimention] - values.initialVelocity[dimention]) / values.acceleration[dimention];
            values.key[0] = ReplaceAtIndex(4, '1', values.key[0]);
            values.key[1] = ReplaceAtIndex(4, '1', values.key[1]);
            values.key[2] = ReplaceAtIndex(4, '1', values.key[2]);
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