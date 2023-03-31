using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    //y = mx + c
    public float gradient;
    public float constant;

    public float perp_gradient;
    public float perp_constant;

    public Vector2 startPoint;
    public Vector2 endPoint;

    public Line(Vector2 start, Vector2 end) 
    {
        CalculateLine(start, end);
        startPoint = start;
        endPoint = end;
    }

    public Line(float p_gradient, float p_constant) 
    {
        gradient = p_gradient;
        constant = p_constant;
    }

    public void CalculateLine(Vector2 start, Vector2 end) 
    {
        float m = (end.x - start.x) / (end.y - start.y);
        //y - mx = c
        float c = start.y - (m * start.x);

        gradient = m;
        constant = c;
    }

    public Line CalculatePerpendicularLine(Vector2 intercept) 
    {
        float m = -1 / gradient; //negative recipricol
        //y - mx = c
        float c = intercept.y - (m * intercept.x);

        perp_gradient = m;
        perp_constant = c;

        Line perp_line = new Line(m, c);
        return perp_line;
    }

    public Vector2 FindLineInterecpt(Line line) 
    {
        //y = m1x + c1 
        //y = m2x + c2

        //solve for x 
        //m1x + c1 = m2x + c2
        //m1x - m2x = c2 - c1
        //(m1 - m2)x = c2 - c1
        //x = c2 - c1 / (m1 - m2)
        //solve for y
        //y = m1(sub x in) + c1

        float x = (constant - line.constant) / (gradient - line.gradient);
        float y = gradient * x + constant;

        return new Vector2(x, y);
    }
}
