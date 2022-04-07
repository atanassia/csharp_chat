using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Supabase;
using Supabase.Realtime;
using Client = Supabase.Client;

namespace AvaloniaDatabase.Model;

public class Database : INotifyPropertyChanged
{
    public Database()
    {
        // В конструкторе создаем массив с сообщениями,
        // в котором будут храниться все строки из таблицы
        Table = new List<Messages>();

        // Подключаемся к базе данных
        string url = "https://wdkpxtwbktvtwootfwai.supabase.co";
        string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Indka3B4dHdia3R2dHdvb3Rmd2FpIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NDQ2NzUwMjksImV4cCI6MTk2MDI1MTAyOX0.4IiXa_igXwbKjghmuKl4P_kQDidsh_yvSkaRphDcrMo";

        Client.InitializeAsync(url, key, new SupabaseOptions
        {
            AutoConnectRealtime = true,
            ShouldInitializeRealtime = true
        });

        // Получаем экземпляр клиента
        Client = Client.Instance;

        // И подписываемся на события изменения в базе данных
        Client.From<Messages>().On(Client.ChannelEventType.All, MessagesChanged);

        LoadData();
    }

    // Клиент для обращения к базе данных
    private Client Client { get; }

    // Массив со студентами из базы
    public IEnumerable<Messages> Table { get; set; }

    // Событие изменения массива для обновления интерфейса
    public event PropertyChangedEventHandler? PropertyChanged;

    // При изменении данных в талице на сервере просто подгружаем данные из нее
    private void MessagesChanged(object sender, SocketResponseEventArgs a)
    {
        LoadData();
    }

    // А вот так просходит загрузка данных из талицы
    // на сервере Supabase в массив нашей программы
    public async void LoadData()
    {
        // Берем данные из таблицы и помещаем их в массив
        var data = await Client.From<Messages>().Get();
        Table = data.Models;

        //Client.From<Messages>().Insert()

        // Вызов этой функции необходим для автоматического обновления
        // интерфейса программы при изменении данных в массиве со студентами
        OnPropertyChanged(nameof(Table));
    }

    // Реализация интерфейса INotifyPropertyChanged необходима для обновления формы программы
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}