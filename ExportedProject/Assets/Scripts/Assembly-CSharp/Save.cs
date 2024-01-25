using System;
using System.Collections.Generic;

[Serializable]
public class Save
{
	public List<int> livingTargetPositions = new List<int>();

	public List<int> livingTargetsTypes = new List<int>();

	public int hits;

	public int shots;
}
