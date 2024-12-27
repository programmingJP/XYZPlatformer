using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{
   public class HeroInputReader : MonoBehaviour
   {
      [SerializeField] private Hero _hero;
      public void OnHorizontalMovement(InputAction.CallbackContext context)
      {
         float direction = context.ReadValue<float>();
      
         _hero.SetDirection(direction);
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
