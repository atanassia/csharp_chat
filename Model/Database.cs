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
        // � ������������ ������� ������ � �����������,
        // � ������� ����� ��������� ��� ������ �� �������
        Table = new List<Messages>();

        // ������������ � ���� ������
        string url = "https://wdkpxtwbktvtwootfwai.supabase.co";
        string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Indka3B4dHdia3R2dHdvb3Rmd2FpIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NDQ2NzUwMjksImV4cCI6MTk2MDI1MTAyOX0.4IiXa_igXwbKjghmuKl4P_kQDidsh_yvSkaRphDcrMo";

        Client.InitializeAsync(url, key, new SupabaseOptions
        {
            AutoConnectRealtime = true,
            ShouldInitializeRealtime = true
        });

        // �������� ��������� �������
        Client = Client.Instance;

        // � ������������� �� ������� ��������� � ���� ������
        Client.From<Messages>().On(Client.ChannelEventType.All, MessagesChanged);

        LoadData();
    }

    // ������ ��� ��������� � ���� ������
    private Client Client { get; }

    // ������ �� ���������� �� ����
    public IEnumerable<Messages> Table { get; set; }

    // ������� ��������� ������� ��� ���������� ����������
    public event PropertyChangedEventHandler? PropertyChanged;

    // ��� ��������� ������ � ������ �� ������� ������ ���������� ������ �� ���
    private void MessagesChanged(object sender, SocketResponseEventArgs a)
    {
        LoadData();
    }

    // � ��� ��� ��������� �������� ������ �� ������
    // �� ������� Supabase � ������ ����� ���������
    public async void LoadData()
    {
        // ����� ������ �� ������� � �������� �� � ������
        var data = await Client.From<Messages>().Get();
        Table = data.Models;

        //Client.From<Messages>().Insert()

        // ����� ���� ������� ��������� ��� ��������������� ����������
        // ���������� ��������� ��� ��������� ������ � ������� �� ����������
        OnPropertyChanged(nameof(Table));
    }

    // ���������� ���������� INotifyPropertyChanged ���������� ��� ���������� ����� ���������
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}