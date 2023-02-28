using System.Net.Sockets;
using System.IO;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.UI;


public class TwitchChat : MonoBehaviour, IConnectChat
{
    TcpClient twitchClient;                                 // Клиент твича для дальнейшей работы
    private StreamReader reader;                                    
    private StreamWriter writer;
    
    public Text textNickname;
    public Text textMessage;
        
    public string username;                                 // Имя пользователя, чей oauth будет использоваться
    
    public string password;                                 // Oauth ключ с источника: twitchapps.com/tmi
    public string channelName;                              // Ник человека на Twitch, с которого будет браться информация о чате.
    
    
    private float reconnectTimer;                           // Используется для переподключения к серверу
    private float reconnectAfter;

    private int messageCounter = 0;                         // Счётчик пришедших сообщений

    // Полученные данные
    [SerializeField] VariableStorage chatMessage;            // Объект класса ChatMessage для управления данными подписчика в чате
   
    

    

    void Start()
    {
        reconnectAfter = 60.0f;
        Connect();
    }
    
    void Update()
    {
        
        if (!twitchClient.Connected)                        // Проверяем Connect
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

    public void Connect() // *Обязательное поле по интерфейсу
    {
        ConnectChat();
    }
    
    void ConnectChat()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);       // Заполняем данные для подключения
        reader = new StreamReader(twitchClient.GetStream());                       // Создаём для передачи инфы Twitch'у reader и writer
        writer = new StreamWriter(twitchClient.GetStream());                       
        
        // Передаём данные Twitch'у 
        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 *:" + username);                 // Здесь используется особая форма записи.
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
        
        print("Connect");
    }

    public void ReadChat()
    {
        print("InReadChat");
        if (twitchClient.Available > 0)
        {
            
            chatMessage.Message = reader.ReadLine();                                // Получаем сообщение от подписчика сверху
            
            if ( chatMessage.Message.Contains("PRIVMSG"))                           // Проверяем, написано ли сообщение пользователем ( PRIVMSG ).
            {
                //string chatName = chatMessage.chatName;                           // Получаем ник подписчика чата
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
