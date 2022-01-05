using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actions : MonoBehaviour {

	public Animation anim;
	
	
	void Start () {
		anim.Play("idle");
	}
	
public void BButt_expand() 		//------------ buton
	{
	anim.Play("expand");	
	}
	
public void BButt_join() 		//------------ buton
	{
	anim.Play("join");	
	}
	


}
