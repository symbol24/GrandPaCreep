using UnityEngine;
using System.Collections;

public class Candy_Controller : MonoBehaviour {
	public int scoreValue;
	public string candyName;
	public enum allCandyTypes{
		good,bad
	}
	public allCandyTypes thisCandyType;
}
