using System;
using UnityEngine;
public class StraightTra:Trajectory
{

   public override void Update()
   {
        Vector3 realSpeed=speed * Time.deltaTime;
        bulletTransform.position += realSpeed;
   }
}
