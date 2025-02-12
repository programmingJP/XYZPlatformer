﻿using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace PixelCrew
{
   public class HeroInputReader : MonoBehaviour
   {
      [SerializeField] private Creatures.Hero _hero;
      
      public void OnMovement(InputAction.CallbackContext context)
      {
         Vector2 direction = context.ReadValue<Vector2>();
      
         _hero.SetDirection(direction);
      }

      public void OnInteract(InputAction.CallbackContext context)
      {
         if (context.canceled)
         {
            _hero.Interact();
         }
      }

      public void OnAttack(InputAction.CallbackContext context)
      {
         if (context.canceled)
         {
            _hero.Attack();
         }
      }

      public void OnDash(InputAction.CallbackContext context)
      {
         if (context.started)
         {
            _hero.Dash();
         }
      }
   }
}
