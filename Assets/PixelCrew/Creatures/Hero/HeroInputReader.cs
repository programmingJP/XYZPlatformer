using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace PixelCrew.Creatures.Hero
{
   public class HeroInputReader : MonoBehaviour
   {
      [SerializeField] private Creatures.Hero.Hero _hero;
      
      public void OnMovement(InputAction.CallbackContext context)
      {
         Vector2 direction = context.ReadValue<Vector2>();
      
         _hero.SetDirection(direction);
      }

      public void OnInteract(InputAction.CallbackContext context)
      {
         if (context.performed)
         {
            _hero.Interact();
         }
      }

      public void OnAttack(InputAction.CallbackContext context)
      {
         if (context.performed)
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

      public void OnThrow(InputAction.CallbackContext context)
      {
         if (context.started)
         {
            _hero.StartThrowing();
         }

         if (context.canceled)
         {
            _hero.PerformThrowing();
         }
      }
   }
}
