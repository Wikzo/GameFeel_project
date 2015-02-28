 #pragma strict
 
// http://answers.unity3d.com/questions/320689/derive-an-equation-from-a-graph.html

 public var curveForceA : AnimationCurve;
 

var timeToReachMaxSpeed : float = 200;
@Range (0.0, 200)
public var time : float;

var maxSpeed : float = 10;

var velocity : float;

private var timeNormalized : float = 1;



 
 function Update() 
 {
     	timeNormalized = time/timeToReachMaxSpeed;
		velocity = curveForceA.Evaluate( timeNormalized ) * maxSpeed;

 }