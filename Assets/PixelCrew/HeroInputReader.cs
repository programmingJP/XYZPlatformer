using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace PixelCrew
{
   public class HeroInputReader : MonoBehaviour
   {
      [SerializeField] private Hero _hero;
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

      public void OnSaySomething(InputAction.CallbackContext context)
      {
         //context.started - начало события
         //context.performed - продолжение события
         //context.canceled - закончилось событие
      
         //contex.phase - состояние события(фаза)
      
         if (context.canceled)
         {
            _hero.SaySomething();
         }
      }
   
   }
}
