using UnityEngine;

public class VariableStorage : MonoBehaviour
{
    // Получаем ник подписчика чата
    public string ChatName { get; set; } = "-"; 
    // Получаем сообщение от подписчика сверху
    public string Message { get; set; } = "-";   
}
