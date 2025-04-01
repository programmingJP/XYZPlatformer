using Cinemachine;
using UnityEngine;

namespace PixelCrew.Components.LevelManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            vCamera.Follow = FindObjectOfType<Creatures.Hero.Hero>().transform;
        }
    }
}