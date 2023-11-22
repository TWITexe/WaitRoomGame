using System;
using System.Net.Sockets;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TwitchChat : MonoBehaviour, IConnectChat
{
    // Клиент твича для дальнейшей работы
    TcpClient twitchClient;                                 
    private StreamReader reader;                                    
    private StreamWriter writer;
    
    public Text textNickname;
    public Text textMessage;
    // Имя пользователя, чей oauth будет использоваться    
    public string username;
    // Oauth ключ с источника: twitchapps.com/tmi
    public string password;                                 
    // Ник человека на Twitch, с которого будет браться информация о чате.
    public string channelName;                              
    
    // Используется для переподключения к серверу
    private float reconnectTimer;                           
    private float reconnectAfter;
    // Счётчик пришедших сообщений
    private int messageCounter = 0;

    // Таймер для считывания сообщений
    [SerializeField] private GameObject timerObject;
    private TimerForChatСommands timerForChatСommands;
    
    // Выводим сообщение на экран
    [SerializeField]private CloudMessageMoving cloudMessageMoving = new CloudMessageMoving();
   
    

    

    void Start()
    {
        timerForChatСommands = timerObject.GetComponent<TimerForChatСommands>();
        reconnectAfter = 60.0f;
        Connect();
    }
    
    void Update()
    {
        try
        {
            // Проверяем Connect
            if (!twitchClient.Connected)                        
            {
                print("Can't connect");
                Connect();
            }
            if (twitchClient.Available == 0)
            {
                reconnectTimer += Time.deltaTime;
            }

            if (twitchClient.Available == 0 && reconnectTimer >= reconnectAfter)
            {
                Connect();
                reconnectTimer = 0.0f;
            }
            ReadChat();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception = " + e);
        }
    }
    
    
    public void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);                  
        // Заполняем данные для подключения
        // Создаём для передачи инфы Twitch'у reader и writer
        reader = new StreamReader(twitchClient.GetStream());                       
        writer = new StreamWriter(twitchClient.GetStream());
        // Передаём данные Twitch'у 
        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        // Здесь используется особая форма записи.
        writer.WriteLine("USER " + username + " 8 *:" + username);                 
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
        
    }

    private void ReadChat()
    {
        if (twitchClient.Available > 0)
        {
            if (!timerForChatСommands.TimerOn)
            {
                // Получаем сообщение от подписчика сверху
                VariableStorage.Message = reader.ReadLine();
                // Проверяем, написано ли сообщение пользователем ( PRIVMSG ).
                if (VariableStorage.Message.Contains("PRIVMSG"))
                {
                    // Получаем никнейм пользователя, написавшего сообщение
                    int splitPoint = VariableStorage.Message.IndexOf("!", 1);
                    VariableStorage.ChatName = VariableStorage.Message.Substring(0, splitPoint);
                    VariableStorage.ChatName = VariableStorage.ChatName.Substring(1);

                    // Получаем текст сообщения 
                    splitPoint = VariableStorage.Message.IndexOf(":", 1);
                    VariableStorage.Message = VariableStorage.Message.Substring(splitPoint + 1);
                    
                    messageCounter++;
                    if (messageCounter >= 5)
                    {
                        textNickname.text = VariableStorage.ChatName;
                        textMessage.text = "➤" + VariableStorage.Message;
                        messageCounter = 0;
                        cloudMessageMoving.StartMessage();
                    }
                }
            }
        }


    }
    
}
