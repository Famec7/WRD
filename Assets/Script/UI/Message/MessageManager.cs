using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : Singleton<MessageManager>
{
    public Message messagePrefab; // 메시지 프리팹
    public int poolSize = 10; // 초기 풀 크기
    public Transform messageParent; // 메시지 오브젝트의 부모

    private Queue<Message> messagePool = new Queue<Message>();
    protected override void Init()
    {
        ;
    }

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewMessage();
        }
    }

    private Message CreateNewMessage()
    {
        Message newMessage = Instantiate(messagePrefab, messageParent);
        newMessage.gameObject.SetActive(false);
        messagePool.Enqueue(newMessage);
        return newMessage;
    }

    public void ShowMessage(string text, Vector2 position, float displayTime = 2f, float fadeTime = 0.5f)
    {
        Message message = null;

        if (messagePool.Count > 0)
        {
            message = messagePool.Dequeue();
            message.gameObject.SetActive(true);
        }
        else
        {
            message = CreateNewMessage();
            message.gameObject.SetActive(true);
        }

        message.transform.localPosition = position;
        message.Initialize(text, displayTime, fadeTime);
    }

    public void ReturnMessage(Message message)
    {
        message.gameObject.SetActive(false);
        messagePool.Enqueue(message);
    }

}
