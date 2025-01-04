//[SerializeField] private Hero _hero;

/*private void Awake()
   {
      _hero = GetComponent<Hero>();//GetComponent - не бесплатный метод, в данном случае лучше сделать через сериализацию
   }*/

/*private void Update()
{
   float horizontal = Input.GetAxis("Horizontal");
   
   _hero.SetDirection(horizontal);

   if (Input.GetButtonUp("Fire1"))
   {
      _hero.SaySomething();
   }
  
   
   
   /*
   GetKeyDown - когда кнопка только нажата
   GetKey - когда кнопка зажата
   GetKeyUp - когда кнопка была отпущена
   Key - для клавиатуры
   Button - для виртуальных кнопок
   */   

/*if (Input.GetKey(KeyCode.A))
{
   _hero.SetDirection(-1f);
}
else if(Input.GetKey(KeyCode.D))
{
   _hero.SetDirection(1f);
}
else
{
   _hero.SetDirection(0f);
}
}*/
