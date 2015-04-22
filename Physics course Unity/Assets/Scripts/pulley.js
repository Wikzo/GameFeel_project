@script RequireComponent(ConfigurableJoint)
@script RequireComponent(Rigidbody)
@script RequireComponent(LineRenderer)
#pragma strict

var ropeLength : float;
var otherEnd : GameObject;

var minLimit : float = 0.1; // if limit == 0, then it acts as a Fixed Joint!

private var originalPosition : Vector3;
private var nextLimit : float;

private var joint : ConfigurableJoint;
private var rigid : Rigidbody;
private var otherJoint : ConfigurableJoint;
private var otherPulley : pulley;


function Start () 
{
	originalPosition = transform.position;
    joint = GetComponent(ConfigurableJoint);
    rigid = GetComponent(Rigidbody);
	nextLimit = joint.linearLimit.limit;
    if (null != otherEnd)
	{
		otherJoint = otherEnd.GetComponent(ConfigurableJoint);
		otherPulley = otherEnd.GetComponent(pulley);
		ropeLength = otherPulley.ropeLength;
	}
	else
	{
		print("Warning: Script pulley requires otherEnd to be set to a GameObject.");
	}
}

function GetActualDistance() : float
{
	return Vector3.Distance(transform.position, originalPosition);
}

function FixedUpdate () 
{
	rigid.WakeUp(); // make sure Rigidbody is ready (default: changing spring limits doesn't update Rigidbodies)
	if (null != otherEnd)
	{
		var thisActualDistance : float = GetActualDistance();
		var otherActualDistance : float = otherPulley.GetActualDistance();

		if (thisActualDistance > joint.linearLimit.limit && 
			otherActualDistance > otherJoint.linearLimit.limit) 
			// both ends stretched
		{
			// adjust the limits such that both ends are stretched equally
			// (this transfers part of the force from one end to the other)
			//nextLimit = thisActualDistance - 
			//	(thisActualDistance + otherActualDistance - ropeLength) / 2.0; 
			//var otherNextLimit : float = otherActualDistance - 
			//	(thisActualDistance + otherActualDistance - ropeLength) / 2.0;  
			nextLimit = thisActualDistance - 
				(otherActualDistance - otherJoint.linearLimit.limit);
			var otherNextLimit : float = otherActualDistance - 
				(thisActualDistance - joint.linearLimit.limit);  
			
			if (nextLimit < minLimit) 
			{
				nextLimit = minLimit;
				otherNextLimit = ropeLength - minLimit;
			}
			if (otherNextLimit < minLimit)
			{
				otherNextLimit = minLimit;
				nextLimit = Mathf.Max(minLimit, ropeLength - minLimit);
			}
		}
		else if (thisActualDistance <= joint.linearLimit.limit && 
			otherActualDistance > otherJoint.linearLimit.limit) 
			// only other end is stretched
		{
			// set this limit such that we are still not stretched
			nextLimit = Mathf.Max(minLimit, thisActualDistance);
		}
		else if (thisActualDistance > joint.linearLimit.limit && 
			otherActualDistance <= otherJoint.linearLimit.limit) 
			// only this end is stretched
		{
			// set this limit to the maximum allowed by the ropeLength 
			// without putting any force on the other end
			nextLimit = Mathf.Max(minLimit, ropeLength - otherActualDistance);
		}
		else // none of the ends is stretched 
		{
			// distribute the additional rope length between both ends
			nextLimit = Mathf.Max(minLimit, thisActualDistance + 
				0.5 * (ropeLength - thisActualDistance - otherActualDistance));
		}
		
		// update limits
		if (otherPulley.nextLimit != otherJoint.linearLimit.limit)	
			// has the other end been updated already?
			// (Otherwise we do nothing here and 
			// update the limits in the FixedUpdate function of the otherEnd.) 
		{
			// then it is time to update the limits 
			joint.linearLimit.limit = nextLimit;
			otherJoint.linearLimit.limit = otherPulley.nextLimit;
		}
	}
}

// function for rendering ropes
function Update()
{
	var lines : LineRenderer = GetComponent(LineRenderer);

	if (null != lines)
	{
		lines.SetPosition(0, originalPosition);
		lines.SetPosition(1, transform.position);
	}
}
