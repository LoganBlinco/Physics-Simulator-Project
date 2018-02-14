using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphMaker : MonoBehaviour
{
    #region Variable Declaration

    //Reference to the prefabs for points and lines
    public GameObject pointPrefab;
    public GameObject linePrefab;

    public string _tag = "GraphElement";

    //List containing the gameobjects of the points which get created and plotted
    public List<GameObject> pointsObjects = new List<GameObject>();

    //Horizontal height of widget
    float HorizontalHeight;
    //Vertical height of widget
    float VerticalHeight;

    //Padding variables storing the percentage / 100
    //For example a 5% padding is represented by 0.05f
    //Done to prevent additional manipulations to variable
    float topPaddingPercent = 0.05f;
    float bottomPaddingPercent = 0.05f;
    float leftPaddingPercent = 0.05f;
    float rightPaddingPercent = 0.05f;

    //List stores the points which are to be plotted
    List<Vector2> points = new List<Vector2>();
    float minX = 0;
    float minY = 0;
    float maxX = 0;
    float maxY = 0;

    float axisWidth = 2.0f;
    float lineWidth = 1.5f;

    //Ratio between the vertical and horizontal height , including padding , and (max - min)
    float xRatio = 0;
    float yRatio = 0;

    #endregion

    #region Initial Methods
    //Method which organised the process of creating a graph
    public void CreateGraph(List<Vector2> pointsToPlot)
    {
        InitializeVariables();
        points = pointsToPlot;


        //Gets the height and width from gameobject attached to this script
        HorizontalHeight = transform.GetComponent<RectTransform>().rect.width;
        VerticalHeight = transform.GetComponent<RectTransform>().rect.height;

        //Sorts the vectors in the list variable [points] so that the x values are in assending order
        Sort(ref points);

        //minimum or maximum value in the list [points] at dimention 0 or 1 (x or y)
        minX = GetMin(points, 0);
        minY = GetMin(points, 1);
        maxX = GetMax(points, 0);
        maxY = GetMax(points, 1);

        //Calculates the values for the xRatio and yRatio variabels
        CalculateRatio();

        CreateYAxis();
        CreateXAxis();
        //Plotes points onto the canvas
        PlotPoints();
        //Plots lines between the points onto the canvas
        PlotLines();
    }
    //Resets values of variables to defaults
    private void InitializeVariables()
    {
        //Destroyes objects create by the grapher
        DestroyObejctsWithTag(_tag);
        pointsObjects = new List<GameObject>();
        points = new List<Vector2>();
        minX = 0;
        minY = 0;
        maxX = 0;
        maxY = 0;
        xRatio = 0;
        yRatio = 0;
    }

    //Destroys anyobject with the input tag parameter in the scene
    public static void DestroyObejctsWithTag(string tag)
    {
        try
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Destroy(gameObjects[i]);
            }
        }
        catch(UnityException)
        {

        }
    }
    #endregion

    #region Axis Creation
    private void CreateYAxis()
    {
        if (minX < 0 && maxX < 0)
        {
            maxX = 0;
        }
        else if (0 < minX && 0 < maxX)
        {
            minX = 0;
        }
        float sizeOfWidget = VerticalHeight * (1 - (bottomPaddingPercent + topPaddingPercent));
        Vector3 position = new Vector3(
            (transform.position.x + HorizontalHeight * leftPaddingPercent) + (0 - minX) * xRatio,
            (transform.position.y + VerticalHeight * bottomPaddingPercent),
            0.0f);

        CreateLine(sizeOfWidget, axisWidth, 90f, position);
    }

    private void CreateXAxis()
    {
        if (minY < 0 && maxY < 0)
        {
            maxY = 0;
        }
        else if (0 < minY && 0 < maxY)
        {
            minY = 0;
        }
        float sizeOfWidget = HorizontalHeight * (1 - (leftPaddingPercent + rightPaddingPercent));
        Vector3 position = new Vector3(
            (transform.position.x + HorizontalHeight * leftPaddingPercent),
            (transform.position.y + VerticalHeight * bottomPaddingPercent) + (0 - minY) * yRatio,
            0.0f);

        CreateLine(sizeOfWidget, axisWidth, 0f, position);
    }

    #endregion

    #region Line Creation
    //Plots lines between points
    private void PlotLines()
    {
        for (int i = 0; i < pointsObjects.Count - 1; i++)
        {
            //Change in position between the two points 
            Vector3 delta = -(pointsObjects[i].transform.position - pointsObjects[i + 1].transform.position);

            //Calculates angle between first and second point
            float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            //Position to begin the line
            Vector3 Position = pointsObjects[i].transform.position;
            //Creates a ling with size , width and angle from a position
            CreateLine(delta.magnitude, lineWidth, angle, Position);
        }
    }

    //Creates a ling with size , width and angle from a position
    private void CreateLine(float size , float lineWidth , float angle, Vector3 Position)
    {
        //Creates a prefab of linePrefab
        GameObject tempLine = Instantiate(linePrefab) as GameObject;
        //Sets parant to the gameobject attached to the script
        tempLine.transform.SetParent(transform);

        //Changes tag so can be destroyed
        tempLine.tag = _tag;

        //Reference to the Rect Transform of the line
        RectTransform imageRectTransform = tempLine.GetComponent<RectTransform>();

        //Changes the X and Y lenghts
        imageRectTransform.sizeDelta = new Vector2(size, lineWidth);
        //Changes pivot location of the gameobject
        imageRectTransform.pivot = new Vector2(0, 0.5f);
        //Puts gameobject to location of the first point
        imageRectTransform.position = Position;
        //Changes angle so that the line's direction is towards the second points
        imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    #endregion

    #region Setup Calculations

    //Sorts the vectors in the list variable [points] so that the x values are in assending order
    //Uses the insertion sort algorithm
    private void Sort(ref List<Vector2> points)
    {
        Vector2 temp;
        // foreach(int i in a)
        for (int i = 1; i <= points.Count; i++)
        {
            for (int j = 0; j < points.Count - i; j++)
            {
                if (points[j].x > points[j + 1].x)
                {
                    temp = points[j];
                    points[j] = points[j + 1];
                    points[j + 1] = temp;
                }
            }
        }
    }
    //Plots points onto the canvas
    private void PlotPoints()
    {
        //For every point to plot
        for (int i = 0;i<points.Count;i++)
        {
            //Adjusted position to fit widget
            Vector3 position = new Vector3(
                (transform.position.x + HorizontalHeight * leftPaddingPercent) + (points[i].x - minX) * xRatio,
                (transform.position.y + VerticalHeight * bottomPaddingPercent) + (points[i].y - minY) * yRatio,
                0.0f);

            CreatePoint(position);
        }
    }

    private void CreatePoint(Vector3 position)
    {
        //Creates the prefab using the pointPrefab at location = position with no rotation
        GameObject temp = Instantiate(pointPrefab, position, Quaternion.identity) as GameObject;
        //Parant is the object attached to this script (which is a sub parant of canvas allowing it to be seen)
        temp.transform.SetParent(transform);
        temp.name = "X = " + position.x.ToString();
        temp.tag = _tag;
        //adds gameobject to the pointsObjects lists
        pointsObjects.Add(temp);

    }

    //Calculates the x and y ratio
    private void CalculateRatio()
    {
        //Equations from mathematical graphing model
        if (maxX != minX)
        {
            xRatio = HorizontalHeight * (1 - (leftPaddingPercent + rightPaddingPercent)) / (maxX - minX);
        }
        else
        {
            xRatio = 0;
        }
        if (maxY != minY)
        {
            yRatio = VerticalHeight * (1 - (topPaddingPercent + bottomPaddingPercent)) / (maxY - minY);
        }
        else
        {
            yRatio = 0;
        }
        
    }
    //Returns maximum value in the specificied dimention
    private float GetMax(List<Vector2> points, int dimention)
    {
        float currentMax = 0;
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i][dimention] > currentMax)
            {
                currentMax = points[i][dimention];
            }
        }
        return currentMax;
    }
    //Returns minimum value in the specificied dimention
    private float GetMin(List<Vector2> points, int dimention)
    {
        float currentMin = 0;
        for (int i =0;i<points.Count;i++)
        {
            if (points[i][dimention] < currentMin)
            {
                currentMin = points[i][dimention];
            }
        }
        return currentMin;
    }

    #endregion
}