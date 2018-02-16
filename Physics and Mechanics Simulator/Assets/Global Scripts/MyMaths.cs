using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMaths : MonoBehaviour
{
    //Goes through each element inside of the vector
    //rounds such element's value to the next integer
    public static Vector3 Vector_Ceil(Vector3 v)
    {
        //The vector to be returned
        Vector3 newVector = new Vector3();
        for (int i = 0; i < 3; i++)
        {
            //rounds the element's value up
            newVector[i] = Mathf.Ceil(v[i]);
        }
        return newVector;
    }

    //Returns the magnitude (no direction) of the value inputted into parameters
    public static float Magnitude(float value)
    {
        //Determines if value is negative
        if (value < 0)
        {
            //negative times a negative is positive
            return -value;
        }
        //if the value is not negative then it does not need additional manipulation
        else
        {
            return value;
        }
    }

    //Returns the square root of the value (approximated)
    //Uses the babylonian algorithm.
    //Infomation can be found at : en.wikipedia.org/wiki/Methods_of_computing_square_roots#Babylonian_method
    public static float SquareRoot(float x)
    {
        //Cannot square root a negative awnser and so 0 is returned.
        if (x <= 0)
        {
            return 0;
        }
        //Arbitary first guess
        float guess = 1.0f;
        float current = 2.0f;
        //The itteration shall be completed when the current = guess.
        //This occurs because of the 32 bit floating point binary value stored.
        //Could be changed to 3-6 itterations for performance improvements
        while (guess != current)
        {
            guess = current;
            //Performs algorithm of babylonian apporximation
            current = (float)(0.5 * (guess + x / guess));
        }
        return current;
    }
    //Clamps a value between the minimum and maximum
    public static float Clamp(float value, float min, float max)
    {
        // makes sure that min is smaller than maximum value for clamp
        if (min > max)
        {
            //swaps value
            float temp = min;
            min = max;
            max = temp;
        }
        if (value > max)
        {
            value = max;
        }
        else if (value < min)
        {
            value = min;
        }
        return value;
    }

    //Vector_magnitude requires an overload for List<float> n dimentional vetors and Vector3's
    //Returns the magnitude of the vector/list
    public static float Vector_Magnitude(List<float> a)
    {
        //Stores the magnitude squared.Used during itteration
        float SquaredMagnitude = 0;
        //Adds the squared magnitude for each element
        for (int i = 0; i < a.Count; i++)
        {
            SquaredMagnitude += Mathf.Pow(a[i], 2);
        }
        //Magnitude is the square root of the squaredmagnitude
        return SquareRoot(SquaredMagnitude);
    }
    //Returns the magnitude of a Vector (lenght of the vector)
    public static float Vector_Magnitude(Vector3 a)
    {
        float SquaredMagnitude = 0;
        for (int i = 0; i < 3; i++)
        {
            //incriments value by the square of element
            SquaredMagnitude += Mathf.Pow(a[i], 2);
        }
        //magnitude is the square root of squared magnitude
        return SquareRoot(SquaredMagnitude);
    }

    //Returns the value of the scalar / dot product of two lists a and b
    public static float DotProduct_Value(List<float> a, List<float> b)
    {
        //Making sure they have the same lenght of vector
        if (a.Count != b.Count)
        {
            //If the lenght is not the same the value is an error therefore 0 is returned.
            return 0;
        }
        else
        {
            float counter = 0;
            //For every element in the vectors
            for (int i = 0; i < a.Count; i++)
            {
                //Multiply the elements
                counter += a[i] * b[i];
            }
            //value of scalar product
            return counter;
        }
    }
    //Overload for vector3 type insted of list<float>
    //Returns value of dot product / scalar product
    public static float DotProduct_Value(Vector3 a, Vector3 b)
    {
        float counter = 0;
        for (int i = 0; i < 3; i++)
        {
            counter += a[i] * b[i];
        }
        return counter;
    }

    //returns the angle between two vectors(lists).
    public static float DotProduct_Angle(List<float> a, List<float> b)
    {
        //Vectors must have same dimentions
        if (a.Count != b.Count)
        {
            return 0;
        }
        else
        {
            float Cosangle = DotProduct_Value(a, b)/(Vector_Magnitude(a) * Vector_Magnitude(b));
            //arccos means cos^-1 so inverse of cos
            return Mathf.Acos(Cosangle);
        }
    }
}