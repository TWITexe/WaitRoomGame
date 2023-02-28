using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableStorage : MonoBehaviour
{
    public string ChatName { get; set; } = "-"; // Получаем ник подписчика чата
    public string Message { get; set; } = "-";   // Получаем сообщение от подписчика сверху
}
