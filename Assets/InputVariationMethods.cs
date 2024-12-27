using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

//Ивенты не отправляют бродкасты //самый прозрачный способ
//Бродкасты не создают колл беки
//Через код используется своя система, самый выгодный способ

//public class InputVariationMethods : MonoBehaviour
//{
    //[SerializeField] private Hero _hero;

    //private HeroInputAction _inputActions;

    //Подписываемся на события
    /*private void Awake()
    {
        _inputActions = new HeroInputAction();
        _inputActions.Hero.HorizontalMovement.performed += OnHorizontalMovement; //В процессе
        _inputActions.Hero.HorizontalMovement.canceled += OnHorizontalMovement; //Закончилось
      
        _inputActions.Hero.SaySomething.performed += OnSaySomething;
    }*/

    //Когда включается компонент, включаем прослушку событий
    /*private void OnEnable()
    {
        _inputActions.Enable();
    }*/

    /*//Cистема бродкаста сообщений
    //События будут вызываться атоматически, можем сделать их приватными
    private void OnHorizontalMovement(InputValue context)
    {
       float direction = context.Get<float>();
       
       _hero.SetDirection(direction);
    }
 
    private void OnSaySomething(InputValue context)
    {
       _hero.SaySomething();
    }*/
   
    //Вызов через систему ивентов юнити 
    //Данный блок кода использовался так же для инпут системы через код, если мы работает через юнити ивенты, то модификатор доступа должен быть паблик
    /*private void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        float direction = context.ReadValue<float>();
      
        _hero.SetDirection(direction);
    }*/

    /*private void OnSaySomething(InputAction.CallbackContext context)
    {
        //context.started - начало события
        //context.performed - продолжение события
        //context.canceled - закончилось событие
      
        //contex.phase - состояние события(фаза)
      
        if (context.canceled)
        {
            _hero.SaySomething();
        }
    }*/
//}
