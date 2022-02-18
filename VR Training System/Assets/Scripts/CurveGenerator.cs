using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is used for creating the curve in the cutting plane 
/// </summary>
public class CurveGenerator : MonoBehaviour
{
	/// <summary>
	/// Is used for creating a curve out of an array of input points
	/// </summary>
	public static Vector3[] CreateCurve(Vector3[] inputPoints, float distBetweenPointsCoefficient)
	{
		//create curve for every dimenson
		AnimationCurve curveX = new AnimationCurve();
		AnimationCurve curveY = new AnimationCurve();
		AnimationCurve curveZ = new AnimationCurve();

		//create keyframe sets for those curves
		Keyframe[] keysX = new Keyframe[inputPoints.Length];
		Keyframe[] keysY = new Keyframe[inputPoints.Length];
		Keyframe[] keysZ = new Keyframe[inputPoints.Length];

		//set keyframes
		for (int i = 0; i < inputPoints.Length; i++)
		{
			keysX[i] = new Keyframe(i, inputPoints[i].x);
			keysY[i] = new Keyframe(i, inputPoints[i].y);
			keysZ[i] = new Keyframe(i, inputPoints[i].z);
		}

		//apply keyframes to curves
		curveX.keys = keysX;
		curveY.keys = keysY;
		curveZ.keys = keysZ;

		//smooth curve tangents
		for (int i = 0; i < inputPoints.Length; i++)
		{
			curveX.SmoothTangents(i, 0);
			curveY.SmoothTangents(i, 0);
			curveZ.SmoothTangents(i, 0);
		}

		//list to write smoothed values to
		List<Vector3> curve = new List<Vector3>();

		//find segments in each section
		for (int i = 0; i < inputPoints.Length; i++)
		{
			//add first point
			curve.Add(inputPoints[i]);

			// for making sure we stay inside array size
			if (i + 1 < inputPoints.Length)
			{
				// get distance to next input point
				float distanceToNext = Vector3.Distance(inputPoints[i], inputPoints[i + 1]);
				// how many points will be created
				int pointCount = (int)(distanceToNext / distBetweenPointsCoefficient);

				//add points
				for (int s = 1; s < pointCount; s++)
				{
					//interpolate time on curve
					float time = ((float)s / (float)pointCount) + (float)i;

					//get value on that time
					Vector3 newPoint = new Vector3(curveX.Evaluate(time), curveY.Evaluate(time), curveZ.Evaluate(time));

					//add new point to list
					curve.Add(newPoint);
				}
			}
		}
		return curve.ToArray();
    }
}
