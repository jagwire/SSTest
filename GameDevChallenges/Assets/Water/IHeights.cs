using UnityEngine;
using System.Collections;

public interface IHeights{
	float this [int x, int z] {
		get;
		set;
	}
}
