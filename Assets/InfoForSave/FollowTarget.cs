using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _damping;

    private void LateUpdate()
    {
        Vector3 destination = new Vector3(_target.position.x, _target.position.y,transform.position.z); // тут мы получаем желаемую позицию
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * _damping);//Lerp - это функция интерполяции
        //Lerp принимает в себя два вектора, первый вектор - текущая позиция, вторая - целевая позиция(куда хотим прийти), третий параметр значение  от 0 до 1.
        //0 - текущая позиция, 1 - целевая позиция
        //Интерполяция это нахождение промежуточного значения между двумя значениями в зависимости от того, какое третье значение мы туда поместили
        
        //Можем использовать Mathf.Lerp() при работе с числами
        
        
    }
}
