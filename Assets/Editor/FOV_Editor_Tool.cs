using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class FOV_Editor_Tool : Editor
{
    private void OnSceneGUI()
    {
        Enemy Enemy = (Enemy)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(Enemy.transform.position, Vector3.up, Vector3.forward, 360, Enemy.VisRadius);
        Vector3 View_Angle_1 = Driection_From_Angle(Enemy.transform.eulerAngles.y, -Enemy.VisAngle / 2);
        Vector3 View_Angle_2 = Driection_From_Angle(Enemy.transform.eulerAngles.y, Enemy.VisAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(Enemy.transform.position, Enemy.transform.position + View_Angle_1 * Enemy.VisRadius);
        Handles.DrawLine(Enemy.transform.position, Enemy.transform.position + View_Angle_2 * Enemy.VisRadius);

        if(Enemy.PlayerInSight)
        {
            Handles.color = Color.green;
            Handles.DrawLine(Enemy.transform.position, Enemy.Target.transform.position);

        }
    }
    private Vector3 Driection_From_Angle(float eulerY, float AngleDegrees)
    {
        AngleDegrees += eulerY;
        return new Vector3 (Mathf.Sin(AngleDegrees*Mathf.Deg2Rad),0,Mathf.Cos(AngleDegrees*Mathf.Deg2Rad));
    }
}

