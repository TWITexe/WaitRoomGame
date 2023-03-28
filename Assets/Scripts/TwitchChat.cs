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

    // Полученные данные | Объект класса ChatMessage для управления данными подписчика в чате
    [SerializeField] VariableStorage chatMessage;
    
    // Таймер для считывания сообщений
    [SerializeField] private GameObject timerObject;
    private TimerForChatСommands timerForChatСommands;
   
    

    

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
                chatMessage.Message = reader.ReadLine();
                // Проверяем, написано ли сообщение пользователем ( PRIVMSG ).
                if (chatMessage.Message.Contains("PRIVMSG"))
                {
                    // Получаем никнейм пользователя, написавшего сообщение
                    int splitPoint = chatMessage.Message.IndexOf("!", 1);
                    chatMessage.ChatName = chatMessage.Message.Substring(0, splitPoint);
                    chatMessage.ChatName = chatMessage.ChatName.Substring(1);

                    // Получаем текст сообщения 
                    splitPoint = chatMessage.Message.IndexOf(":", 1);
                    chatMessage.Message = chatMessage.Message.Substring(splitPoint + 1);

                    messageCounter++;
                    print(messageCounter);

                    if (messageCounter >= 5)
                    {
                        textNickname.text = chatMessage.ChatName;
                        textMessage.text = "➤" + chatMessage.Message;
                        messageCounter = 0;
                    }
                }
            }
        }


    }
    
}
