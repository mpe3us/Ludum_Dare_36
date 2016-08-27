using UnityEngine;
using System.Collections;

// Interface for objects which can take damage
public interface IAttackableObject {

	void TakeDamage(int amount);

}
