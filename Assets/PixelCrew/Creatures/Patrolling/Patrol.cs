using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Patrolling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}
